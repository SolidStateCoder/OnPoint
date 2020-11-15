using Autofac;
using DynamicData;
using DynamicData.Binding;
using FluentAssertions;
using OnPoint.Universal;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace OnPoint.ViewModels
{
    // A view model with multiple children and helper methods to maintain the collection.
    public abstract class MultiContentVM<T> : ViewModelBase
    {
        public ObsColl<T> Contents { get; } = new ObsColl<T>();
        public ObsColl<T> SelectedContents { get; } = new ObsColl<T>();

        public T SelectedContent { get => _SelectedContent; set => this.RaiseAndSetIfChanged(ref _SelectedContent, value); }
        private T _SelectedContent = default;

        protected IObservable<bool> WhenSelected { get; }
        protected IObservable<bool> WhenSelected_NotBusy { get; }
        protected IObservable<bool> WhenAnyContents { get; }

        public MultiContentVM() : this(null, 0, null, null) { }

        public MultiContentVM(ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default) : base(lifeTimeScope, viewModelTypeId, screen, urlPathSegment)
        {
            WhenSelected = this.WhenAny(vm => vm.SelectedContent, x => x.Value != null);
            WhenSelected_NotBusy = this.WhenAny(vm => vm.SelectedContent, vm => vm.IsBusy, (x, y) => x.Value != null && !y.Value);
            WhenAnyContents = this.WhenAny(vm => vm.Contents.Count, x => x.Value > 0);
            Contents.CollectionChanged += Contents_CollectionChanged;
        }

        protected override ExecutionResultMessage Activated(CompositeDisposable disposable)
        {
            Contents
                .ToObservableChangeSet()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => ContentsChanged(x))
                .DisposeWith(disposable);

            this.WhenAnyValue(vm => vm.SelectedContent)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SelectedContentChanged);

            return base.Activated(disposable);
        }

        private void Contents_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ContentsAdded(args.NewItems.Cast<T>().ToList());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    ContentsRemoved(args.OldItems.Cast<T>().ToList());
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ContentsCleared();
                    break;
            }
        }

        #region Contents

        protected virtual void ContentsRemoved(List<T> oldContents) { }

        protected virtual void ContentsAdded(IList<T> newContents) { }

        protected virtual void ContentsCleared() { }

        protected virtual void ContentsChanged(IChangeSet<T> changeSet) { }

        protected virtual T SelectOrCreateContent(Func<T> contentCreator, bool allowDuplicate = false)
        {
            contentCreator.Should().NotBeNull();

            Type type = typeof(T);
            T content = default;
            if (!allowDuplicate)
            {
                content = Contents.FirstOrDefault(x => x.GetType() == type);
            }
            //
            if (content == null)
            {
                content = contentCreator();
                AddContent(content);
            }
            SelectedContent = content;
            return content;
        }

        public virtual void AddAndSelectContent(T content)
        {
            AddContent(content);
            SelectedContent = content;
        }

        public virtual T AddContent(T content)
        {
            Contents.Add(content);
            return content;
        }

        public virtual void AddContents(IEnumerable<T> contents)
        {
            contents.Should().NotBeNull();
            AddContents(contents.ToArray());
        }

        public virtual void AddContents(params T[] contents)
        {
            contents.Should().NotBeNull();

            foreach (var content in contents)
            {
                Contents.Add(content);
            }
        }

        public virtual void ClearAndAddContents(params T[] contents)
        {
            ClearContents();
            AddContents(contents);
        }

        public virtual void ClearAndAddContents(IEnumerable<T> contents)
        {
            ClearAndAddContents(contents.ToArray());
        }

        public virtual void RemoveContent(T content, bool selectNext = true)
        {
            if (Contents.Contains(content))
            {
                bool changeSelection = false;
                int index = Contents.IndexOf(content);
                if (EqualityComparer<T>.Default.Equals(SelectedContent, content))
                {
                    SelectedContent = default;
                    changeSelection = true;
                }
                //
                Contents.Remove(content);
                //
                if (selectNext && changeSelection && Contents.Count > 0)
                {
                    if (index < Contents.Count)
                    {
                        SelectedContent = Contents[index];
                    }
                    else
                    {
                        SelectedContent = Contents.Last();
                    }
                }
            }
        }

        public virtual void RemoveContents(Func<T, bool> filter)
        {
            filter.Should().NotBeNull();

            List<T> toRemove = Contents.Where(filter).ToList();
            foreach (T content in toRemove)
            {
                RemoveContent(content);
            }
        }

        public virtual void ClearContents()
        {
            Contents.Clear();
        }

        public virtual bool ReplaceContent(T oldCopy, T newCopy)
        {
            bool retVal = false;
            if (Contents.Contains(oldCopy) && newCopy != null)
            {
                int index = Contents.IndexOf(oldCopy);
                RemoveContent(oldCopy, false);
                Contents.Insert(index, newCopy);
            }
            return retVal;
        }

        public virtual bool ReplaceContent(T newCopy)
        {
            bool retVal = false;
            T item = Contents.FirstOrDefault(x => x?.Equals(newCopy) == true);
            if (item != null)
            {
                retVal = ReplaceContent(item, newCopy);
            }
            return retVal;
        }

        #endregion

        protected virtual void SelectedContentChanged(T item) { }
    }
}