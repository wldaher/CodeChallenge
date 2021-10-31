using CodeChallenge.Repository;
using CodeChallenge.Repository.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlooringController : ControllerBase
    {
        private readonly ILogger<FlooringController> _logger;
        private readonly ICodeChallengeDbContext _dbContext;

        public FlooringController(ILogger<FlooringController> logger, ICodeChallengeDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetFlooring")]
        public IEnumerable<FlooringDTO> Get(string? manufacturer, int? type, string? color, string? style, int? size)
        {
            _logger.LogInformation("GetFlooring called with params manufacturer:{manufacturer}, type: {type}, color: {color}, style: {style}, size {size}", manufacturer, type, color, style, size); 
            return this._dbContext.SearchFlooring(manufacturer, type, color, style, size);
        }

        [HttpPut(Name = "PutFlooring")]
        public FlooringDTO Put(EditableFlooringDTO flooringDTO)
        {
            _logger.LogInformation("PutFlooring called with params {flooringDTO}", flooringDTO.ToString());
            return this._dbContext.SaveFlooring(flooringDTO);
        }

        [HttpPost(Name = "PostFlooring")]
        public FlooringDTO Post(EditableFlooringDTO flooringDTO)
        {
            _logger.LogInformation("PostFlooring called with params {flooringDTO}", flooringDTO.ToString());
            return this._dbContext.SaveFlooring(flooringDTO);
        }

        [HttpDelete(Name = "DeleteFlooring")]
        public int Delete(int flooringId)
        {
            _logger.LogInformation("DeleteFlooring called with params {flooringId}", flooringId);
            return this._dbContext.DeleteFlooring(flooringId);
        }
    }
}