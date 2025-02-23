using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class InsSchedRequestDto
    {
        [Required]
        public List<string> Assembly { get; set; }
    }
}
