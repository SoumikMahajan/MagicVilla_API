using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Villa> CreateAsyncSP(VillaCreateDTO villaCreateDTO)
        {
            //var affectRows = await _db.Database.ExecuteSqlInterpolatedAsync($"Exec [dbo].[MagicVillaApiVilla] @OPERATION_ID=2,@Name={villaCreateDTO.Name},@Details={villaCreateDTO.Details},@Rate={villaCreateDTO.Rate},@Sqft={villaCreateDTO.Sqft},@Occupancy={villaCreateDTO.Occupancy},@ImageUrl={villaCreateDTO.ImageUrl},@Amenity={villaCreateDTO.Amenity}").ToListAsync();
            var affectRows = await dbSet.FromSqlInterpolated($"Exec [dbo].[MagicVillaApiVilla] @OPERATION_ID=2,@Name={villaCreateDTO.Name},@Details={villaCreateDTO.Details},@Rate={villaCreateDTO.Rate},@Sqft={villaCreateDTO.Sqft},@Occupancy={villaCreateDTO.Occupancy},@ImageUrl={villaCreateDTO.ImageUrl},@Amenity={villaCreateDTO.Amenity}").ToListAsync();
            return affectRows.FirstOrDefault();
        }
    }
}
