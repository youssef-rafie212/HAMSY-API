using Core.DTO;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace HAMSY_API.Controllers
{
    [ApiController]
    [Route("api/memory-analysis")]
    public class MemoryAnalysisController : ControllerBase
    {
        private readonly IMemoryAnalysisService _memoryAnalysisService;

        public MemoryAnalysisController(IMemoryAnalysisService memoryAnalysisService)
        {
            _memoryAnalysisService = memoryAnalysisService;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Analyze(MemoryRequestDto req)
        {
            return Ok(_memoryAnalysisService.MemoryAnalysis(req));
        }
    }
}
