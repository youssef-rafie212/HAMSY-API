namespace Core.DTO
{
    public class IROptResponseDto
    {
        public string OptimizedIR { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = [];
    }
}
