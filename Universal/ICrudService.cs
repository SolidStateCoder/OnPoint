using System.Collections.Generic;

namespace OnPoint.Universal
{
    public interface ICrudService<T>
    {
        T CreateNewItem();
        IEnumerable<T> RefreshItems();
        IEnumerable<T> SaveItems(IEnumerable<T> items);
        bool DeleteItem(T item);
    }
}