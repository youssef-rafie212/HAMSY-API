using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class InsSelRequestDto
    {
        [Required]
        public string IR { get; set; }
    }
}
