namespace Core.Domain.Entities
{
    public class SymbolTable
    {
        public string Scope { get; set; } = string.Empty;
        // key for variable name, value for variable value (if its a function the value will be null).
        public Dictionary<string, int?> Names { get; set; } = [];
    }
}
