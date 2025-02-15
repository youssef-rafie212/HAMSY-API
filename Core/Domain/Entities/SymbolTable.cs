using LLVMSharp.Interop;

namespace Core.Domain.Entities
{
    public class SymbolTable
    {
        // Scope name (global for global scope, function function_name for function scope
        public string Scope { get; set; } = string.Empty;
        // Key for name, value for type (variable / function)
        public Dictionary<string, LLVMValueRef> Names { get; set; } = [];
        public Dictionary<string, string> NamesTypes { get; set; } = [];
        public SymbolTable? Parent { get; set; }
    }
}
