using EventManager.Shared.Dtos;

namespace EventManager.Services
{
    public interface IBaseService<T> where T : BaseDto
    {
        public Task<Result<T>> Get(Guid id, CancellationToken ct = default);
    }
}
