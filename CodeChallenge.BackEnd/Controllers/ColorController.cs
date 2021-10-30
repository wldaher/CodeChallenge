using CodeChallenge.Repository;
using CodeChallenge.Repository.Entities;

using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColorController : ControllerBase
    {
        private readonly ILogger<FlooringController> _logger;
        private readonly ICodeChallengeDbContext _dbContext;

        public ColorController(ILogger<FlooringController> logger, ICodeChallengeDbContext dbContext)
        {
            this._logger = logger;
            this._dbContext = dbContext;
        }

        [HttpGet(Name = "GetColors")]
        public IEnumerable<ColorEntity> Get()
        {
            this._logger.LogInformation("GetColor called");
            return this._dbContext.GetColors();
        }
    }
}