using Microsoft.EntityFrameworkCore;
using LuxeFemStore.Models;

namespace LuxeFemStore.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<User> Users { get; set; }
}