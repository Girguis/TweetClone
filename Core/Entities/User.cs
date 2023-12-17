namespace Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int NumberOfFollowers { get; set; }
    public int NumberOfFollowing { get; set; }
    public int NumberOfPosts { get; set; }
}
