using Frwk.Bootcamp.QuickWait.ProfileUsers.Application;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
                           options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddServicesProfileUsers();
builder.Services.AddRepositoriesProfileUsers();

builder.Services.AddDbContext<DBContext>(options =>
                options.UseSqlServer($"Data Source=host.docker.internal, 1433;" +
                $"Initial Catalog=DbProfileUser;" +
                $"Persist Security Info=True;" +
                $"User ID=sa;" +
                $"Password=Pr0f!leUs3r"));

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

app.MapControllers();

app.Run();
