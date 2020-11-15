using Autofac;
using DynamicData;
using DynamicData.Binding;
using OnPoint.Universal;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OnPoint.ViewModels
{
    // An extension of MultiContentVM that reports when its contents have changed.
    public abstract class MultiContentWatchedVM<T> : MultiContentVM<T> where T : class, INotifyPropertyChanged
    {
        public MultiContentWatchedVM(ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default) : base(lifeTimeScope, viewModelTypeId, screen, urlPathSegment) { }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            Contents
                .ToObservableChangeSet()
                .ObserveOn(RxApp.MainThreadScheduler)
                .WhenAnyPropertyChanged("IsChanged")
                .Subscribe(x => ContentHasChanged(x))
                .DisposeWith(disposable);

            return await base.Activated(disposable);
        }

        protected virtual void ContentHasChanged(T item) { }
    }
}