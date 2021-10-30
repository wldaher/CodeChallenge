using CodeChallenge.Repository;
using CodeChallenge.Repository.DTOs;
using CodeChallenge.Repository.Entities;

using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TypeController : ControllerBase
    {
        private readonly ILogger<FlooringController> _logger;
        private readonly ICodeChallengeDbContext _dbContext;

        public TypeController(ILogger<FlooringController> logger, ICodeChallengeDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetTypes")]
        public IEnumerable<TypeEntity> Get()
        {
            _logger.LogInformation("GetType called");
            return this._dbContext.GetTypes();
        }
    }
}