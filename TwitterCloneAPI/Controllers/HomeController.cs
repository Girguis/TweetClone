using Application.Components.Tweet.Commands;
using Application.Components.Tweet.DTOs;
using Application.Components.Tweet.Queries;
using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneAPI.Extensions;
using TwitterCloneAPI.Filters;
using TwitterCloneAPI.Models;

namespace TwitterCloneAPI.Controllers;

[Route("api/Home")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly ISender sender;
    public HomeController(ISender sender)
    {
        this.sender = sender;
    }
    [ProducesResponseType(typeof(List<TweetsDTO>),200)]
    [ProducesErrorResponseType(typeof(ResponseErrorModel))]
    [ProducesDefaultResponseType]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = new GetTweetsQuery()
        {
            UserGuid = this.Guid()
        };

        var result = await sender.Send(query);
        return result.ToActionResult();
    }
}
