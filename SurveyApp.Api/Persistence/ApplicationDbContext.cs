using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Api.Entities;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Claims;

namespace SurveyApp.Api.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Poll> Polls { get; set; }
    public DbSet<Question> Questions { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
	 : base(options)
	{
        _httpContextAccessor = httpContextAccessor;
    }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		var cascadeFKs = modelBuilder.Model
			.GetEntityTypes()
			.SelectMany(t => t.GetForeignKeys())
			.Where(FK => FK.DeleteBehavior == DeleteBehavior.Cascade && !FK.IsOwnership);

		foreach (var fk in cascadeFKs)
			fk.DeleteBehavior = DeleteBehavior.Restrict;

		base.OnModelCreating(modelBuilder);
	}
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
		var entries = ChangeTracker.Entries<AuditableEntity>();
		foreach(var entityEntry in entries)
		{
			var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

			if (entityEntry.State == EntityState.Added)
				entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;

			else if (entityEntry.State == EntityState.Modified)
			{
				entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }


        return base.SaveChangesAsync(cancellationToken);
    }

}


