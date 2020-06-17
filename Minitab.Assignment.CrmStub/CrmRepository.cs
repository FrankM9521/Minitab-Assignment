using Microsoft.EntityFrameworkCore;
using Minitab.Assignment.CrmStub.Interfaces;
using Minitab.Assignment.CrmStub.Mappers;
using Minitab.Assignment.DataContext;
using Minitab.Assignment.DomainModels;
using Minitab.Assignment.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Minitab.Assignment.CrmStub
{
    /// <summary>
    /// EF Core repository
    /// </summary>
    public class CrmRepository : ICrmRepository
    {
        private readonly MinitabDbContext _dbContext;
        public CrmRepository(MinitabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Fake CRM Stub. Inserts customer
        /// </summary>
        /// <param name="customerDomainModel"></param>
        /// <returns></returns>
        public async Task UpsertCustomer(CustomerDomainModel customerDomainModel)
        {
            await _dbContext.AddAsync(customerDomainModel.ToCustomerEntity());
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Just to help with end to end testing
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public  async Task<CustomerDomainModel> GetByEmail(string emailAddress)
        {
             var entity =  await _dbContext.Set<CustomerEntity>()
                .Include("Address")
                .Where(c => c.CustomerEmail.Equals(emailAddress))
                    .FirstOrDefaultAsync();
            return entity?.ToCustomerDomainModel();
        }

        /// <summary>
        /// Just to help with end to end testing
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            await _dbContext.Database.ExecuteSqlRawAsync("delete from addresses");
            await _dbContext.Database.ExecuteSqlRawAsync("delete from customers");
        }
    }
}
