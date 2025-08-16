using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Web.Services.BaseServices
{
    public class BaseService : IBaseService
    {
        public ResponseDto responseModel { get; set; }
        IHttpClientFactory httpClient { get; set; }

        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory _httpClient,ITokenProvider tokenProvider)
        {
            httpClient = _httpClient;
            _tokenProvider=tokenProvider;
        }

        public async Task<ResponseDto> SendAsync(RequestDto apiRequest,bool withBearer=true)
        {
            try
            {
                using (var client = httpClient.CreateClient("MangoAPI"))
                {
                    HttpRequestMessage message = new HttpRequestMessage();
                    message.Headers.Add("Accept", "application/json");
                    if (withBearer)
                    {
                        var token = _tokenProvider.GetToken();
                        message.Headers.Authorization=new System.Net.Http.Headers.AuthenticationHeaderValue( "Bearer",token);
                    }
                    message.RequestUri = new Uri(apiRequest.Url);
                    client.DefaultRequestHeaders.Clear();
                    if (apiRequest.Data is not null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");

                        //var json = JsonConvert.SerializeObject(apiRequest.Data);
                        //var content = new StringContent(json, Encoding.UTF8, "application/json");
                        //message.Content = content;
                    }
                    
                    HttpResponseMessage apiresponse = null;
                    switch (apiRequest.ApiType)
                    {
                        case SD.ApiType.GET:
                            message.Method = HttpMethod.Get;
                            break;
                        case SD.ApiType.POST:
                            message.Method = HttpMethod.Post;
                            break;
                        case SD.ApiType.PUT:
                            message.Method = HttpMethod.Put;
                            break;
                        case SD.ApiType.DELETE:
                            message.Method = HttpMethod.Delete;
                            break;
                        default:
                            message.Method = HttpMethod.Get;
                            break;

                    }
                    
                    apiresponse = await client.SendAsync(message);
                    switch (apiresponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return new() { IsSuccess = false, Message = "Not Found" };
                        case HttpStatusCode.Forbidden:
                            return new() { IsSuccess = false, Message = "Access Denied" };
                        case HttpStatusCode.Unauthorized:
                            return new() { IsSuccess = false, Message = "Unauthorized" };
                        case HttpStatusCode.InternalServerError:
                            return new() { IsSuccess = false, Message = "Internal Server Error" };
                        default:

                            var apiContent = await apiresponse.Content.ReadAsStringAsync();
                            var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                            return apiResponseDto;
                    }


                }
            }
            catch (Exception e)
            {
                var dto = new ResponseDto
                {
                    Message = "Error",
                    IsSuccess = false,
                    ErrorrMessages = new List<string> { e.Message.ToString() }

                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(res);
                return apiResponseDto;

            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
