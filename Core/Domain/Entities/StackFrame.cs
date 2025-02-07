namespace Core.Domain.Entities
{
    public class StackFrame
    {
        public string FunctionName { get; set; } = string.Empty;
        // key for variable name, value for variable value.
        public Dictionary<string, int> FrameData { get; set; } = [];
    }
}
