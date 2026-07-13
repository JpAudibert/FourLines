const string appName = "FourLines";
const string appVersion = "1.0.0";

const string openTelemetryEndpoint = "http://localhost:4317";

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

try
{
    Log.Information("Starting application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.ConfigureSerilog(appName, appVersion, openTelemetryEndpoint);

    builder.Services.ConfigureProblemDetails();
    builder.Services.ConfigureOpenTelemtryTracingAndMetrics(openTelemetryEndpoint);

    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var secretKey = jwtSettings["Secret"];

    // Add services to the container.
    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services
        .AddDomain()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });

    builder.Services.ConfigureJwtAuthentication(jwtSettings["Issuer"]!, jwtSettings["Audience"]!, secretKey!);
    
    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.ConfigureSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options => options.AddDocument("v1", "API Version 1", isDefault: true));
    }

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Application started successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
