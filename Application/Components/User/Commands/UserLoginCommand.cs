using Application.Helpers;
using Application.Response;
using Core.Repository;
using Core.Services;
using FluentValidation;
using MediatR;
using System.Net;

namespace Application.Components.User.Commands;

public sealed class UserLoginCommand : IRequest<ResponseResult<string>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

internal sealed class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, ResponseResult<string>>
{
    private readonly IUserRepo userRepo;
    private readonly ITokenService tokenService;

    public UserLoginCommandHandler(IUserRepo userRepo, ITokenService tokenService)
    {
        this.userRepo = userRepo;
        this.tokenService = tokenService;
    }

    public async Task<ResponseResult<string>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var validator = new UserLoginCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        var errors = validationResult.ConvertErrorsToList();
        if (!validationResult.IsValid)
            return ResponseResult<string>.CreateError(HttpStatusCode.BadRequest, "validation-error", errors);

        var hashedPassword = PasswordHelper.Hash(request.Password);
        var loginResult = await userRepo.Login(request.UserName, hashedPassword);
        if (!loginResult.IsSuccess)
            return ResponseResult<string>.CreateError(HttpStatusCode.InternalServerError, "internal-error");

        if (loginResult.Data == null)
            return ResponseResult<string>.CreateError(HttpStatusCode.BadRequest, "login-error");

        var tokenResult = tokenService.Generate(loginResult.Data.Guid);
        if (!tokenResult.IsSuccess)
            return ResponseResult<string>.CreateError(HttpStatusCode.BadRequest, "token-error");

        return ResponseResult<string>.CreateSuccess(tokenResult.Data);
    }
}
internal sealed class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
{
    public UserLoginCommandValidator()
    {
        RuleFor(r => r.UserName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(r => r.Password)
            .NotEmpty()
            .MinimumLength(5);
    }
}