using Core.Repository;
using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserRepo : IUserRepo
{
    private readonly TwitterDbContext context;

    public UserRepo(TwitterDbContext context)
    {
        this.context = context;
    }
    public async Task<Result<User, Exception>> Get(string userGuid)
    {
        try
        {
            var user = await context
                .Users
                .SingleOrDefaultAsync(x => x.Guid.Equals(userGuid));
            return ResultExtensions<User, Exception>.Success(user);
        }
        catch (Exception ex)
        {
            return ResultExtensions<User, Exception>.Error(ex);
        }
    }

    public async Task<Result<User, Exception>> GetByEmail(string email)
    {
        try
        {
            var user = await context
                .Users
                .SingleOrDefaultAsync(x => x.Email.Equals(email));
            return ResultExtensions<User, Exception>.Success(user);
        }
        catch (Exception ex)
        {
            return ResultExtensions<User, Exception>.Error(ex);
        }
    }

    public async Task<Result<User, Exception>> GetByUserName(string userName)
    {
        try
        {
            var user = await context
                .Users
                .SingleOrDefaultAsync(x => x.UserName.Equals(userName));
            return ResultExtensions<User, Exception>.Success(user);
        }
        catch (Exception ex)
        {
            return ResultExtensions<User, Exception>.Error(ex);
        }
    }

    public async Task<Result<User, Exception>> Login(string userName, string password)
    {
        try
        {
            var user = await context
                .Users
                .SingleOrDefaultAsync(x => x.UserName.Equals(userName)
                                      && x.Password.Equals(password));
            return ResultExtensions<User, Exception>.Success(user);
        }
        catch (Exception ex)
        {
            return ResultExtensions<User, Exception>.Error(ex);
        }
    }

    public async Task<Result<int, Exception>> Register(string email, string password, string userName)
    {
        try
        {
            await context.Users.AddAsync(new User
            {
                Email = email,
                Password = password,
                UserName = userName,
                Guid = Guid.NewGuid().ToString()
            });
            var result = await context.SaveChangesAsync();
            return ResultExtensions<int, Exception>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultExtensions<int, Exception>.Error(ex);
        }
    }


    public async Task<Result<int, Exception>> Follow(User user, User toFollowUser)
    {
        try
        {
            context.Followers.Add(new Follower
            {
                FollowedByUser = user,
                FollowedByUserId = user.Id,
                User = toFollowUser,
                UserId = toFollowUser.Id,
            });
            user.NumberOfFollowing += 1;
            toFollowUser.NumberOfFollowers += 1;
            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            context.Users.Attach(toFollowUser);
            context.Entry(toFollowUser).State = EntityState.Modified;
            var result = await context.SaveChangesAsync();
            return ResultExtensions<int, Exception>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultExtensions<int, Exception>.Error(ex);
        }
    }
    public async Task<Result<Follower, Exception>> IsFollowing(User user, User toFollowUser)
    {
        try
        {
            var result = await context.Followers
                .FirstOrDefaultAsync(x=> x.UserId == toFollowUser.Id 
                                    && x.FollowedByUserId == user.Id);
            return ResultExtensions<Follower, Exception>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultExtensions<Follower, Exception>.Error(ex);
        }
    }
    public async Task<Result<int, Exception>> Unfollow(Follower follower)
    {
        try
        {
            context.Followers.Attach(follower);
            context.Entry(follower).State = EntityState.Deleted;

            follower.User.NumberOfFollowers -= 1;
            context.Users.Attach(follower.User);
            context.Entry(follower.User).State = EntityState.Modified;

            follower.FollowedByUser.NumberOfFollowing -= 1;
            context.Users.Attach(follower.FollowedByUser);
            context.Entry(follower.FollowedByUser).State = EntityState.Modified;
            
            var result = await context.SaveChangesAsync();
            return ResultExtensions<int, Exception>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultExtensions<int, Exception>.Error(ex);
        }
    }
}
