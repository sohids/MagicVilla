using MagicVilla.Web.Models;

namespace MagicVilla.Web.Services.IService
{
    public interface IBaseService
    {
        ApiResponse responseModel { get; set; }
        Task<T?> SendAsync<T>(ApiRequest apiRequest);
    }
}
