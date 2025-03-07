namespace Core.Domain.Entities
{
    public class SymbolTable
    {
        public string Scope { get; set; } = string.Empty;
        // Key for name, value for address
        public Dictionary<string, string> Names { get; set; } = [];
        // Key for name, value for type (variable / function)
        public Dictionary<string, string> NamesTypes { get; set; } = [];
        public SymbolTable? Parent { get; set; }
    }
}
