using EventManager.Database;
using EventManager.Shared.Database;
using EventManager.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;

namespace EventManager.Services
{
    public interface IEventService : IBaseService<EventDto>
    {
        public Task<Result<EventDto>> Create(EventDto request, CancellationToken ct = default);

        public Task<Result<IEnumerable<EventDto>>> GetAll(CancellationToken ct = default);
        public Task<Result> Delete(Guid id, CancellationToken ct = default);
        public Task<Result<EventDto>> Update(EventDto request, CancellationToken ct = default);
    }


    public class EventService(EventManagerDbContext database) : BaseService(database), IEventService
    {
        public async Task<Result<EventDto>> Create(EventDto request, CancellationToken ct = default)
        {
            Event created = new Event()
            {
                Name = request.Name,
                Capacity = request.Capacity,
                Location = request.Location,
                Country = request.Country,
            };

            if (!await Create(created,ct:ct))
            {
                Log.Warning("Couldn't create event: {input}", JsonSerializer.Serialize(request));
                return Result<EventDto>.FailOnCreate("Couldn't create event");
            }

            return Result.Ok(created.ToDto());
        }

        public async Task<Result<EventDto>> Update(EventDto request, CancellationToken ct = default)
        {
            var original = await Get<Event>(request.Id,ct);
            if (original is null)
            {
                return Result<EventDto>.FailNotFound("Couldn't find the original event");
            }
            original.Capacity = request.Capacity;
            original.Location = request.Location;
            original.Country = request.Country;
            original.Name = request.Name;


            if (!await Update(original,ct))
            {
                Log.Warning("Couldn't update event: {input}", JsonSerializer.Serialize(request));
                return Result<EventDto>.Fail("Couldn't update event");
            }

            return Result.Ok(original.ToDto());
        }

        public async Task<Result> Delete(Guid id, CancellationToken ct = default)
        {
            Event? querried = await Get<Event>(id,ct);
            if (querried is null)
            {
                return Result.FailNotFound("Event not found");
            }
            if (await Delete(querried, ct))
            {
                return Result.Ok("Deleted event");
            }
            else
            {
                Log.Warning("Couldn't delete event with id: {id}", id);
                return Result.Fail("Couldn't delete event");
            }
        }

        public async Task<Result<EventDto>> Get(Guid id, CancellationToken ct = default)
        {
            return await GetObject<Event, EventDto>(id);

        }

        public async Task<Result<IEnumerable<EventDto>>> GetAll(CancellationToken ct = default)
        {
            return Result<IEnumerable<EventDto>>.Ok((await Database.Events.ToListAsync(ct)).Select(x => x.ToDto()));
        }




    }
}
