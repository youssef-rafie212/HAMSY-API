namespace Core.Domain.Entities
{
    public class StackFrame
    {
        public string FunctionName { get; set; } = string.Empty;
        public List<string> FrameData { get; set; } = [];
    }
}
