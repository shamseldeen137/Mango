using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IBaseService : IDisposable

    {
        public ResponseDto responseModel { get; set; }
        Task<ResponseDto> SendAsync(RequestDto apiRequest, bool withBearer=true);
    }
}
