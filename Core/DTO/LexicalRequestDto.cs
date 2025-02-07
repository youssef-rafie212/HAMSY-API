using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class LexicalRequestDto
    {
        [Required]
        public string SourceCode { get; set; } = string.Empty;
    }
}
