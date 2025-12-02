using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventManager.Shared.Database;

namespace EventManager.Database
{
    public class EventManagerDbContext : IdentityDbContext<User,IdentityRole<Guid>, Guid>
    {
        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
            
        }

        public DbSet<Event> Events { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
             

            });
           
           

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<T> GetDbSet<T>() where T : class, IDbItem
        {
            return Set<T>();
        }

    }
}
