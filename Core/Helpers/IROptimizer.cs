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

                    // if the expression is a single operand do nothing
                    if (expression.Split(' ').Length == 1)
                    {
                        optimizedIR.Add(instruction);
                        return;
                    }

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
                        if (reversed)
                        {
                            if (relatedExpressions.ContainsKey(expression))
                            {
                                optimizedIR.Add($"{varName} = {relatedExpressions[expression]}");
                            }
                            else
                            {
                                optimizedIR.Add($"{varName} = {relatedExpressions[ReverseExpr(expression)]}");
                            }
                        }
                        else
                        {
                            optimizedIR.Add($"{varName} = {relatedExpressions[expression]}");
                        }

                    }
                    else
                    {
                        optimizedIR.Add(instruction);
                        RemoveEntryByValue(relatedExpressions, varName);
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
                else if (instruction != "")
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

                bool predicate = false;
                char sep = ' ';

                if (instruction.Contains("=")) sep = '=';

                if (sep == '=')
                {
                    predicate = (instruction.Contains("+")
                    || instruction.Contains("-")
                    || instruction.Contains("*")
                    || instruction.Contains("/")
                    || instruction.Contains("%")
                    )
                    &&
                    (
                       (int.TryParse(instruction.Split(sep)[1].Trim().Split(' ')[0], out op1))
                       &&
                       (int.TryParse(instruction.Split(sep)[1].Trim().Split(' ')[2], out op2))
                    );
                }
                else
                {
                    predicate = (instruction.Contains("+")
                    || instruction.Contains("-")
                    || instruction.Contains("*")
                    || instruction.Contains("/")
                    || instruction.Contains("%")
                    )
                    &&
                    (
                       (int.TryParse(instruction.Split(sep).Skip(1).ToList()[0], out op1))
                       &&
                       (int.TryParse(instruction.Split(sep).Skip(1).ToList()[2], out op2))
                    );
                }

                if (predicate)
                {
                    if (sep == '=')
                    {
                        string varName = instruction.Split("=")[0].Trim();
                        string operation = instruction.Split("=")[1].Trim().Split(' ')[1];
                        int result = CalculateExpression(operation, op1, op2);
                        optimizedIR.Add($"{varName} = {result}");
                    }
                    else
                    {
                        string operation = instruction.Split(sep).Skip(1).ToList()[1];
                        int result = CalculateExpression(operation, op1, op2);
                        string key = instruction.Contains("param") ? "param" : "return";
                        optimizedIR.Add($"{key} {result}");
                    }
                }
                else if (instruction != "")
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
            namesWithConstant.Add("global", []);

            string currentScope = "global";

            foreach (string i in IR)
            {
                string instruction = i.Trim();
                int constValue = 0;
                Dictionary<string, int> relatedNames = namesWithConstant[currentScope];

                if (instruction.Contains("="))
                {
                    string varName = instruction.Split("=")[0].Trim();

                    if (relatedNames.ContainsKey(varName) && !int.TryParse(instruction.Split("=")[1].Trim(), out int temp))
                    {
                        relatedNames.Remove(varName);
                    }

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
                        optimizedIR.Add(instruction);
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
                        }
                        optimizedIR.Add(newInst);
                    }
                }
                else if (instruction.Contains("param"))
                {
                    string newInst = instruction;
                    string paramValue = instruction.Split(" ")[1].Trim();
                    // Replace name with its constant value from current scope.
                    if (relatedNames.ContainsKey(paramValue))
                    {
                        newInst = Regex.Replace(newInst, paramValue, relatedNames[paramValue].ToString());
                    }
                    optimizedIR.Add(newInst);
                }
                else if (instruction.Contains("return"))
                {
                    string newInst = instruction;
                    string expression = instruction.Split(" ")[1].Trim();
                    foreach (string token in expression.Split(' '))
                    {
                        // Replace name with its constant value from current scope.
                        if (relatedNames.ContainsKey(token))
                        {
                            newInst = Regex.Replace(newInst, token, relatedNames[token].ToString());
                        }
                    }
                    optimizedIR.Add(newInst);
                }
                // Function (new scope)
                else if (instruction.Contains("function"))
                {
                    optimizedIR.Add(instruction);
                    string funcName = instruction.Split(" ")[1];
                    currentScope = funcName;
                    namesWithConstant.Add(funcName, []);
                }
                else if (instruction.StartsWith("L") && instruction.EndsWith(":"))
                {
                    optimizedIR.Add(instruction);
                    currentScope = instruction;
                    namesWithConstant.Add(instruction, []);
                }
                else if (instruction != "")
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

        private void RemoveEntryByValue(Dictionary<string, string> dict, string value)
        {
            List<string> keysToRemove = new List<string>();

            // Find keys with the target value
            foreach (var kvp in dict)
            {
                if (kvp.Value == value)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            // Remove the keys from the dictionary
            foreach (var key in keysToRemove)
            {
                dict.Remove(key);
            }
        }

        public string GetIR()
        {
            return string.Join("\n", IR);
        }
    }
}
