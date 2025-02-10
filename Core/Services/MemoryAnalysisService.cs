using Core.Domain.Entities;
using Core.DTO;
using Core.Helpers;
using Core.ServiceContracts;

namespace Core.Services
{
    public class MemoryAnalysisService : IMemoryAnalysisService
    {
        private readonly ICompilationService _compilationService;

        public MemoryAnalysisService(ICompilationService compilationService)
        {
            _compilationService = compilationService;
        }

        public MemoryResponseDto MemoryAnalysis(MemoryRequestDto memoryRequestDto)
        {
            SyntaxResponseDto syntaxRes = _compilationService.SyntaxAnalysis(new() { SourceCode = memoryRequestDto.SourceCode });

            if (syntaxRes.Errors.Count > 0)
            {
                return new()
                {
                    Errors = syntaxRes.Errors,
                };
            }

            TreeNode parseTree = syntaxRes.ParseTree;
            SymbolTableResponseDto symbolTableRes = _compilationService.SymbolTables(new() { ParseTree = parseTree });

            if (symbolTableRes.Errors.Count > 0)
            {
                return new()
                {
                    Errors = symbolTableRes.Errors,
                };
            }

            List<SymbolTable> symbolTables = symbolTableRes.SymbolTables;

            MemoryAnalyzer analyzer = new(symbolTables, parseTree);
            analyzer.Analyze();
            List<ExecutionStep> executionSteps = analyzer.ExecutionSimulator.ExecutionSteps;

            return new()
            {
                ExecutionSteps = executionSteps,
            };
        }
    }
}
