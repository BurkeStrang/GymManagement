using ErrorOr;
using GymManagement.Application.Gyms.Commands.AddTrainer;
using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Application.Gyms.Commands.DeleteGym;
using GymManagement.Application.Gyms.Queries.GetGym;
using GymManagement.Application.Gyms.Queries.ListGyms;
using GymManagement.Contracts.Gyms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Api.Controllers;

[Route("subscriptions/{subscriptionId:guid}/gyms")]
public class GymsController(ISender _mediator) : ApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Successful response
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] // Validation error
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] // Not found
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)] // Unauthorized
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)] // Conflict
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> CreateGym(
        CreateGymRequest request,
        Guid subscriptionId)
    {
        var command = new CreateGymCommand(request.Name, subscriptionId);

        var createGymResult = await _mediator.Send(command);

        return createGymResult.Match(
            gym => CreatedAtAction(
                nameof(GetGym),
                new { subscriptionId, GymId = gym.Id },
                new GymResponse(gym.Id, gym.Name)),
            Problem);
    }

    [HttpDelete("{gymId:guid}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Successful response
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] // Validation error
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] // Not found
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)] // Unauthorized
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)] // Conflict
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> DeleteGym(Guid subscriptionId, Guid gymId)
    {
        var command = new DeleteGymCommand(subscriptionId, gymId);

        var deleteGymResult = await _mediator.Send(command);

        return deleteGymResult.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Successful response
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] // Validation error
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] // Not found
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)] // Unauthorized
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)] // Conflict
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> ListGyms(Guid subscriptionId)
    {
        var command = new ListGymsQuery(subscriptionId);

        var listGymsResult = await _mediator.Send(command);

        return listGymsResult.Match(
            gyms => Ok(gyms.ConvertAll(gym => new GymResponse(gym.Id, gym.Name))),
            Problem);
    }

    [HttpGet("{gymId:guid}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Successful response
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] // Validation error
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] // Not found
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)] // Unauthorized
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)] // Conflict
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> GetGym(Guid subscriptionId, Guid gymId)
    {
        var command = new GetGymQuery(subscriptionId, gymId);

        var getGymResult = await _mediator.Send(command);

        return getGymResult.Match(
            gym => Ok(new GymResponse(gym.Id, gym.Name)),
            Problem);
    }

    [HttpPost("{gymId:guid}/trainers")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Successful response
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] // Validation error
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] // Not found
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)] // Unauthorized
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)] // Conflict
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> AddTrainer(AddTrainerRequest request, Guid subscriptionId, Guid gymId)
    {
        var command = new AddTrainerCommand(subscriptionId, gymId, request.TrainerId);

        var addTrainerResult = await _mediator.Send(command);

        return addTrainerResult.Match(
            success => Ok(),
            Problem);
    }
}