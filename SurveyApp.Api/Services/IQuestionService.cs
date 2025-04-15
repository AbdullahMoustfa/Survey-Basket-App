using SurveyApp.Api.Contracts.Questions;

namespace SurveyApp.Api.Services
{
    public interface IQuestionService
    {
        Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken);
        Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int id,  CancellationToken cancellationToken);   
        Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken); 
        Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken); 
        Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken );
    }
}
