using database.Entities;
using Microsoft.EntityFrameworkCore;

namespace database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }
}