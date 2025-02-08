using Core.Domain.Entities;

namespace Core.DTO
{
    public class SemanticResponseDto
    {
        public TreeNode AST { get; set; } = new();
        public List<string> Errors { get; set; } = [];
    }
}
