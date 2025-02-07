namespace Core.DTO
{
    public class RegAllocResponseDto
    {
        public string Assembly { get; set; } = string.Empty;
        public List<string> Notes { get; set; } = [];
        public List<string> Errors { get; set; } = [];
    }
}
