using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class SymbolTablesRequestDto
    {
        [Required]
        public TreeNode ParseTree { get; set; }
    }
}
