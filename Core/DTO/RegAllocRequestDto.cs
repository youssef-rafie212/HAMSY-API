using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class RegAllocRequestDto
    {
        [Required]
        public List<string> Assembly { get; set; }
    }
}
