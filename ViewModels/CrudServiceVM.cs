using FluentAssertions;
using OnPoint.Universal;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace OnPoint.ViewModels
{
    public class CrudServiceVM<T> : CrudVM<T> where T : class, IIsChanged
    {
        public ICrudService<T> CrudService { get => _CrudService; set => this.RaiseAndSetIfChanged(ref _CrudService, value); }
        private ICrudService<T> _CrudService = default;

        public CrudServiceVM(ICrudService<T> crudService)
        {
            crudService.Should().NotBeNull();
            CrudService = crudService;
        }

        protected override async Task<T> AddNewItemAsync(CancellationToken token) => await CrudService.CreateNewItemAsync(token);
        protected override IEnumerable<T> RefreshItems() => CrudService.RefreshItems();
        protected override IEnumerable<T> SaveChangedItems() => CrudService.SaveItems(Contents.Where(x => x.IsChanged));
        protected override bool DelectItem(T item) => CrudService.DeleteItem(item);
        protected override IEnumerable<T> SearchItems(params Expression<Func<T, bool>>[] filters) => CrudService.SearchItems(filters);
    }
}