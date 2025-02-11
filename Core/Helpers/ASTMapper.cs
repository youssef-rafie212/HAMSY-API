using Core.Domain.Entities;
using Core.Enums;

namespace Core.Helpers
{
    public static class ASTMapper
    {
        public static TreeNode MapFromParseTree(TreeNode parseTree)
        {
            if (parseTree.Type == TreeNodeType.Program.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                for (int i = 0; i < parseTree.Children.Count - 1; i++)
                {
                    newNode.Children.Add(MapFromParseTree(parseTree.Children[i]));
                }
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.VariableDeclaration.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                newNode.Children.Add(MapFromParseTree(parseTree.Children[1]));
                newNode.Children.Add(MapFromParseTree(parseTree.Children[3]));
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.Assignment.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                newNode.Children.Add(MapFromParseTree(parseTree.Children[0]));
                newNode.Children.Add(MapFromParseTree(parseTree.Children[2]));
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.FunctionDefinition.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                newNode.Children.Add(MapFromParseTree(parseTree.Children[1]));
                newNode.Children.Add(MapFromParseTree(parseTree.Children[4]));
                newNode.Children.Add(MapFromParseTree(parseTree.Children[7]));
                for (int i = 10; i < parseTree.Children.Count - 1; i++)
                {
                    newNode.Children.Add(MapFromParseTree(parseTree.Children[i]));
                }
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.MainFunction.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                newNode.Children.Add(MapFromParseTree(parseTree.Children[1]));
                for (int i = 5; i < parseTree.Children.Count - 1; i++)
                {
                    newNode.Children.Add(MapFromParseTree(parseTree.Children[i]));
                }
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.ReturnStatement.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                newNode.Children.Add(MapFromParseTree(parseTree.Children[1]));
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.WhileLoop.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                newNode.Children.Add(MapFromParseTree(parseTree.Children[2]));
                for (int i = 5; i < parseTree.Children.Count; i++)
                {
                    newNode.Children.Add(MapFromParseTree(parseTree.Children[i]));
                }
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.IfStatement.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                newNode.Children.Add(MapFromParseTree(parseTree.Children[2]));
                int last = 0;
                for (int i = 5; i < parseTree.Children.Count; i++)
                {
                    if (parseTree.Children[i].Type == TreeNodeType.Statement.ToString())
                    {
                        newNode.Children.Add(MapFromParseTree(parseTree.Children[i]));
                    }
                    else
                    {
                        last = i;
                        break;
                    }
                }
                if (parseTree.Children.Count > last + 1)
                {
                    newNode.Children.Add(MapFromParseTree(parseTree.Children[last + 1]));
                }
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.ElseStatement.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                newNode.Children.Add(MapFromParseTree(parseTree.Children[2]));
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.FunctionCall.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                newNode.Children.Add(MapFromParseTree(parseTree.Children[0]));
                newNode.Children.Add(MapFromParseTree(parseTree.Children[2]));
                newNode.Children.Add(MapFromParseTree(parseTree.Children[4]));
                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.Expression.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };

                if (parseTree.Children.Count == 1)
                {
                    newNode.Children.Add(MapFromParseTree(parseTree.Children[0]));
                }
                else
                {
                    newNode.Children.Add(MapFromParseTree(parseTree.Children[0]));
                    newNode.Children.Add(MapFromParseTree(parseTree.Children[1]));
                    newNode.Children.Add(MapFromParseTree(parseTree.Children[2]));
                }

                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.Condition.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };

                newNode.Children.Add(MapFromParseTree(parseTree.Children[0]));
                newNode.Children.Add(MapFromParseTree(parseTree.Children[1]));
                newNode.Children.Add(MapFromParseTree(parseTree.Children[2]));

                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.ReturnStatement.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };

                newNode.Children.Add(MapFromParseTree(parseTree.Children[1]));

                return newNode;
            }
            else if (parseTree.Type == TreeNodeType.Terminal.ToString())
            {
                TreeNode newNode = new() { Type = parseTree.Type, Value = parseTree.Value };
                return newNode;
            }

            return MapFromParseTree(parseTree.Children[0]);
        }
    }
}