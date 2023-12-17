using Application.Components.Tweet.DTOs;
using Application.Components.Tweet.Mappers;
using Application.Response;
using Core;
using Core.Entities;
using Core.Repository;
using MediatR;
using System.Net;

namespace Application.Components.Tweet.Queries;

public sealed class GetTweetsQuery : IRequest<ResponseResult<IEnumerable<TweetsDTO>>>
{
    public string UserGuid { get; set; }
}

internal sealed class GetTweetsQueryHandler : IRequestHandler<GetTweetsQuery, ResponseResult<IEnumerable<TweetsDTO>>>
{
    private readonly ITweetRepo tweetRepo;
    private readonly IUserRepo userRepo;

    public GetTweetsQueryHandler(ITweetRepo tweetRepo, IUserRepo userRepo)
    {
        this.tweetRepo = tweetRepo;
        this.userRepo = userRepo;
    }

    public async Task<ResponseResult<IEnumerable<TweetsDTO>>> Handle(GetTweetsQuery request, CancellationToken cancellationToken)
    {
        Result<List<UserTweets>, Exception> tweetsResult;

        if (string.IsNullOrEmpty(request.UserGuid))
            tweetsResult = await tweetRepo.Get();
        else
        {
            var userResult = await userRepo.Get(request.UserGuid);
            tweetsResult = await tweetRepo.Get(userResult.Data);
        }
        if (!tweetsResult.IsSuccess)
            return ResponseResult<IEnumerable<TweetsDTO>>
                .CreateError(HttpStatusCode.InternalServerError, "internal-error");

        return ResponseResult<IEnumerable<TweetsDTO>>
                .CreateSuccess(tweetsResult.Data?.ToDto());
    }
}