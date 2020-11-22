using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace OnPoint.Universal
{
    public interface ICrudService<T>
    {
        Task<T> CreateNewItemAsync(CancellationToken token);
        IEnumerable<T> RefreshItems();
        IEnumerable<T> SaveItems(IEnumerable<T> items);
        bool DeleteItem(T item);
        IEnumerable<T> SearchItems(params Expression<Func<T, bool>>[] filters);
    }
}