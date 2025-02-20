using SurveyApp.Api.Contracts.Polls;

namespace SurveyApp.Api.Services
{
    public interface IPollService
	{
		Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<PollResponse> AddAsync(PollRequest request, CancellationToken cancellationToken = default);
		Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default);
		Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
		Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);	
	}
}
