using ComponentsStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComponentsStore.DataAccess
{
    public class ComponentsStoreDbContext : DbContext
    {
        public ComponentsStoreDbContext(DbContextOptions<ComponentsStoreDbContext> options)
            : base(options)
        {

        }
        public DbSet<ComponentEntity> Components { get; set; }
    }
}
