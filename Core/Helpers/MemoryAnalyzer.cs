using Core.Domain.Entities;
using Core.Enums;

namespace Core.Helpers
{
    public class MemoryAnalyzer
    {
        public ExecutionSimulator ExecutionSimulator { get; set; }
        public List<SymbolTable> SymbolTables { get; set; }
        public TreeNode ParseTree { get; set; }
        public List<string> UpdatedDataSegment { get; set; } = [];
        public List<StackFrame> UpdatedStack { get; set; } = [];

        public MemoryAnalyzer(List<SymbolTable> symbolTables, TreeNode parseTree)
        {
            SymbolTables = symbolTables;
            ParseTree = parseTree;
            ExecutionSimulator = new();
        }

        public void Analyze()
        {
            GlobalVariables();
            ExecuteFunction("main");
        }

        private void GlobalVariables()
        {
            SymbolTable globalScope = SymbolTables.Find(s => s.Scope == "global")!;

            if (globalScope.Names.Count > 0)
            {
                foreach (var entry in globalScope.Names)
                {
                    if (entry.Value == "variable")
                    {
                        UpdatedDataSegment.Add(entry.Key);
                    }
                }

                MemoryData memoryData = new() { DataSegment = UpdatedDataSegment, Stack = UpdatedStack };

                ExecutionSimulator.AddStep("Global variables declaration", memoryData);
            }
        }

        private void ExecuteFunction(string functionName)
        {
            AddFunctionCall(functionName);

            List<string> innerFunctionCalls = GetFunctionCalls(functionName);

            if (innerFunctionCalls.Count > 0)
            {
                foreach (string call in innerFunctionCalls)
                {
                    if (call != functionName)
                    {
                        ExecuteFunction(call);
                    }
                }
            }

            RemoveFunctionCall(functionName);
        }

        private List<string> GetFunctionCalls(string functionName)
        {
            TreeNode node = TraverseTo(functionName, ParseTree)!;
            return SearchForCall(node, []);
        }

        private TreeNode? TraverseTo(string functionName, TreeNode startNode)
        {
            if ((startNode.Type == TreeNodeType.FunctionDefinition.ToString()
            || startNode.Type == TreeNodeType.MainFunction.ToString())
            && (startNode.Children[1].Value == functionName))
            {
                return startNode;
            }

            foreach (TreeNode child in startNode.Children)
            {
                TreeNode? res = TraverseTo(functionName, child);
                if (res != null) return res;
                continue;
            }

            return null;
        }

        private List<string> SearchForCall(TreeNode node, List<string> callsFound)
        {
            if (node.Type == TreeNodeType.FunctionCall.ToString())
            {
                callsFound.Add(node.Children[0].Value);
                return callsFound;
            }

            foreach (TreeNode child in node.Children)
            {
                List<string> res = SearchForCall(child, callsFound);
                foreach (string name in res)
                {
                    if (!callsFound.Contains(name))
                    {
                        callsFound.Add(name);
                    }
                }
            }

            return callsFound;
        }

        private void AddFunctionCall(string functionName)
        {
            List<SymbolTable> scopesRelatedToFunction = SymbolTables.FindAll(s =>
            {
                string[] scopeName = s.Scope.Split(' ');
                string scopeFunctionName = scopeName.Last();

                return scopeFunctionName == functionName;
            });

            StackFrame newStackFrame = new() { FunctionName = functionName };

            foreach (SymbolTable scope in scopesRelatedToFunction)
            {
                foreach (var entry in scope.Names)
                {
                    newStackFrame.FrameData.Add(entry.Key);
                }
            }

            UpdatedStack.Add(newStackFrame);

            MemoryData memoryData = new() { DataSegment = UpdatedDataSegment, Stack = UpdatedStack };
            ExecutionSimulator.AddStep($"'{functionName}' function call", memoryData);
        }

        private void RemoveFunctionCall(string functionName)
        {
            StackFrame stackFrameToDelete = UpdatedStack.Find(sf =>
            {
                return sf.FunctionName == functionName;
            })!;

            UpdatedStack.Remove(stackFrameToDelete);

            MemoryData memoryData = new() { DataSegment = UpdatedDataSegment, Stack = UpdatedStack };
            ExecutionSimulator.AddStep($"'{functionName}' function returned", memoryData);
        }
    }
}
