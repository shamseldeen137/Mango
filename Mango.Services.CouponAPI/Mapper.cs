using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI
{
    public static class MapperConfig
    {

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Coupon, CouponDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
