using MagicVilla.Api.Data;
using MagicVilla.Api.Models;

namespace MagicVilla.Api.Repository.IRepository
{
    public class VillaNumberRepository : Repository<VillaNumber>,  IVillaNumberRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VillaNumberRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.LastUpdatedDate = DateTime.Now;
            _dbContext.VillaNumbers.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}

