using Application.Components.Tweet.Commands;
using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneAPI.Extensions;
using TwitterCloneAPI.Filters;
using TwitterCloneAPI.Models;

namespace TwitterCloneAPI.Controllers;

[AuthorizationFilter]
[Route("api/Tweets")]
[ApiController]
public class TweetController : ControllerBase
{
    private readonly ISender sender;
    public TweetController(ISender sender)
    {
        this.sender = sender;
    }
    [ProducesResponseType(200)]
    [ProducesErrorResponseType(typeof(ResponseErrorModel))]
    [ProducesDefaultResponseType]
    [HttpPost]
    public async Task<IActionResult> Tweet([FromBody] ContentModel model)
    {
        var command = new TweetsCommand()
        {
            Content = model.Text,
            UserGuid = this.Guid()
        };

        var result = await sender.Send(command);
        return result.ToActionResult();
    }
}
