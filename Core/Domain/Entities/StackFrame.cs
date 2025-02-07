namespace Core.Domain.Entities
{
    public class StackFrame
    {
        public string FunctionName { get; set; } = string.Empty;
        public Dictionary<string, int> FrameData { get; set; } = [];
    }
}
