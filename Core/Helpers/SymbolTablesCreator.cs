using Core.Domain.Entities;
using Core.Enums;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public class SymbolTablesCreator
    {
        public List<SymbolTable> Tables = [];
        public List<string> Errors = [];
        public string CurrentScope = "global";

        public SymbolTablesCreator()
        {
            // Initialzing the global symbol table as it always exists.
            Tables.Add(new SymbolTable()
            {
                Scope = "global",
                Names = [],
            });
        }

        // Creates a symbol table for each scope and handles errors as well.
        public void CreateTables(TreeNode node, TreeNode? prevSibling)
        {
            // Handle the global scope
            if (node.Type == TreeNodeType.Program.ToString())
            {

                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], i == 0 ? null : node.Children[i - 1]);
                }
            }

            // Create a function scope
            else if (node.Type == TreeNodeType.FunctionDefinition.ToString() || node.Type == TreeNodeType.MainFunction.ToString())
            {
                string functionName = node.Children[1].Value;
                // Add function name to the global scope (because we can only define functions in the global scopes).
                Tables[0].Names.Add(functionName, null);
                Tables[0].NamesTypes.Add(functionName, "function");

                SymbolTable functionScopeTable = new()
                {
                    Names = [],
                    Scope = $"{functionName}",
                };
                Tables.Add(functionScopeTable);

                CurrentScope = functionName;
                for (int i = 2; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], i == 0 ? null : node.Children[i - 1]);
                }

            }
            // Handle names
            else if (node.Type == TreeNodeType.Terminal.ToString())
            {
                List<string> keywords = ["int", "return", "if", "else", "while"];
                Regex idRegex = new(@"^[a-zA-Z_][a-zA-Z0-9_]*$");

                // Identifier
                if (!keywords.Contains(node.Value) && idRegex.IsMatch(node.Value))
                {
                    SymbolTable tableToAddTo = Tables.Find(t =>
                    {
                        string funcNameFromTableScope = t.Scope.Split(' ').Last();
                        return funcNameFromTableScope == CurrentScope;
                    })!;
                    // Declaration
                    if (prevSibling != null && prevSibling.Value == "int")
                    {
                        // Check if the name already exists in current scope.
                        if (tableToAddTo!.Names.ContainsKey(node.Value))
                        {
                            Errors.Add($"DECLARATION ERROR, name: '{node.Value}' already exists in the {CurrentScope} scope.");
                        }
                        else
                        {
                            tableToAddTo.Names.Add(node.Value, null);
                            tableToAddTo.NamesTypes.Add(node.Value, "variable");
                        }
                    }
                    // Usage
                    else
                    {
                        if (!IsDeclared(node.Value, tableToAddTo!))
                        {
                            Errors.Add($"VARIABLE USAGE ERROR, the name '{node.Value}' is used but not declared in usage scope or its parent scopes.");
                        }
                    }
                }
            }
            // If not a scope maker or an identifier we just skip to the children of the node.
            else
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], i == 0 ? null : node.Children[i - 1]);
                }
            }
        }

        private bool IsDeclared(string name, SymbolTable scope)
        {
            if (scope.Names.ContainsKey(name)) return true;
            else
            {
                if (scope.Scope == "global") return false;
                return IsDeclared(name, Tables[0]);
            }

        }
    }
}

