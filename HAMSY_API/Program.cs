using Core.ServiceContracts;
using Core.Services;
using HAMSY_API.Middlewares;

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
            builder.Services.AddScoped<IAIOptimizationService, AIOptimizationService>();

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
