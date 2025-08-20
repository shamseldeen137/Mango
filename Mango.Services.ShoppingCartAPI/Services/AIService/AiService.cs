using Mango.Services.ShoppingCartAPI.Services.IServices;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Hosting;

namespace Mango.Services.ShoppingCartAPI.Services.AIService
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;

        public AiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetRecommendationAsync(string prompt)
        {
            var request = new
            {
               // model = "gpt-3.5-turbo",
                model = "gpt-4o-mini",
                messages = new[]
         {
        new { role = "user", content = prompt }
    },
                max_tokens = 1000,
                temperature = 0.7
            };

            _httpClient.DefaultRequestHeaders.Clear();
            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);
            var content = await response.Content.ReadAsStringAsync(); // <
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error {response.StatusCode}: {content}");
                throw new Exception($"OpenAI API error: {content}");
            }
            var result = await response.Content.ReadFromJsonAsync<OpenAiResponse>();
            return result?.Choices?.FirstOrDefault()?.Message?.Content ?? "No recommendation found.";
        }
    }
}
