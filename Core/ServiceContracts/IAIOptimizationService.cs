using Core.DTO;

namespace Core.ServiceContracts
{
    public interface IAIOptimizationService
    {
        public SourceOptResponseDto Optimize(SourceOptRequestDto shittyCode);
    }
}
