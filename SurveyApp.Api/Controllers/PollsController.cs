using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SurveyApp.Api.Contracts.Polls;


namespace SurveyApp.Api.Controllers;


    public class PollsController : BaseApiController
{
	private readonly IPollService _pollService;

	public PollsController(IPollService pollService)
	{
		_pollService = pollService;
	}

	[HttpGet("")]
	[Authorize]
	public async Task<IActionResult> GetAll()
	{
		var polls = await _pollService.GetAllAsync();

		var response = polls.Adapt<IEnumerable<PollResponse>>();

		return Ok(response);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get([FromRoute] int id,
		CancellationToken cancellationToken = default)
	{
		var poll = await _pollService.GetAsync(id, cancellationToken);

		var response = poll.Adapt<PollResponse>();

		if (response is null) return NotFound();

		return Ok(response);
	}

	[HttpPost("")]
	public async Task<IActionResult> Add(PollRequest request,
		CancellationToken cancellationToken)
	{
		var newPoll = await _pollService.AddAsync(request.Adapt<Poll>(), cancellationToken);

		return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
	{
		var isUpdated = await _pollService.UpdateAsync(id, request.Adapt<Poll>(), cancellationToken);

		if (!isUpdated)
			return NotFound();

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
	{
		var isDeleted = await _pollService.DeleteAsync(id, cancellationToken);

		if (!isDeleted)
			return NotFound();

		return NoContent();
	}

	[HttpPut("{id}/togglePublish")]
	public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
	{
		var isUpdated = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

		if (!isUpdated)
			return NotFound();

		return NoContent();
	}
}
