using Core.Domain.Entities;
using Core.Enums;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public class SymbolTablesCreator
    {
        public List<SymbolTable> Tables = [];
        public List<string> Errors = [];
        private int _ifScopeCounter = 0;
        private int _elseScopeCounter = 0;
        private int _whileScopeCounter = 0;

        public SymbolTablesCreator()
        {
            // Initialzing the global symbol table as it always exists.
            Tables.Add(new SymbolTable()
            {
                Scope = "global",
                Names = [],
                Parent = null,
            });
        }

        // Creates a symbol table for each scope and handles errors as well.
        public void CreateTables(TreeNode node, SymbolTable? scope, TreeNode? prevSibling)
        {
            // Handle the global scope
            if (node.Type == TreeNodeType.Program.ToString())
            {
                // No need to create a new symbol table as it gets initialized in the constructor.
                SymbolTable globalScopeTable = Tables[0];

                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], globalScopeTable, i == 0 ? null : node.Children[i - 1]);
                }
            }

            // Create a function scope
            else if (node.Type == TreeNodeType.FunctionDefinition.ToString() || node.Type == TreeNodeType.MainFunction.ToString())
            {
                string functionName = node.Children[1].Value;
                // Add function name to the global scope (because we can only define functions in the global scopes).
                Tables[0].Names.Add(functionName);

                SymbolTable functionScopeTable = new()
                {
                    Names = [],
                    Scope = $"function {functionName}",
                    Parent = scope,
                };

                for (int i = 0; i < node.Children.Count; i++)
                {
                    if (i == 1) continue; // Skip adding the function name because we added it to the global scope already.
                    CreateTables(node.Children[i], functionScopeTable, i == 0 ? null : node.Children[i - 1]);
                }
                Tables.Add(functionScopeTable);
            }

            // Create if statement scope
            else if (node.Type == TreeNodeType.IfStatement.ToString())
            {
                _ifScopeCounter++;
                SymbolTable ifScopeTable = new()
                {
                    Names = [],
                    Scope = $"if statement {_ifScopeCounter}",
                    Parent = scope,
                };
                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], ifScopeTable, i == 0 ? null : node.Children[i - 1]);
                }
                Tables.Add(ifScopeTable);
            }

            // Create else statement scope
            else if (node.Type == TreeNodeType.ElseStatement.ToString())
            {
                _elseScopeCounter++;
                SymbolTable elseScopeTable = new()
                {
                    Names = [],
                    Scope = $"else statement {_elseScopeCounter}",
                    Parent = scope,
                };
                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], elseScopeTable, i == 0 ? null : node.Children[i - 1]);
                }
                Tables.Add(elseScopeTable);
            }

            // Create while statement scope
            else if (node.Type == TreeNodeType.WhileLoop.ToString())
            {
                _whileScopeCounter++;
                SymbolTable whileScopeTable = new()
                {
                    Names = [],
                    Scope = $"while loop {_whileScopeCounter}",
                    Parent = scope,
                };
                for (int i = 0; i < node.Children.Count; i++)
                {
                    CreateTables(node.Children[i], whileScopeTable, i == 0 ? null : node.Children[i - 1]);
                }
                Tables.Add(whileScopeTable);
            }

            // Handle names
            else if (node.Type == TreeNodeType.Terminal.ToString())
            {
                List<string> keywords = ["int", "return", "if", "else", "while"];
                Regex idRegex = new(@"^[a-zA-Z_][a-zA-Z0-9_]*$");

                // Identifier
                if (!keywords.Contains(node.Value) && idRegex.IsMatch(node.Value))
                {
                    // Declaration
                    if (prevSibling != null && prevSibling.Value == "int")
                    {
                        // Check if the name already exists in current scope.
                        if (scope!.Names.Contains(node.Value))
                        {
                            Errors.Add($"DECLARATION ERROR, name: '{node.Value}' already exists in the {scope.Scope} scope.");
                        }
                        else
                        {
                            scope.Names.Add(node.Value);
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

        private bool IsDeclared(string name, SymbolTable scope)
        {
            if (scope.Names.Contains(name)) return true;

            return IsDeclared(name, scope.Parent!);
        }
    }
}

