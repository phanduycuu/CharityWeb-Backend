
using Charity.Models;

namespace Charity.Mapper
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Role = user.Role,
                FullName = user.FullName,
                Email = user.Email,
                Id = user.Id,
            };
        }
    }
}
