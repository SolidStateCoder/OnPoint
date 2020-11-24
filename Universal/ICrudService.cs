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
        Task<IEnumerable<T>> SaveItemsAsync(CancellationToken token, IEnumerable<T> items);
        Task<bool> DeleteItemAsync(CancellationToken token, T item);
        Task<IEnumerable<T>> SearchItemsAsync(CancellationToken token, params Expression<Func<T, bool>>[] filters);
    }
}