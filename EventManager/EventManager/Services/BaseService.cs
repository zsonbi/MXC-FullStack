using EventManager.Database;
using EventManager.Shared.Dtos;


namespace EventManager.Services
{
    public abstract class BaseService : DatabaseMethods
    {

        protected BaseService(EventManagerDbContext dbContext)  : base(dbContext)
        {
        }

        protected static Result<T> OkOrNotFound<T>(T? t) where T : BaseDto
        {
            if (t is null)
            {
                return Result<T>.FailNotFound("");
            }
            else
            {
                return Result<T>.Ok(t);
            }
        }
    }
}
