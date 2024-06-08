using MagicVilla.Web.Models.Dto;

namespace MagicVilla.Web.Services.IService
{
    public interface IVillaNumberService
    {
        Task<T?> GetVillaAsync<T>();
        Task<T?> GetAsync<T>(int id);
        Task<T?> CreateAsync<T>(VillaNumberCreateDto createDto);
        Task<T?> UpdateAsync<T>(VillaNumberUpdateDto updateDto);
        Task<T?> DeleteAsync<T>(int id);
    }
}
