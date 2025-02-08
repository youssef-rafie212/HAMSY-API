namespace Core.Domain.Entities
{
    public class SymbolTable
    {
        // Scope name (global for global scope, function function_name for function scope, if statement number_of_it for if statements, while statement number_of_it for while statements)
        public string Scope { get; set; } = string.Empty;
        public List<string> Names { get; set; } = [];
        public SymbolTable? Parent { get; set; }
    }
}
