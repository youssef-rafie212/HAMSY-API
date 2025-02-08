namespace Core.DTO
{
    public class InsSchedResponseDto
    {
        public string Assembly { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = [];
    }
}
