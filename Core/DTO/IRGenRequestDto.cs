using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class IRGenRequestDto
    {
        [Required]
        public TreeNode AST { get; set; } = new();
    }
}
