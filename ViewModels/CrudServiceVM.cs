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

        protected override async Task<T> AddNewItemAsync(CancellationToken token)
        {
            BusyMessageOverride = "Adding...";
            return await CrudService.CreateNewItemAsync(token);
        }

        protected async override Task<bool> DelectItemAsync(CancellationToken token, T item) => await CrudService.DeleteItemAsync(token, item);
        protected async override Task<IEnumerable<T>> SaveChangedItemsAsync(CancellationToken token) => await CrudService.SaveItemsAsync(token, Contents.Where(x => x.IsChanged));
        protected async override Task<IEnumerable<T>> RefreshItemsAsync(CancellationToken token) => await CrudService.RefreshItemsAsync(token);
        protected async override Task<IEnumerable<T>> SearchItemsAsync(CancellationToken token, params Expression<Func<T, bool>>[] filters) => await CrudService.SearchItemsAsync(token, filters);
    }
}