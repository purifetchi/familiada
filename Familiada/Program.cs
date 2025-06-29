using System.Text.Json;
using System.Text.Json.Serialization;
using Familiada.Hubs;
using Familiada.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddScoped<GameSessionService>();
builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.Converters
            .Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

var app = builder.Build();

app.MapHub<FeudHub>("/hub");
app.MapGet("/", () => "Hello World!");

app.UseCors("CorsPolicy");

app.Run();