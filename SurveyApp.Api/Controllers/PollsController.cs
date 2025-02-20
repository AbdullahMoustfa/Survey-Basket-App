using Microsoft.AspNetCore.Authorization;
using SurveyApp.Api.Contracts.Polls;

namespace SurveyApp.Api.Controllers;


[Authorize]
public class PollsController : BaseApiController
{
    private readonly IPollService _pollService;

    public PollsController(IPollService pollService)
    {
        _pollService = pollService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var polls = await _pollService.GetAllAsync();

        var response = polls.Adapt<IEnumerable<PollResponse>>();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var result = await _pollService.GetAsync(id, cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem(StatusCodes.Status404NotFound);

    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request,
       CancellationToken cancellationToken)
    {
        var newPoll = await _pollService.AddAsync(request, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request
        , CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess
            ? NoContent() 
            : result.ToProblem(StatusCodes.Status404NotFound);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id, cancellationToken);

        return result.IsSuccess 
            ? NoContent() 
            : result.ToProblem(StatusCodes.Status404NotFound);
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSuccess 
            ? NoContent()
            : result.ToProblem(StatusCodes.Status404NotFound);

    }
}
