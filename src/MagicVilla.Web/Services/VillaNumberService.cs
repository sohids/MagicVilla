using MagicVilla.Utility;
using MagicVilla.Web.Models;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Services.IService;

namespace MagicVilla.Web.Services
{
    public class VillaNumberService: BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string? _baseUrl;

        public VillaNumberService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory  = httpClientFactory;
            _baseUrl = configuration["ServiceUrls:VillaApi"];
        }

        public Task<T?> GetVillaAsync<T>(string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Get,
                Url = _baseUrl + "/api/VillaNumbers",
                Token = token
            });
        }

        public Task<T?> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Get,
                Url = _baseUrl + "/api/VillaNumbers/" + id,
                Token = token
            });

        }

        public Task<T?> CreateAsync<T>(VillaNumberCreateDto createDto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Post,
                Data = createDto,
                Url = _baseUrl+ "/api/VillaNumbers",
                Token = token
            });
        }

        public Task<T?> UpdateAsync<T>(VillaNumberUpdateDto updateDto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Put,
                Data = updateDto,
                Url = _baseUrl + "/api/VillaNumbers/" + updateDto.VillaNo,
                Token = token
            });
        }

        public Task<T?> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Delete,
                Url = _baseUrl + "/api/VillaNumbers/" +id,
                Token = token
            });
        }

        
    }
}
