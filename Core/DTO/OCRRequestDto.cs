using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class OCRRequestDto
    {
        [Required]
        public IFormFile SourceCodeImage { get; set; }
    }
}
