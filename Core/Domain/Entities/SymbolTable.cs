using LLVMSharp.Interop;

namespace Core.Domain.Entities
{
    public class SymbolTable
    {
        // Scope name (global for global scope, function function_name for function scope, if statement number_of_it for if statements, else statement number_of_it for else statements, while statement number_of_it for while statements)
        public string Scope { get; set; } = string.Empty;
        // Key for name, value for type (variable / function)
        public Dictionary<string, LLVMValueRef> Names { get; set; } = [];
        public Dictionary<string, string> NamesTypes { get; set; } = [];
    }
}
