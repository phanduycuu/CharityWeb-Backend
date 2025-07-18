using System.ComponentModel.DataAnnotations;

namespace Charity.Dtos.Category
{
    public class CreateCategoryDto
    {

        [Required]
        public string Name { get; set; }
    }
}
