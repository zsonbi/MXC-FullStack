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
        /// <summary>
        /// Creates a new event entry in the database
        /// </summary>
        /// <param name="request">The event details</param>
        /// <param name="ct">Cancellation token for async operations</param>
        /// <returns>The created event data or an error result</returns>
        public Task<Result<EventDto>> Create(EventDto request, CancellationToken ct = default);

        /// <summary>
        /// Gets all of the events
        /// </summary>
        /// <param name="ct">Cancellation token for async operations</param>
        /// <returns>A Result with and IEnumerable of EventDtos</returns>
        public Task<Result<IEnumerable<EventDto>>> GetAll(CancellationToken ct = default);
        /// <summary>
        /// Deletes an event
        /// </summary>
        /// <param name="id">The event id we want to delete</param>
        /// <param name="ct">Cancellation token for async operations</param>
        /// <returns>A result object with error or success</returns>
        public Task<Result> Delete(Guid id, CancellationToken ct = default);
        /// <summary>
        /// Updates an existing event entry in the database
        /// </summary>
        /// <param name="request">The event details</param>
        /// <param name="ct">Cancellation token for async operations.</param>
        /// <returns>The updated event data or an error result wrapped in result object</returns>
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
