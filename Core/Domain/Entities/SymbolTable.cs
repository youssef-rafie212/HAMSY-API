namespace Core.Domain.Entities
{
    public class SymbolTable
    {
        // Scope name (global for global scope, function function_name for function scope, if statement number_of_it for if statements, else statement number_of_it for else statements, while statement number_of_it for while statements)
        public string Scope { get; set; } = string.Empty;
        // Key for name, value for type (variable / function)
        public Dictionary<string, string> Names { get; set; } = [];
        // The parent scope (symbol table) that created the current symbol table.
        public SymbolTable? Parent { get; set; }
    }
}
