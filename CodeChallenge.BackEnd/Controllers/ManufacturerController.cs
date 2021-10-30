using CodeChallenge.Repository;
using CodeChallenge.Repository.DTOs;
using CodeChallenge.Repository.Entities;

using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManufacturerController : ControllerBase
    {
        private readonly ILogger<FlooringController> _logger;
        private readonly ICodeChallengeDbContext _dbContext;

        public ManufacturerController(ILogger<FlooringController> logger, ICodeChallengeDbContext dbContext)
        {
            this._logger = logger;
            this._dbContext = dbContext;
        }

        [HttpGet(Name = "GetManufacturers")]
        public IEnumerable<ManufacturerEntity> Get()
        {
            this._logger.LogInformation("GetManufacturer called");
            return this._dbContext.GetManufacturers();
        }
    }
}