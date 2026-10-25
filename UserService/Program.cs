using Keycloak.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
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
    options.LoginClientId = Environment.GetEnvironmentVariable("KEYCLOAK_LOGIN_CLIENT_ID")!;
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


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MetadataAddress = Environment.GetEnvironmentVariable("KEYCLOAK_METADATA_ADDRESS")!;
        
        options.RequireHttpsMetadata = false;
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = Environment.GetEnvironmentVariable("KEYCLOAK_AUTHORITY"),
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] =
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Введите JWT-токен",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

        return Task.CompletedTask;
    });

    options.AddOperationTransformer((operation, context, _) =>
    {
        operation.Security ??= [];
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
        });

        return Task.CompletedTask;
    });
});

var app = builder.Build();


app.UseCors("AllowSwagger");
app.MapOpenApi().RequireCors("AllowSwagger"); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
