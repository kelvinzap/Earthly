using Earthly.Authentication.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Earthly.Authentication.Data;

public class DataContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<ApiKey> ApiKeys { get; set; }
}