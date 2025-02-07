namespace Core.Domain.Entities
{
    public class TreeNode
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public List<TreeNode> Children { get; set; } = [];
    }
}
