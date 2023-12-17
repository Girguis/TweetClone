using Application.Components.User.DTOs;
using Application.Components.User.Mappers;
using Application.Helpers;
using Application.Response;
using Core.Repository;
using FluentValidation;
using MediatR;
using System.Net;

namespace Application.Components.User.Queries;

public sealed class UserProfileQuery : IRequest<ResponseResult<UserDTO>>
{
    public string UserGuid { get; set; }
}

internal sealed class UserProfileQueryHandler : IRequestHandler<UserProfileQuery, ResponseResult<UserDTO>>
{
    private readonly IUserRepo userRepo;

    public UserProfileQueryHandler(IUserRepo userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task<ResponseResult<UserDTO>> Handle(UserProfileQuery request, CancellationToken cancellationToken)
    {
        var validator = new UserProfileQueryValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        var errors = validationResult.ConvertErrorsToList();
        if (!validationResult.IsValid)
            return ResponseResult<UserDTO>.CreateError(HttpStatusCode.BadRequest, "validation-error", errors);

        var userResult = await userRepo.Get(request.UserGuid);
        if (!userResult.IsSuccess)
            return ResponseResult<UserDTO>.CreateError(HttpStatusCode.InternalServerError, "internal-error");

        if (userResult.Data == null)
            return ResponseResult<UserDTO>.CreateError(HttpStatusCode.BadRequest, "register-error");

        return ResponseResult<UserDTO>.CreateSuccess(userResult.Data.ToDto());
    }
}
internal sealed class UserProfileQueryValidator : AbstractValidator<UserProfileQuery>
{
    public UserProfileQueryValidator()
    {
        RuleFor(r => r.UserGuid)
            .NotEmpty();            
    }
}