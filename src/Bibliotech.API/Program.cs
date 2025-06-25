using System.Text;
using Bibliotech.Core.Configuration;
using Bibliotech.Core.Repositories;
using Bibliotech.Core.Services;
using Bibliotech.Modules.Infrastructure.Data;
using Bibliotech.Modules.Infrastructure.Repositories;
using Bibliotech.Modules.Infrastructure.Scripts;
using Bibliotech.Modules.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "postgresql",
        tags: new[] { "database", "postgresql" }
    ).AddMongoDb(
        builder.Configuration.GetConnectionString("MongoDb")!,
        name: "mongodb",
        tags: new[] { "database", "mongodb" }
    );

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName)
);

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; ;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = jwtSettings.ValidateIssuer,
        ValidateAudience = jwtSettings.ValidateAudience,
        ValidateLifetime = jwtSettings.ValidateLifetime,
        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User", "Admin"));
    options.AddPolicy("RequireEmailVerified", policy => policy.RequireClaim("emailVerified", "True"));
});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var mongoContext = scope.ServiceProvider.GetRequiredService<MongoDBContext>();
    await MongoDbInitializer.InitializeAsync(mongoContext);
}

app.Run();

