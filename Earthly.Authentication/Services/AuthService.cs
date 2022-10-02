using System.Security.Cryptography;
using System.Text;
using Earthly.Authentication.Contracts.V1.Request;
using Earthly.Authentication.Data;
using Earthly.Authentication.Domain;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;

namespace Earthly.Authentication.Services;

public class AuthService : IAuthService
{
    private readonly DataContext _dataContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly string _encryptionKey;
    public AuthService(DataContext dataContext, UserManager<IdentityUser> userManager, string encryptionKey)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _encryptionKey = encryptionKey;
    }

    public async Task<AuthenticationResult> RegisterAsync(CreateUserRequest request)
    {
        var userNameExisting = await _userManager.FindByNameAsync(request.UserName);
        var emailExisting = await _userManager.FindByEmailAsync(request.Email);

        if (userNameExisting != null) 
            return new AuthenticationResult
            {
                Errors = new[] { "The Username already exists" }
            };

        if (emailExisting != null) 
            return new AuthenticationResult
            {
                Errors = new[] { "The email is already registered with another account" }
            };

        var userId = Guid.NewGuid().ToString();
        var newUser = new IdentityUser
        {
            Id = userId,
            UserName = request.UserName,
            Email = request.Email
        };

        var user = await _userManager.CreateAsync(newUser, request.Password);

        if (!user.Succeeded)
            return new AuthenticationResult
            {
                Errors = user.Errors.Select(x => x.Description)
            };

        var apiKey = await GenerateApiKey(userId);

        return new AuthenticationResult
        {
            Success = true,
            ApiKey = apiKey
        };
    }

    
    
    public async Task<AuthenticationResult> LoginAsync(UserLoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return new AuthenticationResult
            {
                Errors = new[] { "The email address is not associated with any account" }
            };

        var userHasValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!userHasValidPassword)
            return new AuthenticationResult
            {
                Errors = new[] { "The Email/Password combination is invalid" }
            };

        var userEncryptedApiKey = await _dataContext.ApiKeys
            .SingleOrDefaultAsync(x => x.UserAuthId == user.Id && !x.Invalidated);
        
        if (userEncryptedApiKey == null)
            return new AuthenticationResult
            {
                Errors = new[] { "Something went wrong" }
            };
        

        var decryptedApiKey = await DecryptApikey(userEncryptedApiKey.Id);
        
        return new AuthenticationResult
        {
            Success = true,
            ApiKey = decryptedApiKey
        };

    }

    public async Task<AuthenticationResult> RegenerateAsync(CreateNewApiKey request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return new AuthenticationResult
            {
                Errors = new[] { "The email address is not associated with any account" }
            };

        var userHasValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!userHasValidPassword)
            return new AuthenticationResult
            {
                Errors = new[] { "The Email/Password combination is invalid" }
            };

        var apiKeyInDb = await
            _dataContext.ApiKeys.SingleOrDefaultAsync(x => x.UserAuthId == user.Id && !x.Invalidated);

        var decryptedKey = await DecryptApikey(apiKeyInDb.Id);

        if (decryptedKey != request.ApiKey)
            return new AuthenticationResult
            {
                Errors = new[] { "Bad request" }
            };

        var apiKey = await GenerateApiKey(user.Id);

        if(apiKey == null)
            return new AuthenticationResult
            {
                Errors = new[] { "Something went wrong" }
            };

        apiKeyInDb.Invalidated = true;
        var invalidated = await _dataContext.SaveChangesAsync();
        
        if(invalidated > 0)
            return new AuthenticationResult
            {
                Success = true,
                ApiKey = apiKey
            };
        
        return new AuthenticationResult
            {
                Errors = new []{"Something went wrong"}
            };
        
    }
    
    private Aes CreateCipher()
    {
        var cipher = Aes.Create();
        cipher.Padding = PaddingMode.ISO10126;
        cipher.Key = Encoding.UTF8.GetBytes(_encryptionKey);

        return cipher;
    }

    private async Task<string> EncryptApiKeyAsync(string apiKey)
    {
        var cipher = CreateCipher();
        var ivKey = new byte[16];
        var generator = RandomNumberGenerator.Create();
        generator.GetBytes(ivKey);

        cipher.IV = ivKey;
        //create the encryptor, convert to bytes and encrypt
        var cryptTransform = cipher.CreateEncryptor();
        var plainText = Encoding.UTF8.GetBytes(apiKey);
        var cipherText = cryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);

        var encryptedApiKey = Convert.ToBase64String(cipherText);
        var ivString = Convert.ToBase64String(ivKey);
        return ivString + encryptedApiKey;
    }

    private async Task<string> DecryptApikey(string userEncryptedApiKey)
    {
        var userApiKeyForEncryption = userEncryptedApiKey[24..];
        var ivKey = userEncryptedApiKey[..24];
        
        
        var cipher = CreateCipher();
        cipher.IV = Convert.FromBase64String(ivKey);
        
        //create the decryptor, convert to bytes and decrypt
        var cryptTransform = cipher.CreateDecryptor();
        var cipherText = Convert.FromBase64String(userApiKeyForEncryption);
        var plainText = cryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
        
        return Encoding.UTF8.GetString(plainText);
    }
    
    private async Task<string> GenerateApiKey(string userId)
    {
        var Key = new byte[32];
        var generator = RandomNumberGenerator.Create();
        generator.GetBytes(Key);

        var apiKey = Convert.ToBase64String(Key);
        
        var encryptedApiKey = await EncryptApiKeyAsync(apiKey);

        var storedApiKey = new ApiKey
        {
            Id = encryptedApiKey,
            CreationDate = DateTime.UtcNow,
            ExpirationDate = DateTime.Now.AddMonths(3),
            Invalidated = false,
            UserAuthId = userId
        };

        await _dataContext.ApiKeys.AddAsync(storedApiKey);
        var created = await _dataContext.SaveChangesAsync();

        return created > 0 ? apiKey : string.Empty;
    }

}