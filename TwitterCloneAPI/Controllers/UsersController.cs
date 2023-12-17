using Application.Components.User.Commands;
using Application.Components.User.DTOs;
using Application.Components.User.Queries;
using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneAPI.Extensions;
using TwitterCloneAPI.Filters;
using TwitterCloneAPI.Models;

namespace TwitterCloneAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ISender sender;
    public UsersController(ISender sender)
    {
        this.sender = sender;
    }

    [ProducesResponseType(200)]
    [ProducesErrorResponseType(typeof(ResponseErrorModel))]
    [ProducesDefaultResponseType]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var command = new RegisterUserCommand()
        {
            Email = model.Email,
            Password = model.Password,
            UserName = model.UserName,
        };

        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [ProducesResponseType(typeof(string), 200)]
    [ProducesErrorResponseType(typeof(ResponseErrorModel))]
    [ProducesDefaultResponseType]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var command = new UserLoginCommand()
        {
            UserName = model.UserName,
            Password = model.Password
        };

        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [AuthorizationFilter]
    [ProducesResponseType(typeof(UserDTO), 200)]
    [ProducesErrorResponseType(typeof(ResponseErrorModel))]
    [ProducesDefaultResponseType]
    [HttpGet("Profile")]
    public async Task<IActionResult> Profile()
    {
        var command = new UserProfileQuery()
        {
            UserGuid = this.Guid()
        };

        var result = await sender.Send(command);
        return result.ToActionResult();
    }
    [AuthorizationFilter]
    [ProducesResponseType(200)]
    [ProducesErrorResponseType(typeof(ResponseErrorModel))]
    [ProducesDefaultResponseType]
    [HttpDelete]
    [Route("{id}/Unfollow")]
    public async Task<IActionResult> Unfollow([FromRoute] string id)
    {
        var command = new UnfollowUserCommand()
        {
            UserGuid = this.Guid(),
            ToUnfollowUserGuid = id
        };

        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [AuthorizationFilter]
    [ProducesResponseType(200)]
    [ProducesErrorResponseType(typeof(ResponseErrorModel))]
    [ProducesDefaultResponseType]
    [HttpPost]
    [Route("{id}/Follow")]
    public async Task<IActionResult> Follow([FromRoute] string id)
    {
        var command = new FollowUserCommand()
        {
            UserGuid = this.Guid(),
            ToUnfollowUserGuid = id
        };

        var result = await sender.Send(command);
        return result.ToActionResult();
    }
}
