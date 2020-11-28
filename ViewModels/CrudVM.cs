using OnPoint.Universal;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// Models standard create, read, update, and delete functionality.
    /// </summary>
    /// <typeparam name="T">The type of the items being crud'ed</typeparam>
    public abstract class CrudVM<T> : MultiContentTrackedVM<T> where T : class, IIsChanged
    {
        /// <summary>
        /// Can be used as the search text entered by the user.
        /// </summary>
        public string SearchTerm { get => _SearchTerm; set => this.RaiseAndSetIfChanged(ref _SearchTerm, value); }
        private string _SearchTerm = default;

        #region Command VMs

        /// <summary>
        /// Models a button bound CreateNewItem.
        /// </summary>
        public CommandVM CreateNewItemVM { get => _CreateNewItem; set => this.RaiseAndSetIfChanged(ref _CreateNewItem, value); }
        private CommandVM _CreateNewItem = default;

        /// <summary>
        /// Models a button bound to DeleteItem.
        /// </summary>
        public CommandVM DeleteItemVM { get => _DeleteItemVM; set => this.RaiseAndSetIfChanged(ref _DeleteItemVM, value); }
        private CommandVM _DeleteItemVM = default;

        /// <summary>
        /// Models a button bound to SaveChangedItems.
        /// </summary>
        public CommandVM SaveChangedItemsVM { get => _SaveChangedItemsVM; set => this.RaiseAndSetIfChanged(ref _SaveChangedItemsVM, value); }
        private CommandVM _SaveChangedItemsVM = default;

        /// <summary>
        /// Models a button bound to SaveItem.
        /// </summary>
        public CommandVM SaveItemVM { get => _SaveItemVM; set => this.RaiseAndSetIfChanged(ref _SaveItemVM, value); }
        private CommandVM _SaveItemVM = default;

        /// <summary>
        /// Models a button bound to RefreshItems.
        /// </summary>
        public CommandVM RefreshItemsVM { get => _RefreshItemsVM; set => this.RaiseAndSetIfChanged(ref _RefreshItemsVM, value); }
        private CommandVM _RefreshItemsVM = default;

        /// <summary>
        /// Models a button bound to SearchItems.
        /// </summary>
        public CommandVM SearchItemsVM { get => _SearchItemsVM; set => this.RaiseAndSetIfChanged(ref _SearchItemsVM, value); }
        private CommandVM _SearchItemsVM = default;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, T> CreateNewItem { get; private set; }
        public ReactiveCommand<T, bool> DeleteItem { get; private set; }
        public ReactiveCommand<T, T> SaveItem { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> SaveChangedItems { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> RefreshItems { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> SearchItems { get; private set; }

        #endregion

        #region State

        public bool IsShowingSearchResults { get => _IsShowingSearchResults; set => this.RaiseAndSetIfChanged(ref _IsShowingSearchResults, value); }
        private bool _IsShowingSearchResults = default;

        public bool IsCreatingNewItem { get => _IsCreatingNewItem?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsCreatingNewItem = default;

        public bool IsDeletingItem { get => _IsDeletingItem?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsDeletingItem = default;

        public bool IsSaving { get => _IsSaving?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsSaving = default;

        public bool IsRefreshingItems { get => _IsRefreshingItems?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsRefreshingItems = default;

        public bool IsSearchingItems { get => _IsSearchingItems?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsSearchingItems = default;

        public virtual bool CanCreateNewItemBeCancelled => true;
        public virtual bool CanDeleteItemBeCancelled => true;
        public virtual bool CanSavingBeCancelled => true;
        public virtual bool CanRefreshingItemsBeCancelled => true;
        public virtual bool CanSearchingItemsBeCancelled => true;

        protected IObservable<bool> WhenSearchTerm_NotBusy { get; }
#if DEBUG
        private static string NL { get; } = Environment.NewLine;
        public override string DebugOutput => base.DebugOutput +
            $"IsCreatingNewItem: {IsCreatingNewItem}{NL}" +
            $"IsDeletingItem: {IsDeletingItem}{NL}" +
            $"IsSaving: {IsSaving}{NL}" +
            $"IsRefreshingItems: {IsRefreshingItems}{NL}" +
            $"IsSearchingItems: {IsSearchingItems}{NL}";
#endif

        #endregion

        #region Config

        public CrudVM()
        {
            WhenSearchTerm_NotBusy = this.WhenAnyValue(vm => vm.SearchTerm, vm => vm.IsBusy, (term, busy) => !term.IsNothing() && !busy);

            CreateNewItem = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(CreateNewItemAsync, CanCreateNewItemBeCancelled)
                , CanCreateNewItem());
            CreateNewItem
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(CreateNewItemComplete);
            CreateNewItem.IsExecuting.ToProperty(this, x => x.IsCreatingNewItem, out _IsCreatingNewItem);
            CreateNewItemVM = CreateCreateNewItemVM(CreateNewItem, 100, 24, "Create", "Click this to add a new item");

            DeleteItem = ReactiveCommand.CreateFromObservable((T x) =>
                CreateAsyncObservable(ct => DeleteItemAsync(ct, x), CanDeleteItemBeCancelled)
                , CanDeleteItem());
            DeleteItem
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DeleteItemComplete);
            DeleteItem.IsExecuting.ToProperty(this, x => x.IsDeletingItem, out _IsDeletingItem);
            DeleteItemVM = CreateDeleteItemVM(DeleteItem, 100, 24, "Delete", "Click this to delete the selected item");

            SaveChangedItems = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(ct => SaveChangedItemsAsync(ct), CanSavingBeCancelled)
                , CanSaveChangedItems());
            SaveChangedItems
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SaveChangedItemsComplete);
            SaveChangedItemsVM = CreateSaveChangedItemsVM(SaveChangedItems, 100, 24, "Save Changes", "Click this to save the items that have changes");

            SaveItem = ReactiveCommand.CreateFromObservable((T x) =>
                CreateAsyncObservable(ct => SaveItemAsync(ct, x), CanSavingBeCancelled)
                , CanSaveItem());
            SaveItem
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SaveItemComplete);
            SaveItemVM = CreateSaveItemVM(SaveItem, 100, 24, "Save Current", "Click this to save an item");
            Observable.Merge(SaveItem.IsExecuting, SaveChangedItems.IsExecuting).ToProperty(this, x => x.IsSaving, out _IsSaving);

            RefreshItems = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(RefreshItemsAsync, CanRefreshingItemsBeCancelled)
                , CanRefreshItems());
            RefreshItems
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RefreshItemsComplete);
            RefreshItems.IsExecuting.ToProperty(this, x => x.IsRefreshingItems, out _IsRefreshingItems);
            RefreshItemsVM = CreateRefreshItemsVM(RefreshItems, 100, 24, "Refresh", "Click this to reload the items");

            SearchItems = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(x => SearchItemsAsync(x, GetSearchCriteria().ToArray()), CanSearchingItemsBeCancelled)
                , CanSearchItems());
            SearchItems
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SearchItemsComplete);
            SearchItems.IsExecuting.ToProperty(this, x => x.IsSearchingItems, out _IsSearchingItems);
            SearchItemsVM = CreateSearchItemsVM(SearchItems, 100, 24, "Search", "Click this to search the items");
        }

        protected override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
#if DEBUG
            Observable.Merge(
                this.WhenAnyValue(x => x.IsCreatingNewItem).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.IsDeletingItem).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.IsSaving).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.IsRefreshingItems).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.IsSearchingItems).Select(_ => Unit.Default)
                )
                .Subscribe(_ => this.RaisePropertyChanged(nameof(DebugOutput)))
                .DisposeWith(disposable);
#endif

            return base.Activated(disposable);
        }

        protected override IList<IObservable<bool>> GetBusyCommands() => base.GetBusyCommands().AddAll(
            RefreshItems.IsExecuting, SaveChangedItems.IsExecuting, SaveItem.IsExecuting, DeleteItem.IsExecuting,
            CreateNewItem.IsExecuting, SearchItems.IsExecuting);

        protected override IList<IObservable<Exception>> GetAllCommandThrownExceptions() => base.GetAllCommandThrownExceptions().AddAll(
            RefreshItems.ThrownExceptions, SaveChangedItems.ThrownExceptions, SaveItem.ThrownExceptions, DeleteItem.ThrownExceptions,
            CreateNewItem.ThrownExceptions, SearchItems.ThrownExceptions);

        protected override IList<IObservable<bool>> GetCancellableCommads()
        {
            IList<IObservable<bool>> retVal = base.GetCancellableCommads();
            if (CanCreateNewItemBeCancelled) retVal.Add(CreateNewItem.IsExecuting);
            if (CanDeleteItemBeCancelled) retVal.Add(DeleteItem.IsExecuting);
            if (CanSavingBeCancelled) retVal.AddAll(SaveChangedItems.IsExecuting, SaveItem.IsExecuting);
            if (CanRefreshingItemsBeCancelled) retVal.Add(RefreshItems.IsExecuting);
            if (CanSearchingItemsBeCancelled) retVal.Add(SearchItems.IsExecuting);
            return retVal;
        }

        #endregion

        #region Command VM overrides

        protected virtual IObservable<bool> CanCreateNewItem() => WhenNotBusy;
        protected virtual IObservable<bool> CanDeleteItem() => WhenSelected_NotBusy;
        protected virtual IObservable<bool> CanSaveChangedItems() => WhenHasChanges_NotBusy;
        protected virtual IObservable<bool> CanSaveItem() => WhenNotBusy;
        protected virtual IObservable<bool> CanRefreshItems() => WhenNotBusy;
        protected virtual IObservable<bool> CanSearchItems() => WhenSearchTerm_NotBusy;

        protected virtual CommandVM CreateCreateNewItemVM(ReactiveCommand<Unit, T> addNewItemCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(addNewItemCmd, width, height, commandText, GetCreateNewItemIconDetails(), tooltip);

        protected virtual IconDetails GetCreateNewItemIconDetails() => null;

        protected virtual CommandVM CreateDeleteItemVM(ReactiveCommand<T, bool> deleteItemCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(deleteItemCmd, width, height, commandText, GetDeleteItemIconDetails(), tooltip);

        protected virtual IconDetails GetDeleteItemIconDetails() => null;

        protected virtual CommandVM CreateSaveChangedItemsVM(ReactiveCommand<Unit, IEnumerable<T>> saveChangedItemsCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(saveChangedItemsCmd, width, height, commandText, GetSaveChangedItemsIconDetails(), tooltip);

        protected virtual IconDetails GetSaveChangedItemsIconDetails() => null;

        protected virtual CommandVM CreateSaveItemVM(ReactiveCommand<T, T> saveItemCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(saveItemCmd, width, height, commandText, GetSaveItemIconDetails(), tooltip);

        protected virtual IconDetails GetSaveItemIconDetails() => null;

        protected virtual CommandVM CreateRefreshItemsVM(ReactiveCommand<Unit, IEnumerable<T>> refreshItemsCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(refreshItemsCmd, width, height, commandText, GetRefreshItemsIconDetails(), tooltip);

        protected virtual IconDetails GetRefreshItemsIconDetails() => null;

        protected virtual CommandVM CreateSearchItemsVM(ReactiveCommand<Unit, IEnumerable<T>> searchItemsCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(searchItemsCmd, width, height, commandText, GetSearchItemsIconDetails(), tooltip);

        protected virtual IconDetails GetSearchItemsIconDetails() => null;

        #endregion

        protected virtual List<Expression<Func<T, bool>>> GetSearchCriteria() => new List<Expression<Func<T, bool>>>();

        protected async virtual Task<T> CreateNewItemAsync(CancellationToken token) => await Task.Run(() => default(T));

        protected virtual void CreateNewItemComplete(T item)
        {
            Debug($"CreateNewItemComplete");
            Contents.Add(item);
            SelectedContent = item;
        }

        protected abstract Task<bool> DeleteItemAsync(CancellationToken token, T item);
        protected virtual void DeleteItemComplete(bool result)
        {
            Debug($"DeleteItemComplete");
            RefreshItems.Execute().Subscribe();
        }

        protected abstract Task<IEnumerable<T>> SaveChangedItemsAsync(CancellationToken token);
        protected virtual void SaveChangedItemsComplete(IEnumerable<T> items) => Debug("SaveChangedItemsComplete");

        protected abstract Task<T> SaveItemAsync(CancellationToken token, T item);
        protected virtual void SaveItemComplete(T item) => Debug($"SaveItemComplete");

        protected abstract Task<IEnumerable<T>> RefreshItemsAsync(CancellationToken token);
        protected virtual void RefreshItemsComplete(IEnumerable<T> items)
        {
            Debug($"RefreshItemsComplete");
            IsShowingSearchResults = false;
            ClearAndAddContents(items);
        }

        protected abstract Task<IEnumerable<T>> SearchItemsAsync(CancellationToken token, params Expression<Func<T, bool>>[] filters);
        protected virtual void SearchItemsComplete(IEnumerable<T> items)
        {
            Debug($"SearchItemsComplete");
            IsShowingSearchResults = true;
            ClearAndAddContents(items);
        }

        protected override string GetBusyOverrideMessage()
        {
            string retVal = null;
            if (IsCreatingNewItem) retVal = "Creating...";
            else if (IsDeletingItem) retVal = "Deleting...";
            else if (IsRefreshingItems) retVal = "Refreshing...";
            else if (IsSaving) retVal = "Saving...";
            else if (IsSearchingItems) retVal = "Searching...";
            return retVal;
        }
    }
}