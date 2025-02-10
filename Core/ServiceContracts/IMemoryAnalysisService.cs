using Core.DTO;

namespace Core.ServiceContracts
{
    public interface IMemoryAnalysisService
    {
        MemoryResponseDto MemoryAnalysis(MemoryRequestDto memoryRequestDto);
    }
}
