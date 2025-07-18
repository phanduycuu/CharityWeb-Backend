
using Charity.Dtos.Category;
using Charity.Helper;

namespace Charity.Service.IService
{
    public interface ICategoryService
    {
        //Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto);
        Task<QueryObject<CategoryDto>> GetCategoriesAsync(int page, int limit);
        //Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto supplierDto);
        //Task<CategoryDto?> getByIDAsync(int id);
        //Task<CategoryDto?> UpdateStatusAsync(int id);
    }
}
