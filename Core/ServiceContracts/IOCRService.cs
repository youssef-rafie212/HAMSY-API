using Core.DTO;

namespace Core.ServiceContracts
{
    public interface IOCRService
    {
        OCRResponseDto ExtractCode(string imgPath);
    }
}
