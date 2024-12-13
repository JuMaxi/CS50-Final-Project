using Microsoft.EntityFrameworkCore;
using PropagatingKindness.Domain.Models;


namespace PropagatingKindness.Infra.Db;

public class PlantsDbContext : DbContext
{
    public PlantsDbContext(DbContextOptions<PlantsDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Advert> Adverts { get; set; }

    public DbSet<Advert> Blogs { get; set; }

    public DbSet<GardeningHelp> GardeningHelpRequests { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<Tag> Tags { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<BlogPostContent> PostContents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlantsDbContext).Assembly);
    }

}
