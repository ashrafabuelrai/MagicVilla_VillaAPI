using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IRepository<T> where T:class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProp = null,int pageSize=0,int pageNumber=1);
        T Get(Expression<Func<T, bool>> filter, string? includeProp = null, bool tracked = false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
