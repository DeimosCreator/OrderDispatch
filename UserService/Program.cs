using Keycloak.Net;
using Microsoft.Extensions.Options;
using UserService.Options;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// сервисы
builder.Services.AddControllers();

builder.Services.AddHttpClient();

builder.Services.AddOpenApi();

builder.Services.Configure<KeycloakSettings>(options =>
{
    options.Url = Environment.GetEnvironmentVariable("KEYCLOAK_URL")!;
    options.Realm = Environment.GetEnvironmentVariable("KEYCLOAK_REALM")!;
    options.AdminClientId = Environment.GetEnvironmentVariable("KEYCLOAK_ADMIN_CLIENT_ID")!;
    options.AdminClientSecret = Environment.GetEnvironmentVariable("KEYCLOAK_ADMIN_CLIENT_SECRET")!;
});

builder.Services.AddScoped<KeycloakClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<KeycloakSettings>>().Value;

    return new KeycloakClient(
        settings.Url,
        settings.AdminClientSecret,
        new KeycloakOptions(adminClientId: settings.AdminClientId));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

app.UseCors("AllowSwagger");
app.MapOpenApi().RequireCors("AllowSwagger"); 

app.MapControllers();

app.Run();
