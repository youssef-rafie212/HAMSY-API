using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class SourceOptRequestDto
    {
        [Required]
        public string SourceCode { get; set; }
    }
}
