using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;

namespace Mango.Services.ProductAPI
{
    public static class MapperConfig
    {

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
