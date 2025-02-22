using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class InsSelRequestDto
    {
        [Required]
        public List<string> IR { get; set; }
    }
}
