using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Migrations;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/Coupon")]
    [ApiController]
    [Authorize]

    public class CouponApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;
        public CouponApiController(AppDbContext db,IMapper mapper)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetCoupon()
        {
            try
            {
                var coupons = _db.Coupons.ToList();
                var result = _mapper.Map<List<CouponDto>>(coupons);
                _responseDto.Result = result;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "success";
            }
            catch (Exception e)
            {
                _responseDto.IsSuccuess=false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;

        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetCoupon(int id)
        {
            try
            {
                var coupon = _db.Coupons.First(a=>a.CouponId==id);
                var result = _mapper.Map<CouponDto>(coupon);
                _responseDto.Result = result;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "success";
            }
            catch (Exception e)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetCouponByCode(string code)
        {
            try
            {
                var coupon = _db.Coupons.First(a=>a.CouponCode.ToLower()==code.ToLower());
                var result = _mapper.Map<CouponDto>(coupon);
                _responseDto.Result = result;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "success";
            }
            catch (Exception e)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public async Task< ResponseDto> Post([FromBody]CouponDto dto)
        {
            try
            {
                var result = _mapper.Map<Coupon>(dto);
            await   _db.Coupons.AddAsync(result);
               await _db.SaveChangesAsync();
                var obj = _mapper.Map<CouponDto>(result);

                _responseDto.Result = obj;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "success";
            }
            catch (Exception e)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
        } 
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                var coupon = _db.Coupons.First(a => a.CouponId == id);
                _db.Coupons.Remove(coupon);
               _db.SaveChanges();

                _responseDto.Result = id;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "success";
            }
            catch (Exception e)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
        } [HttpPut]
        [Authorize(Roles = "ADMIN")]

        public ResponseDto Put([FromBody]CouponDto dto)
        {
            try
            {
                var result = _mapper.Map<Coupon>(dto);
               _db.Coupons.Update(result);
               _db.SaveChanges();
                var obj = _mapper.Map<CouponDto>(result);

                _responseDto.Result = obj;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "success";
            }
            catch (Exception e)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
        }
    }
}
