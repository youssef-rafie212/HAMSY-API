using Core.ServiceContracts;
using Core.Services;
using HAMSY_API.Middlewares;
using static HAMSY_API.Middlewares.GlobalExceptionHandler;

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

<<<<<<< HEAD
			app.UseMiddleware<GlobalExceptionMiddleware>();
			app.MapControllers();
=======
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.MapControllers();
>>>>>>> fb63cb68972850e8d2c90598fcc0788c53391f2d

			app.Run();
		}
	}
}