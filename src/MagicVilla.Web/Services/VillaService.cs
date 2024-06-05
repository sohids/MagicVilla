using MagicVilla.Utility;
using MagicVilla.Web.Models;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Services.IService;

namespace MagicVilla.Web.Services
{
    public class VillaService: BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string? _baseUrl;

        public VillaService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory  = httpClientFactory;
            _baseUrl = configuration["ServiceUrls:VillaApi"];
        }

        public Task<T?> GetVillaAsync<T>()
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Get,
                Url = _baseUrl + "/api/villas"
            });
        }

        public Task<T?> GetAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Get,
                Url = _baseUrl + "/api/villaApi"
            });

        }

        public Task<T?> CreateAsync<T>(VillaCreateDto createDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Post,
                Data = createDto,
                Url = _baseUrl+"/api/villaApi"
            });
        }

        public Task<T?> UpdateAsync<T>(VillaUpdateDto updateDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Put,
                Data = updateDto,
                Url = _baseUrl + "/api/villaApi"+updateDto.Id
            });
        }

        public Task<T?> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.Delete,
                Url = _baseUrl + "/api/villaApi"+id
            });
        }

        
    }
}
