public class Instruction
{
	public string Code { get; set; } = "";
	public int Cycle { get; set; } = -1; // Start cycle
	public int Latency { get; set; } = 1; // Default latency
	public int DepCount { get; set; } = 0; // Number of dependencies
	public int CriticalPath { get; set; } = 0; // Longest dependency chain
	public List<Instruction> Successors { get; set; }

	public bool IsLabel => Code.EndsWith(":");

	public Instruction(string code)
	{
		Code = code;
		Successors = new List<Instruction>();
	}

}