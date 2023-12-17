namespace Core.Entities;

public class Follower
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FollowedByUserId { get; set; }

    public virtual User FollowedByUser { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
