using EventManager.Services;
using EventManager.Shared.Database;
using EventManager.Shared.Dtos;
using Microsoft.AspNetCore.Identity;

namespace EventManager.Database
{



    public class DatabaseMethods
    {
        protected readonly EventManagerDbContext Database;

        public DatabaseMethods(EventManagerDbContext dbContext)
        {
            this.Database = dbContext;
        }


        public async Task<T?> Get<T>(Guid? id, CancellationToken ct=default) where T : class, IDbItem
        {
            return await Database.GetDbSet<T>().FindAsync(id, ct);
        }


        public async Task<bool> Create<T>(T instance, bool save = true, CancellationToken ct = default) where T : class, IDbItem
        {
            var existing = await Database.GetDbSet<T>().FindAsync(instance.Id, ct);
            if (existing != null)
            {
                return false;
            }
            instance.CreatedAt = DateTime.UtcNow;
            Database.GetDbSet<T>().Add(instance);
            if (save)
                await Database.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> Update<T>(T instance, CancellationToken ct = default) where T : class, IDbItem
        {
            T? current = await Database.GetDbSet<T>().FindAsync(instance.Id,ct);
            if (current is null)
            {
                return false;
            }
            Database.GetDbSet<T>().Update(instance);
            await Database.SaveChangesAsync(ct);

            return true;
        }


        public async Task<bool> Delete<T>(Guid id, CancellationToken ct = default) where T : class, IDbItem
        {
            T? instance = await Get<T>(id);
            if (instance is null)
            {
                return false;
            }

            return await Delete<T>(instance,ct);
        }

        public async Task<bool> Delete<T>(T instance, CancellationToken ct=default) where T : class, IDbItem
        {
            if (instance is IDeletable deletable)
            {
                
                deletable.DeletedAtUtc = DateTime.UtcNow;
            }
            else
            {
                Database.GetDbSet<T>().Remove(instance);
            }
            await Database.SaveChangesAsync(ct);
            return true;

        }

        protected async Task<Result<U>> GetObject<G, U>(Guid? id) where G : class, IDbItem where U : BaseDto
        {
            G? obj = await Get<G>(id);
            if (obj is null)
            {
                return Result<U>.FailNotFound("");
            }
            return Result<U>.Ok((U)obj.ToDto());
        }

    }

}
