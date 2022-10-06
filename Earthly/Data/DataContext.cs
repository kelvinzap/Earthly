using Earthly.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Earthly.Data;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Earth> Earth { get; set; }
    
}