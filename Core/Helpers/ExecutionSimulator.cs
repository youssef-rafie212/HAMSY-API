using Core.Domain.Entities;

namespace Core.Helpers
{
    public class ExecutionSimulator
    {
        private int _executionCount = 0;
        public List<ExecutionStep> ExecutionSteps { get; set; } = [];

        public void AddStep(string name, MemoryData memoryState)
        {
            ExecutionSteps.Add(new()
            {
                StepNumber = ++_executionCount,
                StepName = name,
                MemoryState = CloneMemory(memoryState),
            });
        }

        private MemoryData CloneMemory(MemoryData data)
        {
            List<StackFrame> newStack = [];
            foreach (StackFrame sf in data.Stack)
            {
                newStack.Add(new StackFrame()
                {
                    FunctionName = sf.FunctionName,
                    FrameData = new(sf.FrameData)
                });
            }

            return new()
            {
                DataSegment = new(data.DataSegment),
                Stack = newStack,
            };
        }

    }
}
