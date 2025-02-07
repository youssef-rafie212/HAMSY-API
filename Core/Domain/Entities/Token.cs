namespace Core.Domain.Entities
{
    public class Token
    {
        public string Type { set; get; } = string.Empty;
        public string Lexeme { set; get; } = string.Empty;
        public int Line { set; get; }
        public int Column { set; get; }
    }
}
