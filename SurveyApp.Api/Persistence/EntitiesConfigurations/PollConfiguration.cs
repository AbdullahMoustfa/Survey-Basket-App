﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyApp.Api.Entities;

namespace SurveyApp.Api.Persistence.EntitiesConfigurations
{
	public class PollConfiguration : IEntityTypeConfiguration<Poll>
	{
		public void Configure(EntityTypeBuilder<Poll> builder)
		{
			builder.HasIndex(x => x.Title).IsUnique();

			builder.Property(x => x.Title).HasMaxLength(100);
			builder.Property(x => x.Summary).HasMaxLength(1500);
		}
	}
}