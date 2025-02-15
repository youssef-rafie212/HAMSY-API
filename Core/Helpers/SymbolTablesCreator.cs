using Core.Domain.Entities;
using Core.Enums;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public class SymbolTablesCreator
    {
        public List<SymbolTable> Tables = [];
        public List<string> Errors = [];
        private int IfCounter = 0;
        private int ElseCounter = 0;
        private int WhileCounter = 0;

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
        public void CreateTables(TreeNode node, SymbolTable? scope, TreeNode? prevSibling)
        {
            // Handle the global scope
            if (node.Type == TreeNodeType.Program.ToString())
            {

                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], Tables[0], i == 0 ? null : node.Children[i - 1]);
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
                    Parent = scope,
                };
                Tables.Add(functionScopeTable);

                for (int i = 2; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], functionScopeTable, i == 0 ? null : node.Children[i - 1]);
                }

            }

            else if (node.Type == TreeNodeType.IfStatement.ToString())
            {
                IfCounter++;

                SymbolTable ifScopeTable = new()
                {
                    Names = [],
                    Scope = $"if block {IfCounter} in function {scope!.Scope.Split(' ').Last()}",
                    Parent = scope,
                };
                Tables.Add(ifScopeTable);

                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], ifScopeTable, i == 0 ? null : node.Children[i - 1]);
                }

            }

            else if (node.Type == TreeNodeType.ElseStatement.ToString())
            {
                ElseCounter++;

                SymbolTable elseScopeTable = new()
                {
                    Names = [],
                    Scope = $"else block {ElseCounter} in function {scope!.Scope.Split(' ').Last()}",
                    Parent = scope,
                };
                Tables.Add(elseScopeTable);

                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], elseScopeTable, i == 0 ? null : node.Children[i - 1]);
                }

            }

            else if (node.Type == TreeNodeType.WhileLoop.ToString())
            {
                WhileCounter++;

                SymbolTable whileScopeTable = new()
                {
                    Names = [],
                    Scope = $"while block {WhileCounter} in function {scope!.Scope.Split(' ').Last()}",
                    Parent = scope,
                };
                Tables.Add(whileScopeTable);

                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], whileScopeTable, i == 0 ? null : node.Children[i - 1]);
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
                    List<SymbolTable> tablesToAddTo = Tables.FindAll(t =>
                    {
                        string funcNameFromTableScope = t.Scope.Split(' ').Last();
                        return funcNameFromTableScope == scope!.Scope.Split(' ').Last();
                    });

                    // Declaration
                    if (prevSibling != null && prevSibling.Value == "int")
                    {
                        bool isValid = true;
                        foreach (SymbolTable table in tablesToAddTo)
                        {
                            // Check if the name already exists in current scope.
                            if (table.Names.ContainsKey(node.Value))
                            {
                                Errors.Add($"DECLARATION ERROR, name: '{node.Value}' already exists in the {table.Scope.Split(' ').Last()} scope.");
                                isValid = false;
                            }
                        }

                        if (isValid)
                        {
                            scope!.Names.Add(node.Value, null);
                            scope.NamesTypes.Add(node.Value, "variable");
                        }
                    }
                    // Usage
                    else
                    {
                        if (!IsDeclared(node.Value, scope!))
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
                    CreateTables(node.Children[i], scope, i == 0 ? null : node.Children[i - 1]);
                }
            }
        }

        private bool IsDeclared(string name, SymbolTable? scope)
        {
            if (scope == null) return false;
            else if (scope.Names.ContainsKey(name)) return true;
            return IsDeclared(name, scope.Parent);
        }
    }
}

