using Core.DTO;
using Core.ServiceContracts;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Core.Services
{
    public class AIOptimizationService : IAIOptimizationService
    {
        public async Task<SourceOptResponseDto> Optimize(SourceOptRequestDto sourceOptRequestDto, string apiKey)
        {
            string optimizedCode = await SendRequest(sourceOptRequestDto.SourceCode, apiKey);
            return new SourceOptResponseDto() { OptimizedCode = optimizedCode };
        }

        private async Task<string> SendRequest(string sourceCode, string apiKey)
        {
            string endpoint = "https://api.openai.com/v1/chat/completions";
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                new { role = "system", content = "You are a code optimizer. Your task is to optimize the given code without explaining anything. Just return the optimized code and nothing else." },
                new { role = "user", content = sourceCode }
            },
                temperature = 0.2
            };

            string jsonRequest = JsonSerializer.Serialize(requestBody);
            using StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()?.Trim() ?? "";
        }
    }
}
