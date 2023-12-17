using Application.Helpers;
using Application.Response;
using Core.Repository;
using FluentValidation;
using MediatR;
using System.Net;

namespace Application.Components.User.Commands;

public sealed class RegisterUserCommand : IRequest<ResponseResult>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseResult>
{
    private readonly IUserRepo userRepo;

    public RegisterUserCommandHandler(IUserRepo userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task<ResponseResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new RegisterUserCommandValidator(userRepo);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        var errors = validationResult.ConvertErrorsToList();
        if (!validationResult.IsValid)
            return ResponseResult.CreateError(HttpStatusCode.BadRequest, "validation-error", errors);

        var hashedPassword = PasswordHelper.Hash(request.Password);
        var addResult = await userRepo.Register(request.Email, hashedPassword, request.UserName);
        if (!addResult.IsSuccess)
            return ResponseResult.CreateError(HttpStatusCode.InternalServerError, "internal-error");

        if (addResult.Data < 1)
            return ResponseResult.CreateError(HttpStatusCode.BadRequest, "register-error");

        return ResponseResult.CreateSuccess();
    }
}
internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IUserRepo userRepo)
    {
        RuleFor(r => r.UserName)
            .NotEmpty()
            .MaximumLength(100)
            .UserNameNotExists(userRepo)
            .WithMessage("UserName already exists");

        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(250)
            .EmailNotExists(userRepo)
            .WithMessage("Email already exists");

        RuleFor(r => r.Password)
            .NotEmpty()
            .MinimumLength(5);
    }
}