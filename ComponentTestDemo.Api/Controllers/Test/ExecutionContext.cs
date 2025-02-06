using ComponentTestDemo.Api.Database.ComponentTestDemo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComponentTestDemo.Api.Controllers.Test
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExecutionContextController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public ExecutionContextController(AppDbContext dbContext, IConfiguration configuration) 
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("CleanUp")]
        public IActionResult CleanUp()
        {
            var isRunningComponentTest = _configuration.GetValue<bool>("RunningComponentTest");

            if (!isRunningComponentTest)
            {
                return NotFound(new { message = "Component tests are not running." });
            }

            _dbContext.Database.ExecuteSqlRaw(@"
                     DELETE FROM Users;
            ");
            return Ok(new { message = "Database cleaned up successfully." });
        }
    }
}
