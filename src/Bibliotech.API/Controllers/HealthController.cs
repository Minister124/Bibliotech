using Bibliotech.Core.Models;
using Bibliotech.Modules.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Bibliotech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly BibliotechDbContext _dbContext;
        private readonly MongoDBContext _mongoContext;
        private readonly ILogger<HealthController> _logger;

        public HealthController(BibliotechDbContext dbContext, MongoDBContext mongoContext, ILogger<HealthController> logger)
        {
            _dbContext = dbContext;
            _mongoContext = mongoContext;
            _logger = logger;
        }

        [HttpGet("database")]
        public async Task<IActionResult> CheckDatabaseConnections()
        {
            try
            {
                var canConnectToPostgres = await _dbContext.Database.CanConnectAsync();

                var mongoCollections = await _mongoContext.ReadingSessions.CountDocumentsAsync(FilterDefinition<ReadingSession>.Empty);

                return Ok(new
                {
                    PostgreSQL = canConnectToPostgres ? "Connected" : "Failed",
                    MongoDB = "Connected",
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database Connection Check Failed");
                return StatusCode(500, new {
                    Error = "Database Connection Failed",
                    Details = ex.Message
                });
            }
        }
    }
}
