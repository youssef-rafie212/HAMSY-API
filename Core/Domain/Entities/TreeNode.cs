namespace Core.Domain.Entities
{
    public class TreeNode
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public List<TreeNode> Children { get; set; } = [];

        public TreeNode(string type = "")
        {
			Type = type;
			Value = string.Empty;
			Children = new List<TreeNode>();
		}
    }
    
}
