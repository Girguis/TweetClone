using Application.Helpers;
using Application.Response;
using Core.Repository;
using FluentValidation;
using MediatR;
using System.Net;

namespace Application.Components.Tweet.Commands;
public sealed class TweetsCommand : IRequest<ResponseResult>
{
    public string UserGuid { get; set; }
    public string Content { get; set; }
}

internal sealed class TweetCommandHandler : IRequestHandler<TweetsCommand, ResponseResult>
{
    private readonly ITweetRepo tweetRepo;
    private readonly IUserRepo userRepo;

    public TweetCommandHandler(ITweetRepo tweetRepo, IUserRepo userRepo)
    {
        this.tweetRepo = tweetRepo;
        this.userRepo = userRepo;
    }

    public async Task<ResponseResult> Handle(TweetsCommand request, CancellationToken cancellationToken)
    {
        var validator = new TweetCommandValidator(userRepo);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        var errors = validationResult.ConvertErrorsToList();
        if (!validationResult.IsValid)
            return ResponseResult.CreateError(HttpStatusCode.BadRequest, "validation-error", errors);

        var userResult = await userRepo.Get(request.UserGuid);

        var result = await tweetRepo.Create(userResult.Data, request.Content);
        if (!result.IsSuccess)
            return ResponseResult.CreateError(HttpStatusCode.InternalServerError, "internal-error");

        if (result.Data < 1)
            return ResponseResult.CreateError(HttpStatusCode.BadRequest, "tweet-add-error");

        return ResponseResult.CreateSuccess();
    }
}
internal sealed class TweetCommandValidator : AbstractValidator<TweetsCommand>
{
    public TweetCommandValidator(IUserRepo userRepo)
    {
        RuleFor(r => r.UserGuid)
            .NotEmpty()
            .UserExists(userRepo)
            .WithMessage("User doesn't exist");

        RuleFor(r => r.Content)
            .NotEmpty();
    }
}