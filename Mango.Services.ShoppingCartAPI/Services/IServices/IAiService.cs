namespace Mango.Services.ShoppingCartAPI.Services.IServices
{
    public interface IAiService
    {
        Task<string> GetRecommendationAsync(string prompt);
    }
}
