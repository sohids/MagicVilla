using System.Linq.Expressions;
using MagicVilla.Api.Models;

namespace MagicVilla.Api.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task CreateAsync(Villa entity);
        Task RemoveAsync(Villa entity);
        Task UpdateAsync(Villa entity);
        Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>>? filter = null);
        Task<Villa?> GetAsync(Expression<Func<Villa, bool>>? filter = null, bool tracked = true);
        Task SaveAsync();
    }
}
