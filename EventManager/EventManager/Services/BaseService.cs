using EventManager.Database;
using EventManager.Shared.Dtos;


namespace EventManager.Services
{
    public abstract class BaseService : DatabaseMethods
    {

        protected BaseService(EventManagerDbContext dbContext)  : base(dbContext)
        {
        }

        /// <summary>
        /// Wrapper to reduce code determines what to give back on a simple querry
        /// </summary>
        /// <typeparam name="T">Type of the Dto</typeparam>
        /// <param name="t">The data</param>
        /// <returns>Result wrapped Dto</returns>
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
