using System.Text.RegularExpressions;

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

        public void ApplyConstantFoldingPass()
        {
            List<string> optimizedIR = [];

            foreach (string i in IR)
            {
                string instruction = i.Trim();

                int op1 = 0; int op2 = 0;

                bool predicate =
                    (instruction.Contains("+")
                    || instruction.Contains("-")
                    || instruction.Contains("*")
                    || instruction.Contains("/")
                    || instruction.Contains("%")
                    )
                    &&
                    (
                       (int.TryParse(instruction.Split('=')[1].Trim().Split(' ')[0], out op1))
                       &&
                       (int.TryParse(instruction.Split('=')[1].Trim().Split(' ')[2], out op2))
                    );

                // Check if mathematical expression with 2 numeric constants
                if (predicate)
                {
                    string varName = instruction.Split("=")[0].Trim();
                    string operation = instruction.Split("=")[1].Trim().Split(' ')[1];
                    int result = CalculateExpression(operation, op1, op2);
                    optimizedIR.Add($"{varName} = {result}");
                }
                else
                {
                    optimizedIR.Add(instruction);
                }
            }

            IR = optimizedIR;
        }

        public void ApplyConstantPropagationPass()
        {
            List<string> optimizedIR = [];

            //         scope          varName    value
            Dictionary<string, Dictionary<string, int>> namesWithConstant = [];
            Dictionary<string, List<string>> namesWithScope = [];
            namesWithConstant.Add("global", []);
            namesWithScope.Add("global", []);

            string currentScope = "global";

            foreach (string i in IR)
            {
                string instruction = i.Trim();
                int constValue = 0;

                // Assignment
                if (instruction.Contains("="))
                {
                    string varName = instruction.Split("=")[0].Trim();
                    namesWithScope[currentScope].Add(varName);

                    Dictionary<string, int> relatedNames = namesWithConstant[currentScope];

                    // Constant number assignment
                    if (instruction.Split("=")[1].Trim().Split(" ").Length == 1 &&
                        int.TryParse(instruction.Split("=")[1].Trim(), out constValue))
                    {
                        // If variable already exists in the current scope update its value.
                        if (relatedNames.ContainsKey(varName))
                        {
                            relatedNames[varName] = constValue;
                        }
                        // Otherwise add it.
                        else
                        {
                            relatedNames.Add(varName, constValue);
                        }
                    }
                    // check for usage of a constant variable
                    else
                    {
                        string newInst = instruction;
                        string expression = instruction.Split("=")[1].Trim();
                        foreach (string token in expression.Split(' '))
                        {
                            // Replace name with its constant value from current scope.
                            if (relatedNames.ContainsKey(token))
                            {
                                newInst = Regex.Replace(newInst, token, relatedNames[token].ToString());
                            }
                            // Replace name with its constant value from global scope (only if the name doesnt exist in the current scope).
                            else if (!namesWithScope[currentScope].Contains(token) && namesWithConstant["global"].ContainsKey(token))
                            {
                                newInst = Regex.Replace(newInst, token, namesWithConstant["global"][token].ToString());
                            }
                        }
                        optimizedIR.Add(newInst);
                    }
                }
                // Function (new scope)
                else if (instruction.Contains("function"))
                {
                    optimizedIR.Add(instruction);
                    string funcName = instruction.Split(" ")[1];
                    currentScope = funcName;
                    namesWithConstant.Add(funcName, []);
                    namesWithScope.Add(funcName, []);
                }
                else if (instruction.Trim() != "")
                {
                    optimizedIR.Add(instruction);
                }
            }

            IR = optimizedIR;
        }

        private int CalculateExpression(string operation, int op1, int op2)
        {
            // Still think that switch is shit (starting to like it actually)
            switch (operation)
            {
                case "+": return op1 + op2;
                case "-": return op1 - op2;
                case "*": return op1 * op2;
                case "/": return op1 / op2;
                case "%": return op1 % op2;
                default: return 0;
            }
        }


        private string ReverseExpr(string expr)
        {
            return string.Join(' ', expr.Split(' ').Reverse());
        }

        public string GetIR()
        {
            return string.Join("\n", IR);
        }
    }
}
