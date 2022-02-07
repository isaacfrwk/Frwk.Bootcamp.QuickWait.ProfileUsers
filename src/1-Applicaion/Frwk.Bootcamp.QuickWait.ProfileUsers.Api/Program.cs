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
builder.Services.AddHostedService();


builder.Services.AddDbContext<DBContext>(options =>
                options.UseSqlServer(@"Data Source=frwkquickwait.database.windows.net;" +
                                     $"Initial Catalog=DbUser;" +
                                     $"Persist Security Info=True;" +
                                     $"User ID=frwkcosmos;" +
                                     $"Password=Fr@m3w0rk"));

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
