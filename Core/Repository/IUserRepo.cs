using Core.Entities;

namespace Core.Repository;

public interface IUserRepo
{
    Task<Result<User, Exception>> Login(string userName, string password);
    Task<Result<int, Exception>> Register(string email, string password, string userName);
    Task<Result<User, Exception>> Get(string guid);
    Task<Result<User, Exception>> GetByUserName(string userName);
    Task<Result<User, Exception>> GetByEmail(string email);
    Task<Result<int, Exception>> Follow(User user, User toFollowUser);
    Task<Result<int, Exception>> Unfollow(Follower follower);
    Task<Result<Follower, Exception>> IsFollowing(User user, User tofollowUser);
}
