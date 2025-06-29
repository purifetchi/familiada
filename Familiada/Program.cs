using Familiada.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<FeudHub>("/hub");
app.MapGet("/", () => "Hello World!");

app.Run();