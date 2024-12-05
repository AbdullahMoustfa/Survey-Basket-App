using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Api.Entities;
using System.Reflection;
using System.Reflection.Emit;

namespace SurveyApp.Api.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
{ 
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	 : base(options)
	{

	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(modelBuilder);
	}
	public DbSet<Poll> Polls { get; set; }
}


