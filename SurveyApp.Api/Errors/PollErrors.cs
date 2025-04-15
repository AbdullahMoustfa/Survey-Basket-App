namespace SurveyApp.Api.Errors
{
    public class PollErrors
    {
        public static readonly Error PollNotFound =
            new("Poll.NotFound", "No poll was found with the given ID");

        public static readonly Error DuplicatedPollTitle =
          new("Poll.Duplicated", "Another poll with the same title is already exists");
    }
}
