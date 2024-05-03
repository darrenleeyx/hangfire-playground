using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(x =>
    x.UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseInMemoryStorage());

builder.Services.AddHangfireServer(x => x.SchedulePollingInterval = TimeSpan.FromSeconds(15));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHangfireDashboard();

RecurringJob.AddOrUpdate("Recurring Job", () => Console.WriteLine("Hello from hangfire"), Cron.Minutely);

app.MapGet("/job", (IBackgroundJobClient jobClient) =>
{
    jobClient.Enqueue(() => Console.WriteLine("Hello from BG"));
    return Results.Ok("Hello job!");
});

app.UseHttpsRedirection();

app.Run();
