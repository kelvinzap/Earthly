using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Earthly.Authentication.Domain;

public class ApiKey 
{
    [Key]
    public string Id { get; set; }

    public string UserAuthId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool Invalidated     { get; set; }
    public DateTime? InvalidationDate { get; set; }
    [ForeignKey(nameof(UserAuthId))]
    public IdentityUser User { get; set; }
}