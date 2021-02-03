using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Transfermarkt.Web.Services
{
    public interface IData<T> where T: class
    {
        IEnumerable<T> GetByDetails();
        T Get(int id);
        T Get(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Update(T entity);
        T Update(int id, T entity);
        T Delete(T entity);
        T Delete(int id);
    }
}
