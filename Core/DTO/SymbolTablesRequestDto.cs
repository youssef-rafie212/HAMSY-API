using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class SymbolTablesRequestDto
    {
        [Required]
        public string SourceCode { get; set; } = string.Empty;
    }
}
