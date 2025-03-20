using Core.DTO;

namespace Core.ServiceContracts
{
    public interface IAIOptimizationService
    {
        public Task<SourceOptResponseDto> Optimize(SourceOptRequestDto sourceOptRequestDto, string apiKey);
    }
}
