using Application.Components.Tweet.DTOs;
using Core.Entities;

namespace Application.Components.Tweet.Mappers;

internal static class TweetMapper
{
    public static IEnumerable<TweetsDTO> ToDto(this List<UserTweets> tweets)
    {
        if (tweets == null || !tweets.Any())
            return new List<TweetsDTO>();

        return tweets.Select(x => new TweetsDTO
        {
            Content = x.TweetContent,
            CreatedAt = x.CreatedAt,
            UserName = x.User.UserName
        });
    }
}
