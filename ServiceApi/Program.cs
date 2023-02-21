using ServiceApi;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// For the background worker as to not block the API
// References: https://learn.microsoft.com/en-us/dotnet/core/extensions/workers
// References: https://learn.microsoft.com/en-us/dotnet/core/extensions/queue-service
// References: https://www.youtube.com/watch?v=8Sy69b6-nj0

builder.Services.AddHostedService<BackgroundWorkerService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
