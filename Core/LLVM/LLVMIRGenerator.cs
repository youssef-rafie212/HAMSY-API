using Core.Domain.Entities;
using Core.Enums;
using LLVMSharp.Interop;

// TODO: integrate symbol table for better scope handling.
namespace Core.LLVM
{
    public class LLVMIRGenerator
    {
        private LLVMContextRef _context;
        private LLVMModuleRef _module;
        private LLVMBuilderRef _builder;
        private Dictionary<string, LLVMValueRef> _declaredNames = [];
        private Dictionary<string, LLVMValueRef> _globalNames = [];
        private Dictionary<string, LLVMTypeRef> _functionTypes = [];
        private LLVMValueRef _currentFunction;

        public LLVMIRGenerator(string moduleName)
        {
            _context = LLVMContextRef.Create();
            _module = _context.CreateModuleWithName(moduleName);
            _builder = _context.CreateBuilder();
        }

        public LLVMValueRef Traverse(TreeNode node)
        {
            if (node.Type == TreeNodeType.Program.ToString())
            {
                foreach (TreeNode c in node.Children)
                {
                    Traverse(c);
                }
                return default;
            }
            else if (node.Type == TreeNodeType.Terminal.ToString())
            {
                return GenerateTerminal(node);
            }
            else if (node.Type == TreeNodeType.Expression.ToString())
            {
                return GenerateExpression(node);
            }
            else if (node.Type == TreeNodeType.FunctionCall.ToString())
            {
                return GenerateFunctionCall(node);
            }
            else if (node.Type == TreeNodeType.VariableDeclaration.ToString())
            {
                return GenerateVariableDeclaration(node);
            }
            else if (node.Type == TreeNodeType.Assignment.ToString())
            {
                return GenerateAssignment(node);
            }
            else if (node.Type == TreeNodeType.Condition.ToString())
            {
                return GenerateCondition(node);
            }
            else if (node.Type == TreeNodeType.ReturnStatement.ToString())
            {
                return GenerateReturn(node);
            }
            else if (node.Type == TreeNodeType.IfStatement.ToString())
            {
                return GenerateIfStatement(node);
            }
            else if (node.Type == TreeNodeType.WhileLoop.ToString())
            {
                return GenerateWhileLoop(node);
            }
            else if (node.Type == TreeNodeType.FunctionDefinition.ToString() || node.Type == TreeNodeType.MainFunction.ToString())
            {
                return GenerateFunction(node);
            }
            else
            {
                throw new NotSupportedException();
            }

        }

        private LLVMValueRef GenerateTerminal(TreeNode node)
        {
            if (int.TryParse(node.Value, out int number))
            {
                return LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, (ulong)number);
            }
            else
            {
                LLVMValueRef variablePointer = _declaredNames[node.Value];
                return _builder.BuildLoad2(LLVMTypeRef.Int32, variablePointer);
            }
        }

        private LLVMValueRef GenerateExpression(TreeNode node)
        {
            if (node.Children.Count == 1)
            {
                return Traverse(node.Children[0]);
            }
            else if (node.Children.Count == 3)
            {
                LLVMValueRef left = Traverse(node.Children[0]);
                LLVMValueRef right = Traverse(node.Children[2]);
                string op = node.Children[1].Value;

                switch (op)
                {
                    case "+":
                        return _builder.BuildAdd(left, right);
                    case "-":
                        return _builder.BuildSub(left, right);
                    case "*":
                        return _builder.BuildMul(left, right);
                    case "/":
                        return _builder.BuildFDiv(left, right);
                }
            }

            throw new NotSupportedException();
        }

        private LLVMValueRef GenerateFunctionCall(TreeNode node)
        {
            string funcName = node.Children[0].Value;
            LLVMValueRef function = _module.GetNamedFunction(funcName);

            LLVMValueRef arg1 = Traverse(node.Children[1]);
            LLVMValueRef arg2 = Traverse(node.Children[2]);

            return _builder.BuildCall2(_functionTypes[funcName], function, [arg1, arg2]);
        }

        private LLVMValueRef GenerateVariableDeclaration(TreeNode node)
        {
            string name = node.Children[0].Value;
            LLVMValueRef value = Traverse(node.Children[1]);

            if (_currentFunction.Handle == IntPtr.Zero)
            {
                LLVMValueRef globalVar = _module.AddGlobal(LLVMTypeRef.Int32, name);
                globalVar.Initializer = value;
                _declaredNames[name] = globalVar;
                _globalNames[name] = globalVar;
                return globalVar;
            }
            else
            {
                LLVMValueRef alloca = CreateAllocaEntry(_currentFunction, name);
                _builder.BuildStore(value, alloca);
                _declaredNames[name] = alloca;
                return alloca;
            }
        }

        private LLVMValueRef GenerateAssignment(TreeNode node)
        {
            string name = node.Children[0].Value;
            LLVMValueRef variableAllocatedSpace = _declaredNames[name];

            LLVMValueRef value = Traverse(node.Children[1]);

            return _builder.BuildStore(value, variableAllocatedSpace);
        }

        private LLVMValueRef GenerateCondition(TreeNode node)
        {
            string conditionOperator = node.Children[1].Value;

            LLVMValueRef leftValue = Traverse(node.Children[0]);
            LLVMValueRef rightValue = Traverse(node.Children[2]);

            LLVMIntPredicate predicate = conditionOperator switch
            {
                "==" => LLVMIntPredicate.LLVMIntEQ,
                "!=" => LLVMIntPredicate.LLVMIntNE,
                ">" => LLVMIntPredicate.LLVMIntSGT,
                ">=" => LLVMIntPredicate.LLVMIntSGE,
                "<" => LLVMIntPredicate.LLVMIntSLT,
                "<=" => LLVMIntPredicate.LLVMIntSLE,
                _ => throw new Exception($"Invalid condition operator '{conditionOperator}'")
            };

            return _builder.BuildICmp(predicate, leftValue, rightValue, "condition");
        }

        private LLVMValueRef GenerateReturn(TreeNode node)
        {
            LLVMValueRef value = Traverse(node.Children[0]);
            return _builder.BuildRet(value);
        }

        private LLVMValueRef GenerateIfStatement(TreeNode node)
        {
            int indxOfElse = 0;
            bool elseExists = false;
            while (indxOfElse < node.Children.Count)
            {
                if (node.Children[indxOfElse].Type == TreeNodeType.ElseStatement.ToString())
                {
                    elseExists = true;
                    break;
                }
                indxOfElse++;
            }

            TreeNode conditionNode = node.Children[0];

            TreeNode? elseBlockNode = null;
            if (elseExists)
            {
                elseBlockNode = node.Children[indxOfElse];
            }

            LLVMValueRef condValue = GenerateCondition(conditionNode);

            LLVMBasicBlockRef thenBB = _currentFunction.AppendBasicBlock("if.then");
            LLVMBasicBlockRef mergeBB = _currentFunction.AppendBasicBlock("if.end");
            LLVMBasicBlockRef elseBB = new();

            if (elseExists)
            {
                elseBB = _currentFunction.AppendBasicBlock("if.else");
                _builder.BuildCondBr(condValue, thenBB, elseBB);
            }
            else
            {
                _builder.BuildCondBr(condValue, thenBB, mergeBB);
            }

            _builder.PositionAtEnd(thenBB);
            for (int i = 1; i < indxOfElse; i++)
            {
                Traverse(node.Children[i]);
            }

            _builder.BuildBr(mergeBB);

            if (elseExists)
            {
                _builder.PositionAtEnd(elseBB);
                foreach (TreeNode c in node.Children[indxOfElse].Children)
                {
                    Traverse(c);
                }
                _builder.BuildBr(mergeBB);
            }

            _builder.PositionAtEnd(mergeBB);

            return LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0, false);
        }

        private LLVMValueRef GenerateWhileLoop(TreeNode node)
        {
            TreeNode conditionNode = node.Children[0];

            LLVMBasicBlockRef condBB = _currentFunction.AppendBasicBlock("while.cond");
            LLVMBasicBlockRef loopBB = _currentFunction.AppendBasicBlock("while.body");
            LLVMBasicBlockRef endBB = _currentFunction.AppendBasicBlock("while.end");

            _builder.BuildBr(condBB);

            _builder.PositionAtEnd(condBB);
            LLVMValueRef condValue = GenerateCondition(conditionNode);
            _builder.BuildCondBr(condValue, loopBB, endBB);

            _builder.PositionAtEnd(loopBB);

            for (int i = 1; i < node.Children.Count; i++)
            {
                Traverse(node.Children[i]);
            }

            _builder.BuildBr(condBB);

            _builder.PositionAtEnd(endBB);

            return LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0, false);
        }

        private LLVMValueRef GenerateFunction(TreeNode node)
        {
            List<string> keysToRemove = [];

            foreach (var entry in _declaredNames)
            {
                if (!_globalNames.ContainsKey(entry.Key))
                {
                    keysToRemove.Add(entry.Key);
                }
            }

            foreach (string k in keysToRemove)
            {
                _declaredNames.Remove(k);
            }

            string functionName = node.Children[0].Value;
            LLVMTypeRef functionType;
            int numParams;

            if (node.Type == TreeNodeType.MainFunction.ToString())
            {
                numParams = 0;
                functionType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, [], false);
            }
            else
            {
                numParams = 2;
                LLVMTypeRef[] paramTypes = [LLVMTypeRef.Int32, LLVMTypeRef.Int32];
                functionType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, paramTypes, false);
            }

            _functionTypes[functionName] = functionType;
            LLVMValueRef function = _module.AddFunction(functionName, functionType);
            LLVMBasicBlockRef entryBB = function.AppendBasicBlock("entry");
            _builder.PositionAtEnd(entryBB);
            _currentFunction = function;

            if (numParams > 0)
            {
                LLVMValueRef param0 = function.GetParam(0);
                LLVMValueRef param1 = function.GetParam(1);

                LLVMValueRef allocaParam0 = CreateAllocaEntry(function, node.Children[1].Value);
                LLVMValueRef allocaParam1 = CreateAllocaEntry(function, node.Children[2].Value);

                _builder.BuildStore(param0, allocaParam0);
                _builder.BuildStore(param1, allocaParam1);

                _declaredNames[node.Children[1].Value] = allocaParam0;
                _declaredNames[node.Children[2].Value] = allocaParam1;
            }

            int bodyStartIndx = (node.Type == TreeNodeType.MainFunction.ToString()) ? 1 : 3;
            for (int i = bodyStartIndx; i < node.Children.Count; i++)
            {
                Traverse(node.Children[i]);
            }

            return function;
        }

        private LLVMValueRef CreateAllocaEntry(LLVMValueRef function, string varName)
        {
            LLVMBasicBlockRef block = function.FirstBasicBlock;
            LLVMBuilderRef tempBuilder = _context.CreateBuilder();

            if (block.FirstInstruction.Handle != IntPtr.Zero)
            {
                tempBuilder.PositionBefore(block.FirstInstruction);
            }
            else
            {
                tempBuilder.PositionAtEnd(block);
            }

            LLVMValueRef alloca = tempBuilder.BuildAlloca(LLVMTypeRef.Int32, varName);
            tempBuilder.Dispose();

            return alloca;
        }

        public string GetIR()
        {
            return _module.PrintToString();
        }
    }
}
