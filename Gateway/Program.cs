var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddOpenApiForYarp();

var app = builder.Build();

app.MapReverseProxy();
app.MapOpenApiForYarp();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/openapi/all.json",
        "OrderDispatch API"
    );
});

app.Run();