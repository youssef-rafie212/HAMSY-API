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

        public AIOptimizationController(IAIOptimizationService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        public IActionResult Optimize(SourceOptRequestDto req)
        {
            return Ok(_service.Optimize(req));
        }
    }
}
