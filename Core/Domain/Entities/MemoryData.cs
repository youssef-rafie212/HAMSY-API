namespace Core.Domain.Entities
{
    public class MemoryData
    {
        public Dictionary<string, int> DataSegment { get; set; } = [];
        public List<StackFrame> Stack { get; set; } = [];
    }
}
