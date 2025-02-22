namespace Core.DTO
{
    public class InsSelResponseDto
    {
	    public List<string> Assembly { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = [];
    }
}
