using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleDronBot.Interfaces
{
    interface IBaseProvider<T> where T : class
    {
        Task Update(T item);
        Task Delete(T item);
        Task Create(T item);

        ValueTask<T> FindById(long id);

        IQueryable<T> Get();
        ValueTask<IEnumerable<T>> Get(Func<T, bool> predicate);
    }
}
