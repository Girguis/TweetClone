using Microsoft.EntityFrameworkCore;

namespace Core.Entities;
public class UserTweets
{
    public int Id { get; set; }
    public string TweetGuid { get; set; } = null!;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string TweetContent { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
