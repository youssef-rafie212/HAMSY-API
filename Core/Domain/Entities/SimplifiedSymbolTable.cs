namespace Core.Domain.Entities
{
    // A simplified version of SymbolTable to be sent to the front-end.
    public class SimplifiedSymbolTable
    {
        public string Scope { get; set; } = string.Empty;
        public List<string> Names { get; set; } = [];
    }
}
