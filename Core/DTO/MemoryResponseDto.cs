using Core.Domain.Entities;

namespace Core.DTO
{
    public class MemoryResponseDto
    {
        public MemoryData Data { get; set; } = new();
        public List<string> Notes { get; set; } = [];
        public List<string> Errors { get; set; } = [];
    }
}
