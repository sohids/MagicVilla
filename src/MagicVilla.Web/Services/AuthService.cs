using MagicVilla.Utility;
using MagicVilla.Web.Models;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Services.IService;

namespace MagicVilla.Web.Services
{
    public class AuthService: BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string? _baseUrl;

        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _baseUrl = configuration["ServiceUrls:VillaApi"];
        }

        public Task<T?> LoginAsync<T>(LoginRequestDto objToCreate)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Post,
                Data = objToCreate,
                Url = _baseUrl + "/api/v1/UsersAuth/login"
            });
        }

        public Task<T?> RegistrationAsync<T>(RegistrationRequestDto objToCreate)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Get,
                Data = objToCreate,
                Url = _baseUrl + "/api/v1/UsersAuth/register"
            });
        }
    }
}
