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
    public abstract class CrudVM<T> : MultiContentIsChangedVM<T> where T : class, IIsChanged
    {
        public string SearchTerm { get => _SearchTerm; set => this.RaiseAndSetIfChanged(ref _SearchTerm, value); }
        private string _SearchTerm = default;

        #region Command VMs

        public CommandVM CreateNewItemCmdVM { get => _CreateNewItemCmd; set => this.RaiseAndSetIfChanged(ref _CreateNewItemCmd, value); }
        private CommandVM _CreateNewItemCmd = default;

        public CommandVM DeleteItemCmdVM { get => _DeleteItemCmdVM; set => this.RaiseAndSetIfChanged(ref _DeleteItemCmdVM, value); }
        private CommandVM _DeleteItemCmdVM = default;

        public CommandVM SaveChangedItemsCmdVM { get => _SaveChangedItemsCmdVM; set => this.RaiseAndSetIfChanged(ref _SaveChangedItemsCmdVM, value); }
        private CommandVM _SaveChangedItemsCmdVM = default;

        public CommandVM SaveItemCmdVM { get => _SaveItemCmdVM; set => this.RaiseAndSetIfChanged(ref _SaveItemCmdVM, value); }
        private CommandVM _SaveItemCmdVM = default;

        public CommandVM RefreshItemsCmdVM { get => _RefreshItemsCmdVM; set => this.RaiseAndSetIfChanged(ref _RefreshItemsCmdVM, value); }
        private CommandVM _RefreshItemsCmdVM = default;

        public CommandVM SearchItemsCmdVM { get => _SearchItemsCmdVM; set => this.RaiseAndSetIfChanged(ref _SearchItemsCmdVM, value); }
        private CommandVM _SearchItemsCmdVM = default;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, T> CreateNewItemCmd { get; private set; }
        public ReactiveCommand<T, bool> DeleteItemCmd { get; private set; }
        public ReactiveCommand<T, T> SaveItemCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> SaveChangedItemsCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> RefreshItemsCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> SearchItemsCmd { get; private set; }

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
            WhenSearchTerm_NotBusy = this.WhenAnyValue(vm => vm.SearchTerm, vm => vm.IsBusy, (x, y) => !x.IsNothing() && !y);

            CreateNewItemCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(CreateNewItemAsync, CanCreateNewItemBeCancelled)
                , CanCreateNewItem());
            CreateNewItemCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(CreateNewItemComplete);
            CreateNewItemCmd.IsExecuting.ToProperty(this, x => x.IsCreatingNewItem, out _IsCreatingNewItem);
            CreateNewItemCmdVM = CreateCreateNewItemCmdVM(CreateNewItemCmd, 100, 24, "Create", "Click this to add a new item");

            DeleteItemCmd = ReactiveCommand.CreateFromObservable((T x) =>
                CreateAsyncObservable(ct => DeleteItemAsync(ct, x), CanDeleteItemBeCancelled)
                , CanDeleteItem());
            DeleteItemCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DeleteItemComplete);
            DeleteItemCmd.IsExecuting.ToProperty(this, x => x.IsDeletingItem, out _IsDeletingItem);
            DeleteItemCmdVM = CreateDeleteItemCmdVM(DeleteItemCmd, 100, 24, "Delete", "Click this to delete the selected item");

            SaveChangedItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(ct => SaveChangedItemsAsync(ct), CanSavingBeCancelled)
                , CanSaveChangedItems());
            SaveChangedItemsCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SaveChangedItemsComplete);
            SaveChangedItemsCmdVM = CreateSaveChangedItemsCmdVM(SaveChangedItemsCmd, 100, 24, "Save Changes", "Click this to save the items that have changes");

            SaveItemCmd = ReactiveCommand.CreateFromObservable((T x) =>
                CreateAsyncObservable(ct => SaveItemAsync(ct, x), CanSavingBeCancelled)
                , CanSaveItem());
            SaveItemCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SaveItemComplete);
            SaveItemCmdVM = CreateSaveItemCmdVM(SaveItemCmd, 100, 24, "Save Current", "Click this to save an item");
            Observable.Merge(SaveItemCmd.IsExecuting, SaveChangedItemsCmd.IsExecuting).ToProperty(this, x => x.IsSaving, out _IsSaving);

            RefreshItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(RefreshItemsAsync, CanRefreshingItemsBeCancelled)
                , CanRefreshItems());
            RefreshItemsCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RefreshItemsComplete);
            RefreshItemsCmd.IsExecuting.ToProperty(this, x => x.IsRefreshingItems, out _IsRefreshingItems);
            RefreshItemsCmdVM = CreateRefreshItemsCmdVM(RefreshItemsCmd, 100, 24, "Refresh", "Click this to reload the items");

            SearchItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(x => SearchItemsAsync(x, GetSearchCriteria().ToArray()), CanSearchingItemsBeCancelled)
                , CanSearchItems());
            SearchItemsCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SearchItemsComplete);
            SearchItemsCmd.IsExecuting.ToProperty(this, x => x.IsSearchingItems, out _IsSearchingItems);
            SearchItemsCmdVM = CreateSearchItemsCmdVM(SearchItemsCmd, 100, 24, "Search", "Click this to search the items");
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
            RefreshItemsCmd.IsExecuting, SaveChangedItemsCmd.IsExecuting, SaveItemCmd.IsExecuting, DeleteItemCmd.IsExecuting,
            CreateNewItemCmd.IsExecuting, SearchItemsCmd.IsExecuting);

        protected override IList<IObservable<Exception>> GetAllCommandThrownExceptions() => base.GetAllCommandThrownExceptions().AddAll(
            RefreshItemsCmd.ThrownExceptions, SaveChangedItemsCmd.ThrownExceptions, SaveItemCmd.ThrownExceptions, DeleteItemCmd.ThrownExceptions,
            CreateNewItemCmd.ThrownExceptions, SearchItemsCmd.ThrownExceptions);

        protected override IList<IObservable<bool>> GetCancellableCommads()
        {
            IList<IObservable<bool>> retVal = base.GetCancellableCommads();
            if (CanCreateNewItemBeCancelled) retVal.Add(CreateNewItemCmd.IsExecuting);
            if (CanDeleteItemBeCancelled) retVal.Add(DeleteItemCmd.IsExecuting);
            if (CanSavingBeCancelled) retVal.AddAll(SaveChangedItemsCmd.IsExecuting, SaveItemCmd.IsExecuting);
            if (CanRefreshingItemsBeCancelled) retVal.Add(RefreshItemsCmd.IsExecuting);
            if (CanSearchingItemsBeCancelled) retVal.Add(SearchItemsCmd.IsExecuting);
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

        protected virtual CommandVM CreateCreateNewItemCmdVM(ReactiveCommand<Unit, T> addNewItemCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(addNewItemCmd, width, height, commandText, GetCreateNewItemIconDetails(), tooltip);

        protected virtual IconDetails GetCreateNewItemIconDetails() => null;

        protected virtual CommandVM CreateDeleteItemCmdVM(ReactiveCommand<T, bool> deleteItemCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(deleteItemCmd, width, height, commandText, GetDeleteItemIconDetails(), tooltip);

        protected virtual IconDetails GetDeleteItemIconDetails() => null;

        protected virtual CommandVM CreateSaveChangedItemsCmdVM(ReactiveCommand<Unit, IEnumerable<T>> saveChangedItemsCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(saveChangedItemsCmd, width, height, commandText, GetSaveChangedItemsIconDetails(), tooltip);

        protected virtual IconDetails GetSaveChangedItemsIconDetails() => null;

        protected virtual CommandVM CreateSaveItemCmdVM(ReactiveCommand<T, T> saveItemCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(saveItemCmd, width, height, commandText, GetSaveItemIconDetails(), tooltip);

        protected virtual IconDetails GetSaveItemIconDetails() => null;

        protected virtual CommandVM CreateRefreshItemsCmdVM(ReactiveCommand<Unit, IEnumerable<T>> refreshItemsCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(refreshItemsCmd, width, height, commandText, GetRefreshItemsIconDetails(), tooltip);

        protected virtual IconDetails GetRefreshItemsIconDetails() => null;

        protected virtual CommandVM CreateSearchItemsCmdVM(ReactiveCommand<Unit, IEnumerable<T>> searchItemsCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(searchItemsCmd, width, height, commandText, GetSearchItemsIconDetails(), tooltip);

        protected virtual IconDetails GetSearchItemsIconDetails() => null;

        #endregion

        protected virtual List<Expression<Func<T, bool>>> GetSearchCriteria() => new List<Expression<Func<T, bool>>>();

        protected virtual async Task<T> CreateNewItemAsync(CancellationToken token) => await Task.Run(() => default(T));

        protected virtual void CreateNewItemComplete(T item)
        {
            Contents.Add(item);
            SelectedContent = item;
        }

        protected async virtual Task<bool> DeleteItemAsync(CancellationToken token, T item) => await Task.Run(() => false);
        protected virtual void DeleteItemComplete(bool result) => RefreshItemsCmd.Execute().Subscribe();

        protected async virtual Task<IEnumerable<T>> SaveChangedItemsAsync(CancellationToken token) => await Task.Run(() => new List<T>());
        protected virtual void SaveChangedItemsComplete(IEnumerable<T> items) { }

        protected async virtual Task<T> SaveItemAsync(CancellationToken token, T item) => await Task.Run(() => default(T));
        protected virtual void SaveItemComplete(T item) { }

        protected async virtual Task<IEnumerable<T>> RefreshItemsAsync(CancellationToken token) => await Task.Run(() => new List<T>());
        protected virtual void RefreshItemsComplete(IEnumerable<T> items)
        {
            IsShowingSearchResults = false;
            ClearAndAddContents(items);
        }

        protected async virtual Task<IEnumerable<T>> SearchItemsAsync(CancellationToken token, params Expression<Func<T, bool>>[] filters) => await Task.Run(() => new List<T>());
        protected virtual void SearchItemsComplete(IEnumerable<T> items)
        {
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