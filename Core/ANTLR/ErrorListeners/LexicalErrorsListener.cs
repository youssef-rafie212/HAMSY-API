using Antlr4.Runtime;

namespace Core.ANTLR.ErrorListeners
{
    public class LexicalErrorsListener : IAntlrErrorListener<int>
    {
        public List<string> Errors { get; set; } = [];
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add($"LEXICAL ERROR at line {line}, column {charPositionInLine}, message: {msg}");
        }
    }
}
