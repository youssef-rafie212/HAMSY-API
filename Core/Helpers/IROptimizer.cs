using LLVMSharp.Interop;

namespace Core.Helpers
{
    public class IROptimizer
    {
        private LLVMModuleRef _module;

        public IROptimizer(LLVMModuleRef module)
        {
            _module = module;
        }

        public unsafe void ApplyOptimizations()
        {
            LLVMPassManagerRef passManager = LLVM.CreatePassManager();

            LLVM.AddInstructionCombiningPass(passManager);
            LLVM.AddPromoteMemoryToRegisterPass(passManager);
            LLVM.AddGVNPass(passManager);
            LLVM.AddCFGSimplificationPass(passManager);

            LLVM.RunPassManager(passManager, _module);

            LLVM.DisposePassManager(passManager);
        }

        public string GetIRString()
        {
            return _module.PrintToString();
        }

        public LLVMModuleRef GetIRModule()
        {
            return _module;
        }
    }
}
