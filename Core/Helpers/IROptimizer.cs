namespace Core.Helpers
{
    public class IROptimizer
    {
        public List<string> IR { get; private set; }

        public IROptimizer(string ir)
        {
            IR = [.. ir.Split('\n')];
        }

        public void ApplyCommonSubexpressionEliminationPass()
        {
            List<string> optimizedIR = [];

            //         scope              expr    varName
            Dictionary<string, Dictionary<string, string>> expressions = [];
            expressions.Add("global", []);

            string currentScope = "global";

            foreach (string i in IR)
            {
                string instruction = i.Trim();
                // assignment
                if (instruction.Contains("=") && !instruction.Contains("call"))
                {
                    string varName = instruction.Split("=")[0].Trim();
                    string expression = instruction.Split("=")[1].Trim();
                    Dictionary<string, string> relatedExpressions = expressions[currentScope];

                    bool predicate = true;
                    bool reversed = false;

                    // If expression operator allows reversing operands.
                    if (expression.Contains("==") || expression.Contains("!=") || expression.Contains("+") || expression.Contains("*"))
                    {
                        reversed = true;
                        string reversedExpr = ReverseExpr(expression);
                        predicate = relatedExpressions.ContainsKey(expression) || relatedExpressions.ContainsKey(reversedExpr);
                    }
                    else
                    {
                        predicate = relatedExpressions.ContainsKey(expression);
                    }

                    // If expression already exists in the current scope.
                    if (predicate)
                    {
                        optimizedIR.Add($"{varName} = {relatedExpressions[reversed ? ReverseExpr(expression) : expression]}");
                    }
                    else
                    {
                        optimizedIR.Add(instruction);
                        relatedExpressions.Add(expression, varName);
                    }
                }
                // Function (new scope)
                else if (instruction.Contains("function"))
                {
                    optimizedIR.Add(instruction);
                    string funcName = instruction.Split(" ")[1];
                    currentScope = funcName;
                    expressions.Add(funcName, []);
                }
                // Label (new scope)
                else if (instruction.StartsWith("L") && instruction.EndsWith(":"))
                {
                    optimizedIR.Add(instruction);
                    currentScope = instruction;
                    expressions.Add(instruction, []);
                }
                else if (instruction.Trim() != "")
                {
                    optimizedIR.Add(instruction);
                }
            }

            IR = optimizedIR;
        }

        public string ReverseExpr(string expr)
        {
            return string.Join(' ', expr.Split(' ').Reverse());
        }

        public string GetIR()
        {
            return string.Join("\n", IR);
        }
    }
}
