using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Core.Domain.Entities;
using Core.Enums;

namespace Core.Helpers
{
    public static class ParseTreeMapper
    {
        // Maps antlr tree to our TreeNode structure.
        public static TreeNode MapToParseTree(IParseTree antlrTree)
        {
            // Hnadles the case where the node is a non termnal.
            if (antlrTree is ParserRuleContext ctx)
            {
                TreeNode createdNode = new()
                {
                    Type = MapNodeType(ctx).ToString(),
                    Value = ctx.GetText(),
                    Children = []
                };

                if (ctx.ChildCount > 0)
                {
                    foreach (IParseTree n in ctx.children)
                    {
                        createdNode.Children.Add(MapToParseTree(n));
                    }
                }

                return createdNode;
            }
            // Handles the case where the node is a terminal.
            else
            {
                return new()
                {
                    Type = TreeNodeType.Terminal.ToString(),
                    Value = antlrTree.GetText(),
                    Children = []
                };
            }
        }

        // Maps ANTLR rule type to our tree node type.
        private static TreeNodeType MapNodeType(ParserRuleContext ctx)
        {
            switch (ctx.RuleIndex)
            {
                case HAMSYParser.RULE_program: return TreeNodeType.Program;
                case HAMSYParser.RULE_functionDefinition: return TreeNodeType.FunctionDefinition;
                case HAMSYParser.RULE_mainFunction: return TreeNodeType.MainFunction;
                case HAMSYParser.RULE_returnStatement: return TreeNodeType.ReturnStatement;
                case HAMSYParser.RULE_statement: return TreeNodeType.Statement;
                case HAMSYParser.RULE_variableDeclaration: return TreeNodeType.VariableDeclaration;
                case HAMSYParser.RULE_assignment: return TreeNodeType.Assignment;
                case HAMSYParser.RULE_whileLoop: return TreeNodeType.WhileLoop;
                case HAMSYParser.RULE_ifStatement: return TreeNodeType.IfStatement;
                case HAMSYParser.RULE_elseStatement: return TreeNodeType.ElseStatement;
                case HAMSYParser.RULE_expression: return TreeNodeType.Expression;
                case HAMSYParser.RULE_functionCall: return TreeNodeType.FunctionCall;
                case HAMSYParser.RULE_operand: return TreeNodeType.Operand;
                case HAMSYParser.RULE_operator: return TreeNodeType.Operator;
                case HAMSYParser.RULE_condition: return TreeNodeType.Condition;
                case HAMSYParser.RULE_comparisonOperator: return TreeNodeType.ComparisonOperator;

                default: return TreeNodeType.None;
            }
        }
    }
}
