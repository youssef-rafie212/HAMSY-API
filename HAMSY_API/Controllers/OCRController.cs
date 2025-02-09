using Core.DTO;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace HAMSY_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OCRController : ControllerBase
    {
        private readonly IOCRService _ocrService;

        public OCRController(IOCRService ocrService)
        {
            _ocrService = ocrService;
        }

        [HttpPost("extract-code")]
        public async Task<IActionResult> ExtractCode([FromForm] OCRRequestDto req)
        {
            try
            {
                string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                string imgPath = Path.Combine(uploadsPath, req.SourceCodeImage.FileName);
                using (FileStream stream = new(imgPath, FileMode.Create))
                {
                    await req.SourceCodeImage.CopyToAsync(stream);
                }

                OCRResponseDto res = _ocrService.ExtractCode(imgPath);

                return Ok(res);
            }
            catch
            {
                return StatusCode(500, "Server couldn't extract the image file.");
            }
        }
    }
}
