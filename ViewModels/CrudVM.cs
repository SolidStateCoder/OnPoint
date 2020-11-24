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

        public CommandVM AddNewItemCmdVM { get => _AddNewItemCmd; set => this.RaiseAndSetIfChanged(ref _AddNewItemCmd, value); }
        private CommandVM _AddNewItemCmd = default;

        public CommandVM DeleteItemCmdVM { get => _DeleteItemCmdVM; set => this.RaiseAndSetIfChanged(ref _DeleteItemCmdVM, value); }
        private CommandVM _DeleteItemCmdVM = default;

        public CommandVM SaveChangedItemsCmdVM { get => _SaveChangedItemsCmdVM; set => this.RaiseAndSetIfChanged(ref _SaveChangedItemsCmdVM, value); }
        private CommandVM _SaveChangedItemsCmdVM = default;

        public CommandVM RefreshItemsCmdVM { get => _RefreshItemsCmdVM; set => this.RaiseAndSetIfChanged(ref _RefreshItemsCmdVM, value); }
        private CommandVM _RefreshItemsCmdVM = default;

        public CommandVM SearchItemsCmdVM { get => _SearchItemsCmdVM; set => this.RaiseAndSetIfChanged(ref _SearchItemsCmdVM, value); }
        private CommandVM _SearchItemsCmdVM = default;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, T> AddNewItemCmd { get; private set; }
        public ReactiveCommand<T, bool> DeleteItemCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> SaveChangedItemsCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> RefreshItemsCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> SearchItemsCmd { get; private set; }

        #endregion

        #region State

        public bool IsAddingNewItem { get => _IsAddingNewItem?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsAddingNewItem = default;

        public bool IsDeletingItem { get => _IsDeletingItem?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsDeletingItem = default;

        public bool IsSavingChangedItems { get => _IsSavingChangedItems?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsSavingChangedItems = default;

        public bool IsRefreshingItems { get => _IsRefreshingItems?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsRefreshingItems = default;

        public bool IsSearchingItems { get => _IsSearchingItems?.Value ?? false; }
        private ObservableAsPropertyHelper<bool> _IsSearchingItems = default;

        public virtual bool CanAddNewItemBeCancelled => true;
        public virtual bool CanDeleteItemBeCancelled => true;
        public virtual bool CanSavingChangedItemsBeCancelled => true;
        public virtual bool CanRefreshingItemsBeCancelled => true;
        public virtual bool CanSearchingItemsBeCancelled => true;

        protected IObservable<bool> WhenSearchTerm_NotBusy { get; }
#if DEBUG
        private static string NL { get; } = Environment.NewLine;
        public override string DebugOutput => base.DebugOutput +
            $"IsAddingNewItem: {IsAddingNewItem}{NL}" +
            $"IsDeletingItem: {IsDeletingItem}{NL}" +
            $"IsSavingChangedItems: {IsSavingChangedItems}{NL}" +
            $"IsRefreshingItems: {IsRefreshingItems}{NL}" +
            $"IsSearchingItems: {IsSearchingItems}{NL}";
#endif

        #endregion

        #region Config

        public CrudVM()
        {
            WhenSearchTerm_NotBusy = this.WhenAnyValue(vm => vm.SearchTerm, vm => vm.IsBusy, (x, y) => !x.IsNothing() && !y);

            AddNewItemCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(AddNewItemAsync, CanAddNewItemBeCancelled)
                , CanAddNewItem());
            AddNewItemCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(AddNewItemComplete);
            AddNewItemCmd.IsExecuting.ToProperty(this, nameof(_IsAddingNewItem));
            AddNewItemCmdVM = CreateAddNewItemCmdVM(AddNewItemCmd, 90, 24, "Add", "Click this to add a new item");

            DeleteItemCmd = ReactiveCommand.CreateFromObservable((T x) =>
                CreateAsyncObservable(() => false, CanDeleteItemBeCancelled)
                , CanDeleteItem());
            DeleteItemCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DeleteItemComplete);
            DeleteItemCmd.IsExecuting.ToProperty(this, nameof(_IsDeletingItem));
            DeleteItemCmdVM = CreateDeleteItemCmdVM(DeleteItemCmd, 90, 24, "Delete", "Click this to delete the selected item");

            SaveChangedItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(() => (IEnumerable<T>)new List<T>(), CanSavingChangedItemsBeCancelled)
                , CanSaveChangedItems());
            SaveChangedItemsCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SaveChangedItemsComplete);
            SaveChangedItemsCmd.IsExecuting.ToProperty(this, nameof(_IsSavingChangedItems));
            SaveChangedItemsCmdVM = CreateSaveChangedItemsCmdVM(SaveChangedItemsCmd, 90, 24, "Save", "Click this to save the items that have changes");

            RefreshItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(RefreshItemsAsync, CanRefreshingItemsBeCancelled)
                , CanRefreshItems());
            RefreshItemsCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RefreshItemsComplete);
            RefreshItemsCmd.IsExecuting.ToProperty(this, nameof(_IsRefreshingItems));
            RefreshItemsCmdVM = CreateRefreshItemsCmdVM(RefreshItemsCmd, 90, 24, "Refresh", "Click this to reload the items");

            SearchItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(x => SearchItemsAsync(x, GetSearchCriteria().ToArray()), CanSearchingItemsBeCancelled)
                , CanSearchItems());
            SearchItemsCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SearchItemsComplete);
            SearchItemsCmd.IsExecuting.ToProperty(this, nameof(_IsSearchingItems));
            SearchItemsCmdVM = CreateSearchItemsCmdVM(SearchItemsCmd, 90, 24, "Search", "Click this to search the items");
        }

        protected override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
#if DEBUG
            Observable.Merge(
                this.WhenAnyValue(x => x.IsAddingNewItem).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.IsDeletingItem).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.IsSavingChangedItems).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.IsRefreshingItems).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.IsSearchingItems).Select(_ => Unit.Default)
                )
                .Subscribe(_ => this.RaisePropertyChanged(nameof(DebugOutput)))
                .DisposeWith(disposable);
#endif

            return base.Activated(disposable);
        }

        protected override IList<IObservable<bool>> GetBusyCommands() => base.GetBusyCommands().AddAll(
            RefreshItemsCmd.IsExecuting, SaveChangedItemsCmd.IsExecuting, DeleteItemCmd.IsExecuting,
            AddNewItemCmd.IsExecuting, SearchItemsCmd.IsExecuting);

        protected override IList<IObservable<Exception>> GetAllCommandThrownExceptions() => base.GetAllCommandThrownExceptions().AddAll(
            RefreshItemsCmd.ThrownExceptions, SaveChangedItemsCmd.ThrownExceptions, DeleteItemCmd.ThrownExceptions,
            AddNewItemCmd.ThrownExceptions, SearchItemsCmd.ThrownExceptions);

        protected override IList<IObservable<bool>> GetCancellableCommads()
        {
            IList<IObservable<bool>> retVal = base.GetCancellableCommads();
            if (CanAddNewItemBeCancelled) retVal.Add(AddNewItemCmd.IsExecuting);
            if (CanDeleteItemBeCancelled) retVal.Add(DeleteItemCmd.IsExecuting);
            if (CanSavingChangedItemsBeCancelled) retVal.Add(SaveChangedItemsCmd.IsExecuting);
            if (CanRefreshingItemsBeCancelled) retVal.Add(RefreshItemsCmd.IsExecuting);
            if (CanSearchingItemsBeCancelled) retVal.Add(SearchItemsCmd.IsExecuting);
            return retVal;
        }

        #endregion

        #region Command VM overrides

        protected virtual IObservable<bool> CanAddNewItem() => WhenNotBusy;
        protected virtual IObservable<bool> CanDeleteItem() => WhenSelected_NotBusy;
        protected virtual IObservable<bool> CanSaveChangedItems() => WhenHasChanges_NotBusy;
        protected virtual IObservable<bool> CanRefreshItems() => WhenNotBusy;
        protected virtual IObservable<bool> CanSearchItems() => WhenSearchTerm_NotBusy;

        protected virtual CommandVM CreateAddNewItemCmdVM(ReactiveCommand<Unit, T> addNewItemCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(addNewItemCmd, width, height, commandText, GetAddNewItemIconDetails(), tooltip);

        protected virtual IconDetails GetAddNewItemIconDetails() => null;

        protected virtual CommandVM CreateDeleteItemCmdVM(ReactiveCommand<T, bool> deleteItemCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(deleteItemCmd, width, height, commandText, GetDeleteItemIconDetails(), tooltip);

        protected virtual IconDetails GetDeleteItemIconDetails() => null;

        protected virtual CommandVM CreateSaveChangedItemsCmdVM(ReactiveCommand<Unit, IEnumerable<T>> saveChangedItemsCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(saveChangedItemsCmd, width, height, commandText, GetSaveChangedItemsIconDetails(), tooltip);

        protected virtual IconDetails GetSaveChangedItemsIconDetails() => null;

        protected virtual CommandVM CreateRefreshItemsCmdVM(ReactiveCommand<Unit, IEnumerable<T>> refreshItemsCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(refreshItemsCmd, width, height, commandText, GetRefreshItemsIconDetails(), tooltip);

        protected virtual IconDetails GetRefreshItemsIconDetails() => null;

        protected virtual CommandVM CreateSearchItemsCmdVM(ReactiveCommand<Unit, IEnumerable<T>> searchItemsCmd, int width, int height, string commandText, string tooltip) =>
            new CommandVM(searchItemsCmd, width, height, commandText, GetSearchItemsIconDetails(), tooltip);

        protected virtual IconDetails GetSearchItemsIconDetails() => null;

        #endregion

        protected virtual List<Expression<Func<T, bool>>> GetSearchCriteria() => new List<Expression<Func<T, bool>>>();

        protected virtual async Task<T> AddNewItemAsync(CancellationToken token)
        {
            BusyMessageOverride = "Adding...";
            return await Task.Run(() => default(T));
        }
        protected virtual void AddNewItemComplete(T item) => Contents.Add(item);

        protected async virtual Task<bool> DelectItemAsync(CancellationToken token, T item) => await Task.Run(() => false);
        protected virtual void DeleteItemComplete(bool result) => RefreshItemsCmd.Execute().Subscribe();

        protected async virtual Task<IEnumerable<T>> SaveChangedItemsAsync(CancellationToken token) => await Task.Run(() => new List<T>());
        protected virtual void SaveChangedItemsComplete(IEnumerable<T> items) { }

        protected async virtual Task<IEnumerable<T>> RefreshItemsAsync(CancellationToken token) => await Task.Run(() => new List<T>());
        protected virtual void RefreshItemsComplete(IEnumerable<T> items) => ClearAndAddContents(items);

        protected async virtual Task<IEnumerable<T>> SearchItemsAsync(CancellationToken token, params Expression<Func<T, bool>>[] filters) => await Task.Run(() => new List<T>());
        protected virtual void SearchItemsComplete(IEnumerable<T> items) => ClearAndAddContents(items);
    }
}