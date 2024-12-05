using Mapster;
using SurveyApp.Api.Contracts.Polls;
using SurveyApp.Api.Entities;

namespace SurveyApp.Api.Mapping
{
    public class MappingConfigurations : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Poll, PollResponse>()
				.Map(dest => dest.Summary, src => src.Summary);
		}
	}
}
