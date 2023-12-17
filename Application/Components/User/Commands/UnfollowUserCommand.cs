using Application.Helpers;
using Application.Response;
using Core.Repository;
using FluentValidation;
using MediatR;
using System.Net;

namespace Application.Components.User.Commands;


public sealed class UnfollowUserCommand : IRequest<ResponseResult>
{
    public string UserGuid { get; set; }
    public string ToUnfollowUserGuid { get; set; }
}

internal sealed class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, ResponseResult>
{
    private readonly IUserRepo userRepo;
    public UnfollowUserCommandHandler(IUserRepo userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task<ResponseResult> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new UnfollowUserCommandValidator(userRepo);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        var errors = validationResult.ConvertErrorsToList();
        if (!validationResult.IsValid)
            return ResponseResult.CreateError(HttpStatusCode.BadRequest, "validation-error", errors);

        var userResult = await userRepo.Get(request.UserGuid);
        var userToUnfollowResult = await userRepo.Get(request.ToUnfollowUserGuid);

        var followingResult = await userRepo.IsFollowing(userResult.Data, userToUnfollowResult.Data);
        if (!followingResult.IsSuccess)
            return ResponseResult.CreateError(HttpStatusCode.InternalServerError, "internal-error");

        if (followingResult.Data == null)
            return ResponseResult.CreateSuccess();

        var result = await userRepo.Unfollow(followingResult.Data);
        if (!result.IsSuccess)
            return ResponseResult.CreateError(HttpStatusCode.InternalServerError, "internal-error");

        if (result.Data < 1)
            return ResponseResult.CreateError(HttpStatusCode.BadRequest, "unfollow-error");

        return ResponseResult.CreateSuccess();
    }
}
internal sealed class UnfollowUserCommandValidator : AbstractValidator<UnfollowUserCommand>
{
    public UnfollowUserCommandValidator(IUserRepo userRepo)
    {
        RuleFor(r => r.UserGuid)
            .NotEmpty()
            .UserExists(userRepo)
            .WithMessage("User doesn't exist")
            .Must((model, field) =>
            {
                return !model.UserGuid?.Equals(model.ToUnfollowUserGuid) ?? false;
            })
            .WithMessage("User can't unfollow himself");

        RuleFor(r => r.ToUnfollowUserGuid)
            .NotEmpty()
            .UserExists(userRepo)
            .WithMessage("User doesn't exist");
    }
}