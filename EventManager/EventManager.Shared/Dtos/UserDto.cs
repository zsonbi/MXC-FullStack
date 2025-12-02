using EventManager.Shared.Database;


namespace EventManager.Shared.Dtos
{
    public class UserDto : BaseDto
    {
        public string Username { get; set; } =string.Empty;
        public string Email { get; set; } =string.Empty;

        public UserDto(IDbItem user) : base(user)
        {
        }

        public UserDto() { }
    }
}
