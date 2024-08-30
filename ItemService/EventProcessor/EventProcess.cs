﻿using AutoMapper;
using ItemService.Data;
using ItemService.Dtos;
using ItemService.Models;
using System.Text.Json;

namespace ItemService.EventProcessor
{
    public class EventProcess : IEventProcessor
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;
        public EventProcess(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }
        public void Process(string message)
        {
            using var scope = _scopeFactory.CreateScope();
            var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();

            var restauranteReadDto = JsonSerializer.Deserialize<RestauranteReadDto>(message);
            var restaurante = _mapper.Map<Restaurante>(restauranteReadDto);

            if(!itemRepository.ExisteRestauranteExterno(restaurante.Id))
            {
                itemRepository.CreateRestaurante(restaurante);
                itemRepository.SaveChanges();
            }
        }
    }
}
