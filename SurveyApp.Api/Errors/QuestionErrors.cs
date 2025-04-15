namespace SurveyApp.Api.Errors
{
    public class QuestionErrors
    {
        public static readonly Error QuestionNotFound =
            new("Question.NotFound", "No question was found with the given ID");

        public static readonly Error DuplicatedQuestionContent =
          new("Question.DuplicatedContent", "Another question with the same content is already exists");
    }
}
