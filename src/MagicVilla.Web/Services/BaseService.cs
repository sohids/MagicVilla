using System.Text;
using MagicVilla.Utility;
using MagicVilla.Web.Models;
using MagicVilla.Web.Services.IService;
using Newtonsoft.Json;
using Serilog;

namespace MagicVilla.Web.Services
{
    public class BaseService: IBaseService
    {
        public ApiResponse responseModel { get; set; }
        public IHttpClientFactory HttpClientFactory { get; set; }

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            responseModel = new ApiResponse();
            HttpClientFactory = httpClientFactory;
        }
        public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = HttpClientFactory.CreateClient("VillaApi");
                var requestMessage = new HttpRequestMessage();
                requestMessage.Headers.Add("Accept", "application/json");
                requestMessage.RequestUri = new Uri(apiRequest.Url);
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8, "application/json");

                switch (apiRequest.ApiType)
                {
                    case StaticDetails.ApiType.Post:
                        requestMessage.Method = HttpMethod.Post;
                        break;

                    case StaticDetails.ApiType.Put:
                        requestMessage.Method = HttpMethod.Put;
                        break;

                    case StaticDetails.ApiType.Delete:
                        requestMessage.Method = HttpMethod.Delete;
                        break;

                    case StaticDetails.ApiType.Get:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                }
                //we should throw exception if URL is not correct
                //log the response result 
                //log the response code
                //throw exception if there is any type of exception while calling the api 
                var responseMessage = await client.SendAsync(requestMessage);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<T>(responseContent);
                return responseObject;
            }
            catch (Exception e)
            {
                var dto = new ApiResponse
                {
                    ErrorMessage = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var responseContent = JsonConvert.SerializeObject(dto);
                var responseObject = JsonConvert.DeserializeObject<T>(responseContent);
                return responseObject;
            }
        }
    }
}
