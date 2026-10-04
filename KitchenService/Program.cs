using KitchenService.Data;
using KitchenService.Features.Kitchen.Clients.OrderService;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// сервисы
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddHttpClient<IOrderHttpClient, OrderHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["OrderServiceSettings:BaseUrl"]!);
});

builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes(); 
});

var app = builder.Build();

// миграции
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.MapOpenApi();
app.MapControllers();

app.Run();