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
        Task<IEnumerable<T>> RefreshItemsAsync(CancellationToken token);
        Task<IEnumerable<T>> SaveChangedItemsAsync(CancellationToken token, IEnumerable<T> items);
        Task<T> SaveItemAsync(CancellationToken token, T item);
        Task<bool> DeleteItemAsync(CancellationToken token, T item);
        Task<IEnumerable<T>> SearchItemsAsync(CancellationToken token, params Expression<Func<T, bool>>[] filters);
    }
}