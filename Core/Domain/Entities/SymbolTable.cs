using Core.Enums;

namespace Core.Domain.Entities
{
    public class SymbolTable
    {
        public string Scope { get; set; } = string.Empty;
        public Dictionary<string, SymbolTableNameType> Names { get; set; } = [];
    }
}
