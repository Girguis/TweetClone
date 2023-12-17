using Core;
using Core.Entities;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TweetRepo : ITweetRepo
{
    private readonly TwitterDbContext context;

    public TweetRepo(TwitterDbContext context)
    {
        this.context = context;
    }

    public async Task<Result<int, Exception>> Create(User user, string content)
    {
        try
        {
            await context.UserTweets.AddAsync(new UserTweets
            {
                CreatedAt = DateTime.UtcNow,
                TweetContent = content,
                TweetGuid = Guid.NewGuid().ToString(),
                User = user,
                UserId = user.Id
            });
            var result = await context.SaveChangesAsync();
            return ResultExtensions<int, Exception>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultExtensions<int, Exception>.Error(ex);
        }
    }
    public async Task<Result<List<UserTweets>, Exception>> Get()
    {
        try
        {
            var topFollowedUsersIds = context
                              .Users
                              .OrderByDescending(x => x.NumberOfFollowers)
                              .Take(5)
                              .AsQueryable()
                              .AsNoTracking()
                              .Select(x => x.Id);
            var currentDateTime = DateTime.UtcNow;
            var tweets = context
                        .UserTweets
                        .Include(x => x.User)
                        .Where(x => topFollowedUsersIds.Any(z => z == x.UserId)
                                    && (currentDateTime - x.CreatedAt).TotalDays <= 2)
                        .OrderByDescending(x => x.CreatedAt)
                        .AsQueryable()
                        .AsNoTracking();
            return ResultExtensions<List<UserTweets>, Exception>.Success(tweets.ToList());
        }
        catch (Exception ex)
        {
            return ResultExtensions<List<UserTweets>, Exception>.Error(ex);
        }
    }

    public async Task<Result<List<UserTweets>, Exception>> Get(User user)
    {
        try
        {
            var followedUsers = context
                .Followers
                .Where(x => x.FollowedByUserId == user.Id)
                .AsQueryable()
                .AsNoTracking();
            var currentDateTime = DateTime.UtcNow;
            var tweets = context
                        .UserTweets
                        .Include(x => x.User)
                        .Where(x => followedUsers.Any(z => z.UserId == x.UserId)
                                    && (currentDateTime - x.CreatedAt).TotalDays <= 2)
                        .OrderByDescending(x => x.CreatedAt)
                        .AsQueryable()
                        .AsNoTracking();
            return ResultExtensions<List<UserTweets>, Exception>.Success(tweets.ToList());
        }
        catch (Exception ex)
        {
            return ResultExtensions<List<UserTweets>, Exception>.Error(ex);
        }
    }
}
