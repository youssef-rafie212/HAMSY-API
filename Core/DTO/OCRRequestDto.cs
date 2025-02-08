using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class OCRRequestDto
    {
        [Required]
        public byte[] SourceCodeImage { get; set; }
    }
}
