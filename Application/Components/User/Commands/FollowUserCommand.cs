using Application.Helpers;
using Application.Response;
using Core.Repository;
using FluentValidation;
using MediatR;
using System.Net;


namespace Application.Components.User.Commands;

public sealed class FollowUserCommand : IRequest<ResponseResult>
{
    public string UserGuid { get; set; }
    public string ToUnfollowUserGuid { get; set; }
}

internal sealed class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, ResponseResult>
{
    private readonly IUserRepo userRepo;
    public FollowUserCommandHandler(IUserRepo userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task<ResponseResult> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new FollowUserCommandValidator(userRepo);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        var errors = validationResult.ConvertErrorsToList();
        if (!validationResult.IsValid)
            return ResponseResult.CreateError(HttpStatusCode.BadRequest, "validation-error", errors);

        var userResult = await userRepo.Get(request.UserGuid);
        var userToUnfollowResult = await userRepo.Get(request.ToUnfollowUserGuid);

        var isFollowingResult = await userRepo.IsFollowing(userResult.Data, userToUnfollowResult.Data);
        if (!isFollowingResult.IsSuccess)
            return ResponseResult.CreateError(HttpStatusCode.InternalServerError, "internal-error");

        if (isFollowingResult.Data != null)
            return ResponseResult.CreateSuccess();

        var result = await userRepo.Follow(userResult.Data, userToUnfollowResult.Data);
        if (!result.IsSuccess)
            return ResponseResult.CreateError(HttpStatusCode.InternalServerError, "internal-error");

        if (result.Data < 1)
            return ResponseResult.CreateError(HttpStatusCode.BadRequest, "follow-error");



        return ResponseResult.CreateSuccess();
    }
}
internal sealed class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
{
    public FollowUserCommandValidator(IUserRepo userRepo)
    {
        RuleFor(r => r.UserGuid)
            .NotEmpty()
            .UserExists(userRepo)
            .WithMessage("User doesn't exist")
            .Must((model, field) =>
            {
                return !model.UserGuid?.Equals(model.ToUnfollowUserGuid) ?? false;
            })
            .WithMessage("User can't follow himself");

        RuleFor(r => r.ToUnfollowUserGuid)
            .NotEmpty()
            .UserExists(userRepo)
            .WithMessage("User doesn't exist");
    }
}