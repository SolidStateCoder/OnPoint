using Autofac;
using OnPoint.Universal;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace OnPoint.ViewModels
{
    // A standard VM that has one child.
    public abstract class ContentVM<T> : ViewModelBase
    {
        public T Content { get => _Content; set => this.RaiseAndSetIfChanged(ref _Content, value); }
        private T _Content = default;

        protected IObservable<bool> WhenContentNull { get; }
        protected IObservable<bool> WhenContentNotNull { get; }
        protected IObservable<bool> WhenContentNull_NotBusy { get; }
        protected IObservable<bool> WhenContentNotNull_NotBusy { get; }

        public ContentVM() { }

        public ContentVM(T content) : this(content, null, 0, null, null) { }

        public ContentVM(T content = default, ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default) : base(lifeTimeScope, viewModelTypeId, screen, urlPathSegment)
        {
            Content = content;
            WhenContentNull = this.WhenAny(vm => vm.Content, x => x.Value == null);
            WhenContentNotNull = this.WhenAny(vm => vm.Content, x => x.Value != null);
            WhenContentNull_NotBusy = this.WhenAny(vm => vm.Content, vm => vm.IsBusy, (x, y) => x.Value == null && !y.Value);
            WhenContentNotNull_NotBusy = this.WhenAny(vm => vm.Content, vm => vm.IsBusy, (x, y) => x.Value != null && !y.Value);
        }

        protected override ExecutionResultMessage Activated(CompositeDisposable disposable)
        {
            this.WhenAnyValue(vm => vm.Content)
                .Subscribe(ContentHasChanged);

            return base.Activated(disposable);
        }

        protected virtual void ContentHasChanged(T content) { }
    }
}