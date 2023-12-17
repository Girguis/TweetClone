using Core.Entities;

namespace Core.Repository;

public interface ITweetRepo
{
    Task<Result<int, Exception>> Create(User user, string content);

    Task<Result<List<UserTweets>, Exception>> Get();
    Task<Result<List<UserTweets>, Exception>> Get(User user);
}
