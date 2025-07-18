

using Charity.Dtos.Category;
using Charity.Models;

namespace Charity.Mapper
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Name = category.Name,
                Id = category.Id,

            };
        }

        public static Category ToCategoryFromCreateDto(this CreateCategoryDto createCategoryDto)
        {
            return new Category
            {
                Name = createCategoryDto.Name,
            };
        }
    }
}
