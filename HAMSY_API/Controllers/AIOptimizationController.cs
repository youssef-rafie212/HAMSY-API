using Core.DTO;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace HAMSY_API.Controllers
{
    [ApiController]
    [Route("/api/ai-optimization")]
    public class AIOptimizationController : ControllerBase
    {
        private readonly IAIOptimizationService _service;
        private readonly IConfiguration _configuration;

        public AIOptimizationController(IAIOptimizationService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Optimize(SourceOptRequestDto req)
        {
            string? apiKey = _configuration["API_KEY"];
            if (apiKey == null)
            {
                return StatusCode(500);
            }
            return Ok(await _service.Optimize(req, apiKey));
        }
    }
}
