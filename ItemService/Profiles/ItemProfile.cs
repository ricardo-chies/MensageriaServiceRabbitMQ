using AutoMapper;
using ItemService.Dtos;
using ItemService.Models;

namespace ItemService.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<RestauranteReadDto, Restaurante>().
                ForMember(read => read.IdExterno, opt => opt.MapFrom(r => r.Id));
            CreateMap<Restaurante, RestauranteReadDto>();
            CreateMap<ItemCreateDto, Item>();
            CreateMap<Item, ItemCreateDto>();
        }
    }
}