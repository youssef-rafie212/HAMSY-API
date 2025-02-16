using Antlr4.Runtime;
using Core.ANTLR.ErrorListeners;
using Core.Domain.Entities;
using Core.DTO;
using Core.Helpers;
using Core.ServiceContracts;

namespace Core.Services
{
    public class CompilationService : ICompilationService
    {
        public LexicalResponseDto LexicalAnalysis(LexicalRequestDto lexicalRequestDto)
        {
            AntlrInputStream antlrInput = new(lexicalRequestDto.SourceCode);
            HAMSYLexer lexer = new(antlrInput);

            // Removing the default error listeners and adding our custom one.
            LexicalErrorsListener lexicalErrorsListener = new();
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(lexicalErrorsListener);
            IList<IToken> antlrTokens = lexer.GetAllTokens();

            // Mapping ANTLR tokens to our tokens.
            List<Token> tokens = [];
            foreach (IToken t in antlrTokens)
            {
                string tokenType = HAMSYLexer.DefaultVocabulary.GetSymbolicName(t.Type);
                Token token = new()
                {
                    Type = tokenType,
                    Lexeme = t.Text,
                    Line = t.Line,
                    Column = t.Column,
                };
                tokens.Add(token);
            }

            return new()
            {
                Tokens = tokens,
                Errors = lexicalErrorsListener.Errors,
            };
        }

        public SyntaxResponseDto SyntaxAnalysis(SyntaxRequestDto syntaxRequestDto)
        {
            AntlrInputStream antlrInput = new(syntaxRequestDto.SourceCode);
            HAMSYLexer lexer = new(antlrInput);
            CommonTokenStream tokenStream = new(lexer);

            HAMSYParser parser = new(tokenStream);
            // Removing and adding our custom syntax errors listener.
            SyntaxErrorsListener syntaxErrorsListener = new();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(syntaxErrorsListener);

            // Start parsing
            HAMSYParser.ProgramContext cntx = parser.program();

            // Convert the ANTLR tree to our TreeNode structure.
            TreeNode tree = ParseTreeMapper.MapToParseTree(cntx);

            return new()
            {
                ParseTree = tree,
                Errors = syntaxErrorsListener.Errors,
            };
        }

        public SemanticResponseDto SemanticAnalysis(SemanticRequestDto semanticRequestDto)
        {
            var builder = new ASTBuilder();
            TreeNode ast = builder.ConvertToAST(semanticRequestDto.ParseTree);

            return new SemanticResponseDto
            {
                AST = ast,
            };
        }
        public SymbolTableResponseDto SymbolTables(SymbolTablesRequestDto symbolTablesRequestDto)
        {
            SymbolTablesCreator symbolTablesCreator = new();
            symbolTablesCreator.CreateTables(symbolTablesRequestDto.ParseTree, null, null);

            return new()
            {
                SymbolTables = symbolTablesCreator.Tables,
                Errors = symbolTablesCreator.Errors
            };
        }

        public IRGenResponseDto IRGeneration(IRGenRequestDto irGenRequestDto)
        {
            var response = new IRGenResponseDto();

            if (irGenRequestDto == null || irGenRequestDto.AST == null)
            {
                response.Errors.Add("Invalid request: AST is null.");
                return response;
            }

            TACGenerator tacGenerator = new TACGenerator();
            var tacInstructions = tacGenerator.GenerateTAC(irGenRequestDto.AST);
            response.IR = string.Join("\n", tacInstructions);
            return response;
        }

        public IROptResponseDto IROptimization(IROptRequestDto irOptRequestDto)
        {
            IROptimizer optimizer = new(irOptRequestDto.IR);

            optimizer.ApplyConstantFoldingPass();
            optimizer.ApplyConstantPropagationPass();
            optimizer.ApplyCommonSubexpressionEliminationPass();

            return new()
            {
                OptimizedIR = optimizer.GetIR()
            };
        }

        public InsSelResponseDto InstructionSelection(InsSelRequestDto insSelRequestDto)
        {
            throw new NotImplementedException();
        }

        public RegAllocResponseDto RegisterAllocation(RegAllocResponseDto regAllocResponseDto)
        {
            throw new NotImplementedException();
        }
        public InsSchedResponseDto InstructionScheduling(InsSchedRequestDto insSchedRequestDto)
        {
            throw new NotImplementedException();
        }

    }
}
