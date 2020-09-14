using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class RepositoryAsync<T> : IRepositoryAsync<T>
    {
        public Task<bool> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(IEnumerable<T> listEntity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Any(Expression<Func<T, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> Count(Expression<Func<T, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(IEnumerable<T> listEntity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteWhere(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<T> Find(object id, bool isAsNoTracking = false)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<T>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T>> updateExpression)
        {
            throw new NotImplementedException();
        }
    }
}
