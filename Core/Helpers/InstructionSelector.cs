using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Helpers
{
	public class InstructionSelector
	{
		private int registerCounter = 0;
		private bool flag = false;
		private string lastReg = "";
		private string lastLine = "";
		public List<string> ILOCInstructions { get; set; }

		public InstructionSelector()
		{
			ILOCInstructions = new List<string>();
		}

		private string NewRegister() => $"r{registerCounter++}";

		public List<string> GereateILOC(List<string> TACInstructions)
		{
			ILOCInstructions.Clear();
			foreach (var line in TACInstructions)
			{
				ConvertToILOC(line);
			}
			return ILOCInstructions;
		}

		public void Load(string src, string reg)
		{
			if (int.TryParse(src, out _))
			{ // x = 5
				ILOCInstructions.Add($"loadI {src} -> {reg}");
			}
			else
			{ // x = m
				ILOCInstructions.Add($"load {src} -> {reg}");
			}
		}

		public void ConvertToILOC(string tacInstruction)
		{
			string[] tokens = tacInstruction.Split(' ');

			if (tokens.Length > 2 && tokens[1] == "=" && flag)
			{
				ILOCInstructions.Add($"store {lastReg} -> {tokens[0]}");
				flag = false;
				return;
			}

			if (tokens.Length == 3 && tokens[1] == "=") 
			{ // m = 5
				
				string dest = tokens[0];
				string src = tokens[2];
				string reg = NewRegister();

				Load(src, reg);
				ILOCInstructions.Add($"store {reg} -> {dest}");

				
			}
			else if (tokens.Length == 5 && tokens[1] == "=" /*&& (tokens[3] != "+" || tokens[3] != "-" || tokens[3] != "*" || tokens[3] != "/" || tokens[3] != "%")*/ )
			{  // t = m > 0
				string dest = tokens[0];
				string src1 = tokens[2];
				string src2 = tokens[4];
				string op = tokens[3];

				string reg1 = NewRegister();
				string reg2 = NewRegister();
				string reg3 = NewRegister();
				lastReg = reg3;

				Load(src1, reg1);
				Load(src2, reg2);

				string comp = "";

				switch (op)
				{
					case ">": comp = "cmp_GT"; break;
					case "<": comp = "cmp_LT"; break;
					case "==": comp = "cmp_EQ"; break;
					case "!=": comp = "cmp_NE"; break;
					case ">=": comp = "cmp_GE"; break;
					case "<=": comp = "cmp_LE"; break;
					case "+": comp = "add"; break;
					case "-": comp = "sub"; break;
					case "/": comp = "div"; break;
					case "*": comp = "mult"; break;
				}

				ILOCInstructions.Add($"{comp} {reg1} , {reg2} -> {reg3}");

				if (op == "+" || op == "-" || op == "/" || op == "*")
				{
					lastLine = tacInstruction;
					flag = true;
				}

			}
			
			else if (tokens[0] == "if")
			{ // if t goto l1
				string l1 = tokens[3];
				string num = new string(tokens[3].Where(char.IsDigit).ToArray());
				int number= Int32.Parse(num);

				ILOCInstructions.Add($"cbr {lastReg} -> L{number}, L{number+1}");
			}
			else if (tokens[0] == "goto" || tokens[0] == "declare")

			{ // goto l2
				return;
			}
			else if (tokens[0].EndsWith(":"))
			{
				ILOCInstructions.Add($"{tokens[0]}");
			}
			else if (tokens[0] == "return")
			{
				string returnValue = tokens[1];

				if (flag)
				{
					ILOCInstructions.Add($"store {lastReg} -> RET");
					flag = false;
					return;
				}
				string reg = NewRegister();

				Load(returnValue, reg);
				ILOCInstructions.Add($"store {reg} -> RET");
			}
			else if (tokens[0] == "function")
			{
				string functionDeclaration = tacInstruction.Split(' ')[1];
				string functionName = functionDeclaration.Split('(')[0];
				ILOCInstructions.Add($"label {functionName}:");
			}
			else if (tokens[0] == "param")
			{
				string param = tokens[1];
				string reg = NewRegister();
				Load(param, reg);
				ILOCInstructions.Add($"push_param {reg}");
			}
			else if (tokens[2] == "call")
			{
				//param num 
				// param 3
				// t1 = call multiply
				// result = t1
				// 
				//loadI num => r1        // Load the value of 'num' into register r1
				// loadI 3 => r2          // Load the immediate value 3 into register r2
				// call multiply, r1, r2 => r3  // Call 'multiply' with r1 and r2, result in r3
				// store r3 => result     // Store the result from r3 into memory location 'result'
				
				 string functionName = tokens[3];
				 string reg = NewRegister();
				 lastReg = reg;
				 flag = true;
				ILOCInstructions.Add($"call {functionName} -> {reg}");
			}
			else
			{
				throw new Exception($"Unknown TAC instruction: {tacInstruction}");
			}
		}
	}
}
