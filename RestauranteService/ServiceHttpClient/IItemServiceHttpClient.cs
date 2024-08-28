using RestauranteService.Dtos;

namespace RestauranteService.ServiceHttpClient
{
    public interface IItemServiceHttpClient
    {
        void EnviaRestauranteItemService(RestauranteReadDto readDto);
    }
}