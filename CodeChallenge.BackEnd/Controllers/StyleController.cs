using CodeChallenge.Repository;
using CodeChallenge.Repository.Entities;

using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StyleController : ControllerBase
    {
        private readonly ILogger<FlooringController> _logger;
        private readonly ICodeChallengeDbContext _dbContext;

        public StyleController(ILogger<FlooringController> logger, ICodeChallengeDbContext dbContext)
        {
            this._logger = logger;
            this._dbContext = dbContext;
        }

        [HttpGet(Name = "GetStyles")]
        public IEnumerable<StyleEntity> Get()
        {
            this._logger.LogInformation("GetStyle called");
            return this._dbContext.GetStyles();
        }
    }
}