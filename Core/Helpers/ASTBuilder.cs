using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Core.Helpers
{
    public class ASTBuilder
    {
        public TreeNode ConvertToAST(TreeNode parseTreeNode)
        {
            switch (parseTreeNode.Type)
            {
                case "Program":
                    return new TreeNode("Program")
                    {
                        Children = parseTreeNode.Children.Select(ConvertToAST).ToList()
                    };
                case "VariableDeclaration":
                    return new TreeNode("VariableDeclaration")
                    {
                        Value = parseTreeNode.Children[1].Value,
                        Children = { ConvertToAST(parseTreeNode.Children[3]) }
                    };
                case "Assignment":
                    return new TreeNode("Assignment")
                    {
                        Value = parseTreeNode.Children[0].Value,
                        Children = { ConvertToAST(parseTreeNode.Children[2]) }
                    };

                case "FunctionDefinition":


                    var functionNode = new TreeNode("FunctionDefinition")
                    {
                        Value = parseTreeNode.Children[1].Value //idf
                    };

                    var paramsNode = new TreeNode("Parameters");
                    paramsNode.Children.Add(new TreeNode("Parameter")
                    {
                        Value = parseTreeNode.Children[4].Value
                    });
                    paramsNode.Children.Add(new TreeNode("Parameter")
                    {
                        Value = parseTreeNode.Children[7].Value
                    });
                    functionNode.Children.Add(paramsNode);

                    for (int i = 10; i < parseTreeNode.Children.Count - 1; i++)
                    {
                        var bodyNode = ConvertToAST(parseTreeNode.Children[i]);
                        if (bodyNode != null)
                        {
                            functionNode.Children.Add(bodyNode);
                        }
                    }

                    return functionNode;

                case "MainFunction":
                    var mainNode = new TreeNode("MainFunction");

                    for (int i = 5; i < parseTreeNode.Children.Count - 1; i++)
                    {
                        mainNode.Children.Add(ConvertToAST(parseTreeNode.Children[i]));
                    }
                    return mainNode;

                case "ReturnStatement":
                    return new TreeNode("ReturnStatement")
                    {
                        Children = { ConvertToAST(parseTreeNode.Children[1]) } // expression
                    };
                case "Expression":
                    if (parseTreeNode.Children.Count == 1) return ConvertToAST(parseTreeNode.Children[0]); // single operand
                    return new TreeNode("BinaryExpression")
                    {
                        Value = parseTreeNode.Children[1].Value, // operator
                        Children = {
                            ConvertToAST(parseTreeNode.Children[0]), // L_operand
							ConvertToAST(parseTreeNode.Children[2])  // R_operand
						}
                    };

                case "Condition":
                    return new TreeNode("Condition")
                    {
                        Value = parseTreeNode.Children[1].Value, // operator
                        Children =
                        {
                            ConvertToAST(parseTreeNode.Children[0]),
                            ConvertToAST(parseTreeNode.Children[2])
                        }
                    };

                case "IfStatement":
                    var ifNode = new TreeNode("IfStatement");

                    ifNode.Children.Add(ConvertToAST(parseTreeNode.Children[2]));

                    var thenBlockNode = new TreeNode("ThenBlock");
                    for (int i = 5; i < parseTreeNode.Children.Count; i++)
                    {
                        if (parseTreeNode.Children[i].Type == "ElseStatement")
                            break;
                        else if (parseTreeNode.Children[i].Type == "Statement")
                            thenBlockNode.Children.Add(ConvertToAST(parseTreeNode.Children[i]));
                    }
                    ifNode.Children.Add(thenBlockNode);

                    for (int i = 6; i < parseTreeNode.Children.Count; i++)
                    {
                        if (parseTreeNode.Children[i].Type == "ElseStatement")
                        {
                            ifNode.Children.Add(ConvertToAST(parseTreeNode.Children[i]));
                            break;
                        }
                    }
                    return ifNode;

                case "ElseStatement":
                    var elseNode = new TreeNode("ElseStatement");

                    var elseBlockNode = new TreeNode("ElseBlock");
                    for (int i = 2; i < parseTreeNode.Children.Count; i++)// statement*
                    {
                        if (parseTreeNode.Children[i].Type == "Statement")
                            elseBlockNode.Children.Add(ConvertToAST(parseTreeNode.Children[i]));
                    }
                    elseNode.Children.Add(elseBlockNode);

                    return elseNode;


                case "WhileLoop":
                    var whileNode = new TreeNode("WhileLoop");
                    whileNode.Children.Add(ConvertToAST(parseTreeNode.Children[2]));
                    for (int i = 5; i < parseTreeNode.Children.Count; i++)
                    {
                        if (parseTreeNode.Children[i].Type == "Statement")
                            whileNode.Children.Add(ConvertToAST(parseTreeNode.Children[i]));
                    }

                    return whileNode;

                case "Statement":
                    if (parseTreeNode.Children.Count > 0)
                    {
                        return ConvertToAST(parseTreeNode.Children[0]);
                    }
                    return null;

                case "Operand":
                    return new TreeNode("Operand") { Value = parseTreeNode.Value };

                case "Operator":
                    return new TreeNode("Operator") { Value = parseTreeNode.Value };


                default:
                    return new TreeNode(parseTreeNode.Type) { Value = parseTreeNode.Value };
            }

        }


    }
}
