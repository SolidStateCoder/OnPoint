﻿using Autofac;
using DynamicData;
using DynamicData.Binding;
using OnPoint.Universal;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// An extension of <see cref="MultiContentVM"/> that reports when its contents have changed.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="MultiContentVM.Contents"/></typeparam>
    public class MultiContentIsChangedVM<T> : MultiContentVM<T> where T : class, IIsChanged
    {
        public bool HasChangedContents { get => _HasChangedContents; set => this.RaiseAndSetIfChanged(ref _HasChangedContents, value); }
        private bool _HasChangedContents = default;

        protected IObservable<bool> WhenHasChanges_NotBusy { get; }

        public MultiContentIsChangedVM(ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default) : base(lifeTimeScope, viewModelTypeId, screen, urlPathSegment)
        {
            WhenHasChanges_NotBusy = this.WhenAny(vm => vm.IsBusy, vm => vm.HasChangedContents, (x, y) => !x.Value && y.Value);
        }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            Contents
                .ToObservableChangeSet()
                .ObserveOn(RxApp.MainThreadScheduler)
                .WhenAnyPropertyChanged(nameof(IIsChanged.IsChanged))
                .Subscribe(x => ContentHasChanged(x))
                .DisposeWith(disposable);

            return await base.Activated(disposable);
        }

        protected virtual void ContentHasChanged(T item) => HasChangedContents = true;
    }
}