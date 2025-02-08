namespace Core.DTO
{
    public class IRGenResponseDto
    {
        public string IR { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = [];
    }
}
