using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;

namespace Core.Helpers
{
	public class Scheduler
	{
		private List<string> instructions;
		private List<string> scheduledInstructions;

		public Scheduler(List<string> assembly)
		{
			scheduledInstructions = new List<string>();
			instructions = assembly;
		}

		public List<string> GetScheduledCode()
		{
			int currCycle = 0;
			int latency = 1;
			int cntr = 0;

			foreach (var instr in instructions)
			{
				scheduledInstructions.Add($"{currCycle}: {instr}");
				latency = GetLatency(instr);
				cntr = currCycle + latency;
				currCycle++;
				while (currCycle < cntr)
				{
					scheduledInstructions.Add($"{currCycle}: stall");
					currCycle++;
				}
			}

			return scheduledInstructions;
		}

		private int GetLatency(string code)
		{
			if (code.StartsWith("loadI")) return 1;
			if (code.StartsWith("load")) return 3;
			if (code.StartsWith("store")) return 2;
			if (code.StartsWith("add") || code.StartsWith("sub")) return 1;
			if (code.StartsWith("mult") || code.StartsWith("div")) return 2;
			if (code.StartsWith("cmp")) return 1;
			if (code.StartsWith("call")) return 4;
			return 1;
		}

	}
}
