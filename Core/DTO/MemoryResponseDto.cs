using Core.Domain.Entities;

namespace Core.DTO
{
    public class MemoryResponseDto
    {
        public List<ExecutionStep> ExecutionSteps { get; set; } = [];
        public List<string> Errors { get; set; } = [];
    }
}
