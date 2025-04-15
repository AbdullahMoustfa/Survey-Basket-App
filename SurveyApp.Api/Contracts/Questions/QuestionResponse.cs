using SurveyApp.Api.Contracts.Answers;

namespace SurveyApp.Api.Contracts.Questions
{
    public record QuestionResponse(
        int Id,
        string Content,
        IEnumerable<AnswerResponse> Answers
     );
}
