using RestauranteService.Dtos;
using System.Text;
using System.Text.Json;

namespace RestauranteService.ServiceHttpClient
{
    public class ItemServiceHttpClient : IItemServiceHttpClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        public ItemServiceHttpClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }
        public async void EnviaRestauranteItemService(RestauranteReadDto readDto)
        {
            var content = new StringContent
                (
                    JsonSerializer.Serialize(readDto),
                    Encoding.UTF8,
                    "application/json"
                );

            await _client.PostAsync(_configuration["ItemService"], content);
        }
    }
}
