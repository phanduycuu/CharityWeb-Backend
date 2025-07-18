using Charity.Models;
namespace Charity.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> UpdateStatusAsync(Guid id);
        //Task<Category> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
    }
}
