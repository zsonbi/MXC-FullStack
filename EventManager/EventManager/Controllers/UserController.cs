using EventManager.Database;
using EventManager.Services;
using EventManager.Shared.Dtos;
using EventManager.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorContentManager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public UserController(IUserService userService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterRequest request, CancellationToken ct = default)
        {
            var result = await userService.Create(request, ct);

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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await signInManager.PasswordSignInAsync(
                request.UserName,
                request.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok();
            }


            return Unauthorized("Invalid username or password");
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id,CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await userService.Get(id,ct);

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

        [HttpGet("LoggedIn")]
        public async Task<IActionResult> LoggedIn(CancellationToken ct = default)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();

            var userInDb = await userService.Get(user.Id);

            return userInDb.ReturnAsActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct = default)
        {
            var list = await userService.GetAll(ct);
            return Ok(list.Data);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
        {
            var result = await userService.Delete(id, ct);
      
            return result.ReturnAsActionResult();
        }

    }
}
