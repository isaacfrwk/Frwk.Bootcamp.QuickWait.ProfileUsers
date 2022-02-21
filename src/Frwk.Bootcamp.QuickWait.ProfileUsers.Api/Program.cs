using FluentValidation.AspNetCore;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Validator;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.Data.Context;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.IOC;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
                           options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
        .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UserValidator>());
        

builder.Services.AddServicesProfileUsers();
builder.Services.AddRepositoriesProfileUsers();
builder.Services.AddHostedService();
builder.Services.AddDatabaseContext(builder.Configuration);

builder.Services.AddHealthChecks()
                .AddSqlServer(connectionString: builder.Configuration.GetConnectionString("ConnectionString"), name: "Instancia do sql server");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = p => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

await app.UseDatabaseConfiguration();

app.Run();
