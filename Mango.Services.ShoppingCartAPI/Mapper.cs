using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Models.Entities;

namespace Mango.Services.ShoppingCartAPI
{
    public static class MapperConfig
    {

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                cfg.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
