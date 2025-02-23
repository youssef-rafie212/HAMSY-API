using Core.DTO;

namespace Core.ServiceContracts
{
    public interface ICompilationService
    {
        LexicalResponseDto LexicalAnalysis(LexicalRequestDto lexicalRequestDto);
        SyntaxResponseDto SyntaxAnalysis(SyntaxRequestDto syntaxRequestDto);
        SymbolTableResponseDto SymbolTables(SymbolTablesRequestDto symbolTablesRequestDto);
        SemanticResponseDto SemanticAnalysis(SemanticRequestDto semanticRequestDto);
        IRGenResponseDto IRGeneration(IRGenRequestDto irGenRequestDto);
        IROptResponseDto IROptimization(IROptRequestDto irOptRequestDto);
        InsSelResponseDto InstructionSelection(InsSelRequestDto insSelRequestDto);
        RegAllocResponseDto RegisterAllocation(RegAllocRequestDto regAllocRequestDto);
        InsSchedResponseDto InstructionScheduling(InsSchedRequestDto insSchedRequestDto);
    }
}
