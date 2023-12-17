namespace Application.Components.User.DTOs;

public sealed class UserDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public int FollowersNumber { get; set; }
    public int FollowingNumber { get; set; }
    public int PostsNumber { get; set; }
}
