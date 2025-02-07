using Core.DTO;

namespace Core.ServiceContracts
{
    public interface ICompilationService
    {
        LexicalResponseDto LexicalAnalysis(LexicalRequestDto lexicalRequestDto);
        SyntaxResponseDto SyntaxAnalysis(SyntaxRequestDto syntaxRequestDto);
        SemanticResponseDto SemanticAnalysis(SemanticRequestDto semanticRequestDto);
        IRGenResponseDto IRGeneration(IRGenRequestDto irGenRequestDto);
        IROptResponseDto IROptimization(IROptRequestDto irOptRequestDto);
        InsSelResponseDto InstructionSelection(InsSelRequestDto insSelRequestDto);
        RegAllocResponseDto RegisterAllocation(RegAllocResponseDto regAllocResponseDto);
        InsSchedResponseDto InstructionScheduling(InsSchedRequestDto insSchedRequestDto);
    }
}
