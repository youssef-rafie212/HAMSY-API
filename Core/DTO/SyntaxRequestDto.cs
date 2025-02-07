using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class SyntaxRequestDto
    {
        [Required]
        public string SourceCode { get; set; } = string.Empty;
    }
}
