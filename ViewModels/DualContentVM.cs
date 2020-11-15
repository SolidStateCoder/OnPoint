using Autofac;
using OnPoint.Universal;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OnPoint.ViewModels
{
    // A standard VM that has one child.
    public abstract class DualContentVM<T,U> : ContentVM<T>
    {
        public U Content2 { get => _Content2; set => this.RaiseAndSetIfChanged(ref _Content2, value); }
        private U _Content2 = default;

        protected IObservable<bool> WhenContent2Null { get; }
        protected IObservable<bool> WhenContent2NotNull { get; }
        protected IObservable<bool> WhenContent2Null_NotBusy { get; }
        protected IObservable<bool> WhenContent2NotNull_NotBusy { get; }

        protected IObservable<bool> WhenBothNull { get; }
        protected IObservable<bool> WhenBothNotNull { get; }
        protected IObservable<bool> WhenBothNull_NotBusy { get; }
        protected IObservable<bool> WhenBothNotNull_NotBusy { get; }

        public DualContentVM(T content1, U content2) : this(content1, content2, null, 0, null, null) { }

        public DualContentVM(T content1 = default, U content2 = default, ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default) : base(content1,lifeTimeScope, viewModelTypeId, screen, urlPathSegment)
        {
            Content2 = content2;
            WhenContent2Null = this.WhenAny(vm => vm.Content2, x => x.Value == null);
            WhenContent2NotNull = this.WhenAny(vm => vm.Content2, x => x.Value != null);
            WhenContent2Null_NotBusy = this.WhenAny(vm => vm.Content2, vm => vm.IsBusy, (x, y) => x.Value == null && !y.Value);
            WhenContent2NotNull_NotBusy = this.WhenAny(vm => vm.Content2, vm => vm.IsBusy, (x, y) => x.Value != null && !y.Value);

            WhenBothNull = Observable.Merge(WhenContentNull, WhenContent2Null);
            WhenBothNotNull = Observable.Merge(WhenContentNotNull, WhenContent2NotNull);
            WhenBothNull_NotBusy = Observable.Merge(WhenContentNull_NotBusy, WhenContent2Null_NotBusy);
            WhenBothNotNull_NotBusy = Observable.Merge(WhenContentNotNull_NotBusy, WhenContent2NotNull_NotBusy);
        }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            this.WhenAnyValue(vm => vm.Content)
                .Subscribe(ContentHasChanged);

            return await base.Activated(disposable);
        }

        protected virtual void ContentHasChanged(T content) { }
    }
}