var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddOpenApiForYarp();

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApiForYarp();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/all.json", "OrderDispatch API");
    });
}

app.MapHealthChecks("/health");
app.MapReverseProxy();

app.Run();