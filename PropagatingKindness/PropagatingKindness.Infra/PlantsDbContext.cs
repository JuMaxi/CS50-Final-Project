using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PropagatingKindness.Domain.Models;


namespace PropagatingKindness.Infra;

public class PlantsDbContext : DbContext
{
    public PlantsDbContext(DbContextOptions<PlantsDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Advert> Adverts { get; set; }

    public DbSet<Advert> Blogs { get; set; }

    public DbSet<Gardening_Help> Gardening_Helps { get; set; }

    public DbSet<Message> Messages { get; set; }

}
