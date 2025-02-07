using Antlr4.Runtime;

namespace Core.ANTLR.ErrorListeners
{
    public class SyntaxErrorsListener : IAntlrErrorListener<IToken>
    {
        public List<string> Errors { get; set; } = [];
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add($"SYNTAX ERROR at line {line}, column {charPositionInLine}, message: {msg}");
        }
    }
}
