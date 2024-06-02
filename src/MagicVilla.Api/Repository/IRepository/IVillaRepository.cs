using MagicVilla.Api.Models;

namespace MagicVilla.Api.Repository.IRepository
{
    public interface IVillaRepository: IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entity);
    }
}
