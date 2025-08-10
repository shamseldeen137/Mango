namespace Mango.Services.ProductAPI.Models.Dto
{
    public class ResponseDto
    {
        public object Result { get; set; }

        public bool IsSuccuess { get; internal set; }
        public string Message { get; internal set; }
    }
}
