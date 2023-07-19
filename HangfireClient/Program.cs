using Hangfire;
using HangfireClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddHangfire(config =>
{
    config.UseInMemoryStorage();
});
builder.Services.AddHangfireServer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/immediately", () => {
    BackgroundJob.Enqueue<ITestService>((p) => p.Run("I am Tarikul!"));
});
app.MapGet("/delay", () =>
{
    BackgroundJob.Schedule<ITestService>((p) => p.Run("I am Tarikul, I will delay 5 second run the schedule"), TimeSpan.FromSeconds(5));
    return "done";
});
app.MapGet("/recurring", () =>
{
    RecurringJob.AddOrUpdate<ITestService>("my-process", (x) => x.CallApi(), "*/15 * * * * *");
    return "done";
});

app.UseHangfireDashboard();

app.Run();

