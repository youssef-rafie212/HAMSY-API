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
        public InsSchedResponseDto InstructionScheduling(InsSchedRequestDto insSchedRequestDto)
        {
            throw new NotImplementedException();
        }

        public InsSelResponseDto InstructionSelection(InsSelRequestDto insSelRequestDto)
        {
            throw new NotImplementedException();
        }

        public IRGenResponseDto IRGeneration(IRGenRequestDto irGenRequestDto)
        {
            throw new NotImplementedException();
        }

        public IROptResponseDto IROptimization(IROptRequestDto irOptRequestDto)
        {
            throw new NotImplementedException();
        }

        public LexicalResponseDto LexicalAnalysis(LexicalRequestDto lexicalRequestDto)
        {
            AntlrInputStream antlrInput = new(lexicalRequestDto.SourceCode);
            HAMSYLexer lexer = new(antlrInput);

            // Removing the default error listeners and adding our custom one.
            LexicalErrorsListener lexicalErrorsListener = new();
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(lexicalErrorsListener);
            IList<IToken> antlrTokens = lexer.GetAllTokens();

            // Keeping track of how many tokens of each type we got to add to the notes field.
            Dictionary<string, int> tokenCounts = [];

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

                if (tokenCounts.ContainsKey(tokenType))
                {
                    tokenCounts[tokenType]++;
                }
                else
                {
                    tokenCounts.Add(tokenType, 1);
                }
            }

            List<string> notes = [];
            foreach (var entry in tokenCounts)
            {
                notes.Add($"Extracted {entry.Value} '{entry.Key}' token(s).");
            }

            return new()
            {
                Tokens = tokens,
                Notes = notes,
                Errors = lexicalErrorsListener.Errors,
            };
        }

        public RegAllocResponseDto RegisterAllocation(RegAllocResponseDto regAllocResponseDto)
        {
            throw new NotImplementedException();
        }

        public SemanticResponseDto SemanticAnalysis(SemanticRequestDto semanticRequestDto)
        {
            throw new NotImplementedException();
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
            TreeNode tree = ParseTreeMaper.MapToParseTree(cntx);

            return new()
            {
                ParseTree = tree,
                SymbolTables = [],
                Notes = [],
                Errors = syntaxErrorsListener.Errors,
            };
        }
    }
}
