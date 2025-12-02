using EventManager.Database;
using EventManager.Shared.Dtos;
using EventManager.Shared.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Services
{
    public interface IUserService : IBaseService<UserDto>
    {
        public Task<Result<UserDto>> Create(RegisterRequest request, CancellationToken ct = default);

        public Task<Result<IEnumerable<UserDto>>> GetAll(CancellationToken ct = default);
        public Task<Result> Delete(Guid id, CancellationToken ct = default);
        public Task<Result<UserDto>> GetUserByName(string username, CancellationToken ct = default);
    }


    public class UserService(EventManagerDbContext database, UserManager<User> userManager) : BaseService(database), IUserService
    {
        public async Task<Result<UserDto>> Create(RegisterRequest request, CancellationToken ct = default)
        {
            if(request.Password != request.ConfirmPassword)
            {
                return Result<UserDto>.Fail("Passwords does not match");
            }

            User user = new User()
            {
                UserName = request.Username,
                Email = request.Email,
            };

            var result= await userManager.CreateAsync(user,request.Password!);

            if (!result.Succeeded)
            {
                return Result<UserDto>.FailOnCreate(result.Errors.FirstOrDefault()?.Description ?? "Couldn't create account");
            }

            return Result.Ok(user.ToDto());
        }

        public async Task<Result> Delete(Guid id, CancellationToken ct = default)
        {
            User? user =await Get<User>(id,ct);
            if(user is null)
            {
                return Result.FailNotFound("User not found");
            }
            if(await Delete(user))
            {
                return Result.Ok("Deleted user");
            }
            else
            {
                return Result.Fail("Couldn't delete user");
            }
        }

        public async Task<Result<UserDto>> Get(Guid id, CancellationToken ct = default)
        {
            return await GetObject<User, UserDto>(id);

        }

        public async Task<Result<IEnumerable<UserDto>>> GetAll(CancellationToken ct=default)
        {
            return Result<IEnumerable<UserDto>>.Ok((await Database.Users.ToListAsync(ct)).Select(x=>x.ToDto()));
        }

        public async Task<Result<UserDto>> GetUserByName(string username, CancellationToken ct = default)
        {
            return OkOrNotFound((await Database.Users.FirstOrDefaultAsync(x=>x.UserName== username, ct))?.ToDto());
        }


    }
}
