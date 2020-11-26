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
    /// <summary>
    /// Enhances CrudVM and automatically routes requests to an <see cref="ICrudService{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the items being crud'ed</typeparam>
    public class CrudServiceVM<T> : CrudVM<T> where T : class, IIsChanged
    {
        /// <summary>
        /// The class actually performing the CRUD.
        /// </summary>
        public ICrudService<T> CrudService { get => _CrudService; set => this.RaiseAndSetIfChanged(ref _CrudService, value); }
        private ICrudService<T> _CrudService = default;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crudService">The class actually performing the CRUD.</param>
        public CrudServiceVM(ICrudService<T> crudService)
        {
            crudService.Should().NotBeNull();
            CrudService = crudService;
        }

        protected override async Task<T> CreateNewItemAsync(CancellationToken token) { Debug("CreateNewItemAsync"); return await CrudService.CreateNewItemAsync(token); }
        protected async override Task<bool> DeleteItemAsync(CancellationToken token, T item) { Debug("DeleteItemAsync"); return await CrudService.DeleteItemAsync(token, item); }
        protected async override Task<T> SaveItemAsync(CancellationToken token, T item) { Debug("SaveItemAsync"); return await CrudService.SaveItemAsync(token, item); }
        protected async override Task<IEnumerable<T>> SaveChangedItemsAsync(CancellationToken token) { Debug("SaveChangedItemsAsync"); return await CrudService.SaveChangedItemsAsync(token, Contents.Where(x => x.IsChanged)); }
        protected async override Task<IEnumerable<T>> SearchItemsAsync(CancellationToken token, params Expression<Func<T, bool>>[] filters) { Debug("SearchItemsAsync"); return await CrudService.SearchItemsAsync(token, filters); }
        protected async override Task<IEnumerable<T>> RefreshItemsAsync(CancellationToken token)
        {
            Debug("RefreshItemsAsync"); 
            SelectedContent = null;
            return await CrudService.RefreshItemsAsync(token);
        }
    }
}