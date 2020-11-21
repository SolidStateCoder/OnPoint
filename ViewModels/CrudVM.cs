using OnPoint.Universal;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace OnPoint.ViewModels
{
    public abstract class CrudVM<T> : MultiContentIsChangedVM<T> where T : class, IIsChanged
    {
        public ReactiveCommand<Unit, T> AddNewItemCmd { get; private set; }
        public ReactiveCommand<T, bool> DeleteItemCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> SaveChangedItemsCmd { get; private set; }
        public ReactiveCommand<Unit, IEnumerable<T>> RefreshItemsCmd { get; private set; }

        public bool IsAddingNewItem { get => _IsAddingNewItem.Value; }
        private ObservableAsPropertyHelper<bool> _IsAddingNewItem = default;

        public bool IsDeletingItem { get => _IsDeletingItem.Value; }
        private ObservableAsPropertyHelper<bool> _IsDeletingItem = default;

        public bool IsSavingChangedItems { get => _IsSavingChangedItems.Value; }
        private ObservableAsPropertyHelper<bool> _IsSavingChangedItems = default;

        public bool IsRefreshingItems { get => _IsRefreshingItems.Value; }
        private ObservableAsPropertyHelper<bool> _IsRefreshingItems = default;

        public virtual bool CanAddNewItemBeCancelled => true;
        public virtual bool CanDeleteItemBeCancelled => true;
        public virtual bool CanSavingChangedItemsBeCancelled => true;
        public virtual bool CanRefreshingItemsBeCancelled => true;

        public CrudVM()
        { 
            AddNewItemCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(AddNewItem, CanAddNewItemBeCancelled)
                , CanAddNewItem());

            AddNewItemCmd.IsExecuting.ToProperty(this, nameof(_IsAddingNewItem));

            DeleteItemCmd = ReactiveCommand.CreateFromObservable((T x) =>
                CreateAsyncObservable(() => false, CanDeleteItemBeCancelled)
                , CanDeleteItem());

            DeleteItemCmd.IsExecuting.ToProperty(this, nameof(_IsDeletingItem));

            SaveChangedItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(() => (IEnumerable<T>)new List<T>(), CanSavingChangedItemsBeCancelled)
                , CanSaveChangedItems());

            SaveChangedItemsCmd.IsExecuting.ToProperty(this, nameof(_IsSavingChangedItems));

            RefreshItemsCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(RefreshItems, CanRefreshingItemsBeCancelled)
                , CanRefreshItems());

            RefreshItemsCmd.IsExecuting.ToProperty(this, nameof(_IsSavingChangedItems));


            //if (typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T))) // IIsChanged
            //{
            //    int x = 0;
            //}
        }

        protected virtual IObservable<bool> CanAddNewItem() => WhenNotBusy;
        protected virtual IObservable<bool> CanDeleteItem() => WhenSelected_NotBusy;
        protected virtual IObservable<bool> CanSaveChangedItems() => WhenHasChanges_NotBusy;
        protected virtual IObservable<bool> CanRefreshItems() => WhenNotBusy;

        protected virtual T AddNewItem() => default(T);
        protected virtual bool DelectItem(T item) => false;
        protected virtual IEnumerable<T> SaveChangedItems() => new List<T>();
        protected virtual IEnumerable<T> RefreshItems() => new List<T>();
    }
}