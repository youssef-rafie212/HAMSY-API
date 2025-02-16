using Core.Domain.Entities;

namespace Core.Helpers
{
	public class TACGenerator
	{
		private int tempCounter = 0;
		private int labelCounter = 0;

		public List<string> TACInstructions { get; private set; }

		public TACGenerator()
		{
			TACInstructions = new List<string>();
		}
		private string NewTemp() => $"t{tempCounter++}";
		private string NewLabel() => $"L{labelCounter++}";


		public List<string> GenerateTAC(TreeNode ast)
		{
			TACInstructions.Clear();
			TraverseAST(ast);
			return TACInstructions;
		}

		private string TraverseAST(TreeNode node, List<string> functionDeclarations = null)
		{
			switch (node.Type)
			{
				case "Program":
					foreach (var child in node.Children)
					{
						TraverseAST(child);
					}
					break;
				//case "VariableDeclaration":
				//	string varName = node.Value; //var name
				//	string expres = TraverseAST(node.Children[0]); //expression

				//	TACInstructions.Add($"{varName} = {expres}");

				//	return varName;
				case "VariableDeclaration":
					// If inside a function, store it for later use instead of adding immediately
					if (functionDeclarations != null)
					{
						functionDeclarations.Add($"{node.Value} = 0;"); // Initialize to default
					}
					else
					{
						string varName = node.Value;
						string expres = TraverseAST(node.Children[0]);
						TACInstructions.Add($"{varName} = {expres}");
					}
					break;

				case "Assignment":
					string targetName = node.Value;
					string expValue = TraverseAST(node.Children[0]);

					TACInstructions.Add($"{targetName} = {expValue}");

					return targetName;

				case "BinaryExpression":
					string operation = node.Value;
					string left_operand = node.Children[0].Value;
					string right_operand = node.Children[1].Value;

					string temp_ = NewTemp();

					TACInstructions.Add($"{temp_} = {left_operand} {operation} {right_operand}");

					return temp_;

				case "ReturnStatement":
					string returnValue = TraverseAST(node.Children[0]);
					TACInstructions.Add($"return {returnValue}");
					return returnValue;

				case "Condition":
					string op = node.Value;
					string l_operand = node.Children[0].Value;
					string r_operand = node.Children[1].Value;
					string temp = NewTemp();
					TACInstructions.Add($"{temp} = {l_operand} {op} {r_operand}");
					return temp;

				case "IfStatement":
					string tcondition = TraverseAST(node.Children[0]); // Condition
					string ifLabel = NewLabel();
					string endLabel = NewLabel();

					TACInstructions.Add($"if {tcondition} goto {ifLabel}");

					// If there is an ElseStatement, generate an ElseLabel
					string elseLabel = null;
					bool hasElse = node.Children.Count > 2 && node.Children[2].Type == "ElseStatement";
					if (hasElse)
					{
						elseLabel = NewLabel();
						TACInstructions.Add($"goto {elseLabel}");
					}
					else
					{
						TACInstructions.Add($"goto {endLabel}");
					}

					// Then block
					TACInstructions.Add($"{ifLabel}:");
					TreeNode thenBlock = node.Children[1];
					foreach (var stmt in thenBlock.Children)
					{
						TraverseAST(stmt);
					}

					if (hasElse)
					{
						TACInstructions.Add($"goto {endLabel}");
						TACInstructions.Add($"{elseLabel}:");

						TreeNode elseBlock = node.Children[2].Children[0]; // ElseBlock inside ElseStatement
						foreach (var stmt in elseBlock.Children)
						{
							TraverseAST(stmt);
						}
					}

					TACInstructions.Add($"{endLabel}:");
					break;


				case "WhileLoop":
					string loopStart = NewLabel();
					string loopend = NewLabel();

					string cond = TraverseAST(node.Children[0]);

					TACInstructions.Add($"if {cond} goto {loopStart}");
					TACInstructions.Add($"goto {loopend}");

					TACInstructions.Add($"{loopStart}:");

					foreach (var stmt in node.Children.Skip(1))
						TraverseAST(stmt);

					//TACInstructions.Add($"goto {loopStart}");
					cond = TraverseAST(node.Children[0]);
					TACInstructions.Add($"if {cond} goto {loopStart}");
					TACInstructions.Add($"goto {loopend}");

					TACInstructions.Add($"{loopend}:");
					break;

				//case "FunctionDefinition":
				//	TACInstructions.Add($"function {node.Value}:");
				//	foreach (var child in node.Children.Skip(1))
				//		TraverseAST(child);
				//	break;

				//case "MainFunction":
				//	TACInstructions.Add($"function main:");
				//	foreach (var stmt in node.Children)
				//		TraverseAST(stmt);
				//	break;
				case "FunctionDefinition":
					TreeNode p = node.Children[0];
					TACInstructions.Add($"function {node.Value} ({p.Children[0].Value}, {p.Children[1].Value}):");

					var decs = new List<string>();
					CollectVariableDeclarations(node, decs);

					// Emit declarations first
					foreach (var decl in decs)
						TACInstructions.Add(decl);

					// Traverse function body (skip first child as it holds function name)
					foreach (var child in node.Children.Skip(1))
						TraverseAST(child);
					break;

				case "MainFunction":
					string inst = $"function {node.Value} ():";
					TACInstructions.Add(inst);

					// Collect all variable declarations in a first pass
					var declarations = new List<string>();
					CollectVariableDeclarations(node, declarations);

					// Emit declarations first
					foreach (var decl in declarations)
						TACInstructions.Add(decl);

					// Traverse function body (skip first child as it holds function name)
					foreach (var child in node.Children)
						TraverseAST(child);
					break;

				case "FunctionCall":
					string funcName = node.Value;
					var arg1 = TraverseAST(node.Children[0]); // First argument
					string arg2 = TraverseAST(node.Children[1]); // Second argument

					TACInstructions.Add($"param {arg1}");
					TACInstructions.Add($"param {arg2}");


					string tempVar = NewTemp(); // Store result in a temporary variable
					TACInstructions.Add($"{tempVar} = call {funcName}");

					return tempVar;

				case "Operand":
					return node.Value;

				case "":
					return "";
				default:
					if (node.Type == "Terminal" && node.Value == "<EOF>")
					{
						return "";
					}
					throw new NotImplementedException($"Unknown node type: {node.Type}");

			}
			return "";
		}
		private void CollectVariableDeclarations(TreeNode node, List<string> declarations)
		{
			if (node.Type == "VariableDeclaration")
			{
				declarations.Add($"declare {node.Value}");
			}

			foreach (var child in node.Children)
			{
				CollectVariableDeclarations(child, declarations);
			}
		}


	}
}