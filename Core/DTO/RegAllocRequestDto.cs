using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class RegAllocRequestDto
    {
        [Required]
        public string Assembly { get; set; }
    }
}
