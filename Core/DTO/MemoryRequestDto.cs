using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class MemoryRequestDto
    {
        [Required]
        public string SourceCode { get; set; } = string.Empty;
    }
}
