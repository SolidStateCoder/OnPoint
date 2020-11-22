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

        public string SearchTerm { get => _SearchTerm; set => this.RaiseAndSetIfChanged(ref _SearchTerm, value); }
        private string _SearchTerm = default;

        public ReactiveCommand<Unit, T> AddNewItemCmd { get; private set; }
        public ReactiveCommand<T, bool> DeleteItemCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> SaveChangedItemsCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> RefreshItemsCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> SearchItemsCmd { get; private set; }

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
            AddNewItemCmdVM = new CommandVM(AddNewItemCmd, 70, 24, "Add", null, "Click this to add a new item");

            DeleteItemCmd = ReactiveCommand.CreateFromObservable((T x) =>
                CreateAsyncObservable(() => false, CanDeleteItemBeCancelled)
                , CanDeleteItem());
            DeleteItemCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DeleteItemComplete);
            DeleteItemCmd.IsExecuting.ToProperty(this, nameof(_IsDeletingItem));
            DeleteItemCmdVM = new CommandVM(DeleteItemCmd, 70, 24, "Delete", null, "Click this to delete the selected item");

            SaveChangedItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(() => (IEnumerable<T>)new List<T>(), CanSavingChangedItemsBeCancelled)
                , CanSaveChangedItems());
            SaveChangedItemsCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SaveChangedItemsComplete);
            SaveChangedItemsCmd.IsExecuting.ToProperty(this, nameof(_IsSavingChangedItems));
            SaveChangedItemsCmdVM = new CommandVM(DeleteItemCmd, 70, 24, "Save", null, "Click this to save the items that have changes");

            RefreshItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(RefreshItems, CanRefreshingItemsBeCancelled)
                , CanRefreshItems());
            RefreshItemsCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RefreshItemsComplete);
            RefreshItemsCmd.IsExecuting.ToProperty(this, nameof(_IsRefreshingItems));
            RefreshItemsCmdVM = new CommandVM(RefreshItemsCmd, 70, 24, "Refresh", null, "Click this to reload the items");

            SearchItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(() => SearchItems(), CanSearchingItemsBeCancelled)
                , CanSearchItems());
            SearchItemsCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SearchItemsComplete);
            SearchItemsCmd.IsExecuting.ToProperty(this, nameof(_IsSearchingItems));
            SearchItemsCmdVM = new CommandVM(SearchItemsCmd, 70, 24, "Search", null, "Click this to search the items");
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
            IList < IObservable<bool> > retVal = base.GetCancellableCommads();
            if (CanAddNewItemBeCancelled) retVal.Add(AddNewItemCmd.IsExecuting);
            if (CanDeleteItemBeCancelled) retVal.Add(DeleteItemCmd.IsExecuting);
            if (CanSavingChangedItemsBeCancelled) retVal.Add(SaveChangedItemsCmd.IsExecuting);
            if (CanRefreshingItemsBeCancelled) retVal.Add(RefreshItemsCmd.IsExecuting);
            if (CanSearchingItemsBeCancelled) retVal.Add(SearchItemsCmd.IsExecuting);
            return retVal;
        }

        protected virtual IObservable<bool> CanAddNewItem() => WhenNotBusy;
        protected virtual IObservable<bool> CanDeleteItem() => WhenSelected_NotBusy;
        protected virtual IObservable<bool> CanSaveChangedItems() => WhenHasChanges_NotBusy;
        protected virtual IObservable<bool> CanRefreshItems() => WhenNotBusy;
        protected virtual IObservable<bool> CanSearchItems() => WhenSearchTerm_NotBusy;

        protected virtual List<Expression<Func<T, bool>>[]> GetSearchCriteria() => new List<Expression<Func<T, bool>>[]>();

        protected virtual async Task<T> AddNewItemAsync(CancellationToken token) => default(T);
        protected virtual void AddNewItemComplete(T item) => Contents.Add(item);
        protected virtual bool DelectItem(T item) => false;
        protected virtual void DeleteItemComplete(bool result) => RefreshItemsCmd.Execute().Subscribe();
        protected virtual IEnumerable<T> SaveChangedItems() => new List<T>();
        protected virtual void SaveChangedItemsComplete(IEnumerable<T> items) { }
        protected virtual IEnumerable<T> RefreshItems() => new List<T>();
        protected virtual void RefreshItemsComplete(IEnumerable<T> items) => ClearAndAddContents(items);
        protected virtual IEnumerable<T> SearchItems(params Expression<Func<T, bool>>[] filters) => new List<T>();
        protected virtual void SearchItemsComplete(IEnumerable<T> items) => ClearAndAddContents(items);
    }
}