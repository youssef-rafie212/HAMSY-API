using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class IROptRequestDto
    {
        [Required]
        public TreeNode AST { get; set; }
        [Required]
        public TreeNode ParseTree { get; set; }
    }
}
