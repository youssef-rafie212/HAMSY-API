using Core.Domain.Entities;

namespace Core.DTO
{
    public class SymbolTableResponseDto
    {
        public List<SymbolTable> SymbolTables { get; set; } = [];
        public List<string> Notes { get; set; } = [];
        public List<string> Errors { get; set; } = [];
    }
}
