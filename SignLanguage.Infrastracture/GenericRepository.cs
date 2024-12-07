using SignLanguage.Core.Entities;
using SignLanguage.Core.Repository.Contract;
using SignLanguage.Core.Specifications;
using SignLanguage.Infrastracture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignLanguage.Infrastracture
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext=_dbContext;
        }
        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
        {
            throw new NotImplementedException();
        }
    }
}
