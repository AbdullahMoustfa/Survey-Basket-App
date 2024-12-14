using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Api;
using SurveyApp.Api.Persistence;
using SurveyApp.Api.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
