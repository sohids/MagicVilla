using MagicVilla.Web.Models.Dto;

namespace MagicVilla.Web.Services.IService
{
    public interface IVillaService
    {
        Task<T?> GetVillaAsync<T>(string token);
        Task<T?> GetAsync<T>(int id, string token);
        Task<T?> CreateAsync<T>(VillaCreateDto createDto, string token);
        Task<T?> UpdateAsync<T>(VillaUpdateDto updateDto, string token);
        Task<T?> DeleteAsync<T>(int id, string token);
    }
}
