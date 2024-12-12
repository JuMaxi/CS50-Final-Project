using Microsoft.EntityFrameworkCore;

namespace PropagatingKindness.Infra;

public class PlantsDbContext : DbContext
{
    public PlantsDbContext(DbContextOptions<PlantsDbContext> options)
        : base(options)
    {
    }
}
