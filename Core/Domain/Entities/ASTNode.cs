using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class ASTNode
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public List<ASTNode> Children { get; set; } = [];

		public ASTNode(string type = "")
		{
			Type = type;
			Value = string.Empty;
			Children = new List<ASTNode>();
		}

    }
}
