using Core.DTO;
using Core.ServiceContracts;

namespace Core.Services
{
    public class AIOptimizationService : IAIOptimizationService
    {
        public SourceOptResponseDto Optimize(SourceOptRequestDto sourceOptRequestDto)
        {
            // TODO : Use actual AI
            return new()
            {
                OptimizedCode = "لقد كنت اعاني من الصلع, ولكن بعد استعمال حمسي اوبتميزيشن اصبحت اصلع تماما, انه حقا رائع"
            };
        }
    }
}
