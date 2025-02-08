using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class InsSchedRequestDto
    {
        [Required]
        public string Assembly { get; set; }
    }
}
