using AoiCryptoAPI.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AoiCryptoAPI.Services
{
    public class ImageUploadService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private const string ImgbbApiUrl = "https://api.imgbb.com/1/upload";

        public ImageUploadService(IHttpClientFactory httpClientFactory, string apiKey)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = apiKey;
        }

        public async Task<ImgbbResponse> UploadImageAsync(string imageBase64, string name = null, int? expiration = 600)
        {
            var requestUri = $"{ImgbbApiUrl}?key={_apiKey}";

            if (expiration.HasValue)
            {
                requestUri += $"&expiration={expiration.Value}";
            }

            var client = _httpClientFactory.CreateClient();

            using var content = new MultipartFormDataContent
            {
                { new StringContent(imageBase64), "image" }
            };

            if (!string.IsNullOrEmpty(name))
            {
                content.Add(new StringContent(name), "name");
            }

            var response = await client.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to upload image: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine(responseContent);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<ImgbbResponse>(responseContent, options);
        }
    }
}
