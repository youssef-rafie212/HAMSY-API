using Core.Domain.Entities;

namespace Core.DTO
{
    public class LexicalResponseDto
    {
        public List<Token> Tokens { get; set; } = [];
        public List<string> Notes { get; set; } = [];
        public List<string> Errors { get; set; } = [];
    }
}
