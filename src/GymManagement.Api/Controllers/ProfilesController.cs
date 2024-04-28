using MediatR;

using Microsoft.AspNetCore.Mvc;

using GymManagement.Application.Profiles.ListProfiles;
using GymManagement.Contracts.Profiles;
using GymManagement.Application.Profiles.Commands.CreateAdminProfile;
using Microsoft.AspNetCore.Authorization;

namespace GymManagement.Api.Controllers;

[Route("users/{userId:guid}/profiles")]
public class ProfilesController(ISender _mediator) : ApiController
{
    [HttpPost("admin")]
    [Authorize]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Successful response
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] // Validation error
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] // Not found
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)] // Unauthorized
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)] // Conflict
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> CreateAdminProfile(Guid userId)
    {
        var command = new CreateAdminProfileCommand(userId);

        var createProfileResult = await _mediator.Send(command);

        return createProfileResult.Match(
            id => Ok(new ProfileResponse(id)),
            Problem);
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Successful response
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] // Validation error
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] // Not found
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)] // Unauthorized
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)] // Conflict
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> ListProfiles(Guid userId)
    {
        var listProfilesQuery = new ListProfilesQuery(userId);

        var listProfilesResult = await _mediator.Send(listProfilesQuery);

        return listProfilesResult.Match(
            profiles => Ok(new ListProfilesResponse(
                profiles.AdminId,
                profiles.ParticipantId,
                profiles.TrainerId)),
            Problem);
    }
}