using Application.Components.User.DTOs;

namespace Application.Components.User.Mappers;

internal static class UserMapper
{
    public static UserDTO ToDto(this Core.Entities.User user)
    {
        if (user == null)
            return new UserDTO();

        return new UserDTO
        {
            Email = user.Email,
            UserName = user.UserName,
            FollowersNumber = user.NumberOfFollowers,
            FollowingNumber = user.NumberOfFollowing,
            PostsNumber = user.NumberOfPosts
        };
    }
}
