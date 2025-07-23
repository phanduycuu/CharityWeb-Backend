

using Charity.Models;


namespace Charity.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        //Task<Customer> UpdateAsync(int id, UpdateCustomerDto customerDto, string urlImage);
    }
}
