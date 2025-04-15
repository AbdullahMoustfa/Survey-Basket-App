using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SurveyApp.Api
{
    public static class DependencyInjection
	{
		public static IServiceCollection AddDependencies(this IServiceCollection services, 
			IConfiguration configuration)
		{

			services.AddControllers();

			var allowOrigins = configuration.GetSection("AllowOrigins").Get<string[]>();

			services.AddCors(options =>
				options.AddDefaultPolicy(builder =>
					builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.WithOrigins(allowOrigins!)

				)
			);

			services.AddAuthConfig(configuration);

			var connectionString = configuration.GetConnectionString("DefaultConnection") ??
				throw new InvalidOperationException("Connection String 'DefaultConnection' not found.");

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));

			services
				 .AddSwaggerConfig()
				 .AddMapsterConfig()
				 .AddFluentValidationConfig();


			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IPollService, PollService>();
			services.AddScoped<IQuestionService, QuestionService>();

			services.AddExceptionHandler<GlobalExceptionHandler>();
			services.AddProblemDetails();

			return services;
		}

		private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
		{

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			return services;
		}

		private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
		{

			// Add Mapster
			var mappingConfig = TypeAdapterConfig.GlobalSettings;
			mappingConfig.Scan(Assembly.GetExecutingAssembly());

			services.AddSingleton<IMapper>(new Mapper(mappingConfig));


			return services;
		}

		private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
		{

			// Add FluentValidation
			services
				.AddFluentValidationAutoValidation()
			    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			

			return services;
		}

		private static IServiceCollection AddAuthConfig(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddSingleton<IJwtProvider, JwtProvider>();

			//services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

			services.AddOptions<JwtOptions>()
				.BindConfiguration(JwtOptions.SectionName)
				.ValidateDataAnnotations()
				.ValidateOnStart();

			services
				.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}) 
				.AddJwtBearer(O =>
				{
					O.SaveToken = true;
					O.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,	
						ValidateIssuer = true,	
						ValidateAudience = true,	
						ValidateLifetime = true,	
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
						ValidIssuer = configuration["Jwt:Issure"],
						ValidAudience = configuration["Jwt:Audience"]

					};
				});

		
			return services;	

		}
	}
}
