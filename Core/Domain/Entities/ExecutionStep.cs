namespace Core.Domain.Entities
{
    public class ExecutionStep
    {
        public int StepNumber { get; set; }
        public string StepName { get; set; } = string.Empty;
        public MemoryData MemoryState { get; set; } = new();
    }
}
