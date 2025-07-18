using Charity.Data;
using Charity.Models;
using Charity.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace Charity.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly CharityContext _db;
        public CategoryRepository(CharityContext db) : base(db)
        {
            _db = db;
        }

        //public async Task<Category> UpdateAsync(int id, UpdateCategoryDto obj)
        //{
        //    var existingCategory = await _db.Categories.
        //        FirstOrDefaultAsync(item => item.CategoryId == id);

        //    if (existingCategory == null)
        //        return null;
            
        //    existingCategory.Name = obj.Name;
        //    await _db.SaveChangesAsync();
        //    return existingCategory;
        //}

        public async Task<Category> UpdateStatusAsync(Guid id)
        {
            var existingCategory = await _db.Categories.
               FirstOrDefaultAsync(item => item.Id == id);

            if (existingCategory == null) return null;
            //existingCategory.ActiveStatus = !existingCategory.ActiveStatus;
            await _db.SaveChangesAsync();
            return existingCategory;
        }
    }
}
