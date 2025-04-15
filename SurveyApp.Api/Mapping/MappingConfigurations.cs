using Mapster;
using SurveyApp.Api.Contracts.Polls;
using SurveyApp.Api.Contracts.Questions;
using SurveyApp.Api.Entities;

namespace SurveyApp.Api.Mapping
{
    public class MappingConfigurations : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<QuestionRequest, Question>()
				.Ignore(nameof(Question.Answers));
		}
	}
}
