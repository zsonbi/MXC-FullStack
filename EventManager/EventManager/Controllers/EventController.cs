using EventManager.Services;
using EventManager.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorContentManager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EventDto request, CancellationToken ct = default)
        {
            var result = await eventService.Create(request, ct);

            if (result.Success)
            {
                return result.ReturnAsActionResult();
            }
            else
            {
                ModelState.AddModelError("CreateError", result.Message);
                return ValidationProblem(ModelState);
            }
        }


        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] EventDto request, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await eventService.Update(request, ct);

            if (result.Success)
            {
                return result.ReturnAsActionResult();
            }
            else
            {
                ModelState.AddModelError("UpdateError", result.Message);
                return ValidationProblem(ModelState);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id,CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await eventService.Get(id,ct);

            if (result.Success)
            {
                return result.ReturnAsActionResult();
            }
            else
            {
                ModelState.AddModelError("Get", result.Message);
                return ValidationProblem(ModelState);
            }
        }

        [Authorize()]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct = default)
        {
            var list = await eventService.GetAll(ct);
            return Ok(list.Data);
        }


        [Authorize()]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
        {
            var result = await eventService.Delete(id, ct);
      
            return result.ReturnAsActionResult();
        }

    }
}
