//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from HAMSY.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IHAMSYListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class HAMSYBaseListener : IHAMSYListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProgram([NotNull] HAMSYParser.ProgramContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProgram([NotNull] HAMSYParser.ProgramContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.functionDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionDefinition([NotNull] HAMSYParser.FunctionDefinitionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.functionDefinition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionDefinition([NotNull] HAMSYParser.FunctionDefinitionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.mainFunction"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMainFunction([NotNull] HAMSYParser.MainFunctionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.mainFunction"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMainFunction([NotNull] HAMSYParser.MainFunctionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.returnStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterReturnStatement([NotNull] HAMSYParser.ReturnStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.returnStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitReturnStatement([NotNull] HAMSYParser.ReturnStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] HAMSYParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] HAMSYParser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.variableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableDeclaration([NotNull] HAMSYParser.VariableDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.variableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableDeclaration([NotNull] HAMSYParser.VariableDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.assignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssignment([NotNull] HAMSYParser.AssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.assignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssignment([NotNull] HAMSYParser.AssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.whileLoop"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterWhileLoop([NotNull] HAMSYParser.WhileLoopContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.whileLoop"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitWhileLoop([NotNull] HAMSYParser.WhileLoopContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfStatement([NotNull] HAMSYParser.IfStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfStatement([NotNull] HAMSYParser.IfStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpression([NotNull] HAMSYParser.ExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpression([NotNull] HAMSYParser.ExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.functionCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionCall([NotNull] HAMSYParser.FunctionCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.functionCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionCall([NotNull] HAMSYParser.FunctionCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.operand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperand([NotNull] HAMSYParser.OperandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.operand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperand([NotNull] HAMSYParser.OperandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.operator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperator([NotNull] HAMSYParser.OperatorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.operator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperator([NotNull] HAMSYParser.OperatorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.condition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCondition([NotNull] HAMSYParser.ConditionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.condition"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCondition([NotNull] HAMSYParser.ConditionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="HAMSYParser.comparisonOperator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComparisonOperator([NotNull] HAMSYParser.ComparisonOperatorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HAMSYParser.comparisonOperator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComparisonOperator([NotNull] HAMSYParser.ComparisonOperatorContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
