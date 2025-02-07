namespace Core.Domain.Entities
{
    public class SymbolTable
    {
        public string Scope { get; set; } = string.Empty;
        public List<string> Names { get; set; } = [];
    }
}
