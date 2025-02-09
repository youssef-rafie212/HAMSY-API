using Core.DTO;
using Core.ServiceContracts;
using Tesseract;

namespace Core.Services
{
    public class OCRService : IOCRService
    {
        public OCRResponseDto ExtractCode(string imgPath)
        {
            string tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";

            try
            {
                using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imgPath))
                    {
                        using (var page = engine.Process(img))
                        {
                            string extractedText = page.GetText();
                            return new() { ExtractedCode = extractedText };
                        }
                    }
                }
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
