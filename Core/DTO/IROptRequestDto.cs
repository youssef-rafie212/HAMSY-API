using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class IROptRequestDto
    {
        [Required]
        public string IR { get; set; } = string.Empty;
    }
}
