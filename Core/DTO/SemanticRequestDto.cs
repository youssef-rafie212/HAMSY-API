using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class SemanticRequestDto
    {
        [Required]
        public TreeNode ParseTree { get; set; }
    }
}
