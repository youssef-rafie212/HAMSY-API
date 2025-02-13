namespace Core.Domain.Entities
{
    public class MemoryData
    {
        public List<string> DataSegment { get; set; } = [];
        public List<StackFrame> Stack { get; set; } = [];
    }
}
