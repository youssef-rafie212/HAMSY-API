using Core.Domain.Entities;

namespace Core.DTO
{
    public class SyntaxResponseDto
    {
        public TreeNode ParseTree { get; set; } = new();
        public List<string> Errors { get; set; } = [];
    }
}
