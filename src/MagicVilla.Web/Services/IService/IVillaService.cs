using MagicVilla.Web.Models.Dto;

namespace MagicVilla.Web.Services.IService
{
    public interface IVillaService
    {
        Task<T?> GetVillaAsync<T>();
        Task<T?> GetAsync<T>(int id);
        Task<T?> CreateAsync<T>(VillaCreateDto createDto);
        Task<T?> UpdateAsync<T>(VillaUpdateDto updateDto);
        Task<T?> DeleteAsync<T>(int id);
    }
}
