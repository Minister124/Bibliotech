using Bibliotech.Core.Configuration;
using Bibliotech.Core.Repositories;
using Bibliotech.Modules.Infrastructure.Data;
using Bibliotech.Modules.Infrastructure.Repositories;
using Bibliotech.Modules.Infrastructure.Scripts;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(DatabaseSettings.SectionName));

builder.Services.AddDbContext<BibliotechDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly("Bibliotech.API");
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null
        );
    });

    if(builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

builder.Services.AddSingleton<IMongoClient>
(
    serviceProvider =>
    {
        var connectionString = builder.Configuration.GetConnectionString("MongoDb");
        return new MongoClient(connectionString);
    }
);

builder.Services.AddScoped<MongoDBContext>(
    serviceProvider =>
    {
        var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
        var databaseSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
        return new MongoDBContext(mongoClient, databaseSettings.MongoDbDatabaseName);
    }
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReadingSessionRepository, ReadingSessionRepository>();

builder.Services.AddHealthChecks()
    .AddNpgSql
    (
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "postgresql",
        tags: new[] { "database", "postgresql" }
    )
    .AddMongoDb
    (
        builder.Configuration.GetConnectionString("MongoDb")!,
        name: "mongodb",
        tags: new[] { "database", "mongodb" }
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var mongoContext = scope.ServiceProvider.GetRequiredService<MongoDBContext>();
    await MongoDbInitializer.InitializeAsync(mongoContext);
}

app.MapHealthChecks("/health", new HealthCheckOptions {
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions {
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.UseHttpsRedirection();

app.Run();

