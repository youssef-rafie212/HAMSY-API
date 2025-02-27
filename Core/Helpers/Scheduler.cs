using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;

namespace Core.Helpers
{
	public class Scheduler
	{
		private Dictionary<string, List<Instruction>> blocks = new();

		public Scheduler(List<string> assembly)
		{
			ParseBlocks(assembly);
			foreach (var block in blocks.Keys.ToList())
			{
				BuildDependencyGraph(block);
				ComputeCriticalPaths(block);
				ScheduleBlock(block);
			}
		}

		private void ParseBlocks(List<string> assembly)
		{
			string currentBlock = "global";
			blocks[currentBlock] = new List<Instruction>();

			foreach (var line in assembly)
			{
				if (line.EndsWith(":"))
				{
					currentBlock = line.TrimEnd(':');
					blocks[currentBlock] = new List<Instruction>();
				}
				else
				{
					blocks[currentBlock].Add(new Instruction(line));
				}
			}
		}

		private void BuildDependencyGraph(string blockName)
		{
			var instructions = blocks[blockName];
			for (int i = 0; i < instructions.Count; i++)
			{
				var instruction1 = instructions[i];
				var parts1 = instruction1.Code.Split("->", StringSplitOptions.TrimEntries);
				if (parts1.Length < 2) continue;
				string destOperand = parts1[1];

				for (int j = i + 1; j < instructions.Count; j++)
				{
					var instruction2 = instructions[j];
					string[] srcOperands = instruction2.Code.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

					if (srcOperands.Contains(destOperand))
					{
						instructions[i].Successors.Add(instruction2);
						instructions[j].DepCount++;
					}
				}
			}
		}

		private void ComputeCriticalPaths(string blockName)
		{
			var memo = new Dictionary<string, int>();
			var instructions = blocks[blockName];

			foreach (var instr in instructions)
			{
				instr.Latency = GetLatency(instr.Code);
				instr.CriticalPath = ComputeCriticalPath(instr, memo);
			}
		}

		private int ComputeCriticalPath(Instruction instr, Dictionary<string, int> memo)
		{
			if (memo.TryGetValue(instr.Code, out int cachedPath))
				return cachedPath;

			if (instr.Successors.Count == 0)
				return instr.Latency;

			int criticalPath = instr.Latency + instr.Successors.Max(s => ComputeCriticalPath(s, memo));

			memo[instr.Code] = criticalPath;
			return criticalPath;
		}

		private int GetLatency(string code)
		{
			if (code.StartsWith("load")) return 3;
			if (code.StartsWith("store")) return 2;
			if (code.StartsWith("add") || code.StartsWith("sub")) return 1;
			if (code.StartsWith("mult") || code.StartsWith("div")) return 2;
			if (code.StartsWith("cmp")) return 1;
			if (code.StartsWith("call")) return 4;
			return 1;
		}

		private void ScheduleBlock(string blockName)
		{
			var instructions = blocks[blockName];
			List<Instruction> readyList = instructions.Where(i => i.DepCount == 0)
													   .OrderByDescending(i => i.CriticalPath)
													   .ToList();
			List<Instruction> activeList = new();
			List<string> scheduledInstructions = new();
			int curCycle = 0;

			while (readyList.Any() || activeList.Any())
			{
				if (readyList.Any())
				{
					var op = readyList.First();
					readyList.RemoveAt(0);
					op.Cycle = curCycle;
					activeList.Add(op);
					scheduledInstructions.Add($"{curCycle} : {op.Code}");
				}
				else
				{
					scheduledInstructions.Add($"{curCycle} : stall");
				}

				curCycle++;
				var toRemove = activeList.Where(op => op.Cycle + op.Latency == curCycle).ToList();

				foreach (var op in toRemove)
				{
					activeList.Remove(op);
					foreach (var succ in op.Successors)
					{
						succ.DepCount--;
						if (succ.DepCount == 0)
							readyList.Add(succ);
					}
				}
			}
			blocks[blockName] = scheduledInstructions.Select(s => new Instruction(s)).ToList();
		}

		public List<string> GetScheduledCode()
		{
			List<string> finalCode = new();
			foreach (var block in blocks)
			{
				finalCode.Add(block.Key + ":");
				finalCode.AddRange(block.Value.Select(instr => instr.Code));
			}
			return finalCode;
		}
	}
}
