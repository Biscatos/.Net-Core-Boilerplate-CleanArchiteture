using Microsoft.EntityFrameworkCore;


namespace Infrastruture.Persistence.Contexts
{
    public sealed class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // appling configutations

        }
    }
}
