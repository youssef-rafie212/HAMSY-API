using Core.ServiceContracts;
using Core.Services;

namespace HAMSY_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<ICompilationService, CompilationService>();
            builder.Services.AddScoped<IOCRService, OCRService>();
            builder.Services.AddScoped<IMemoryAnalysisService, MemoryAnalysisService>();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
