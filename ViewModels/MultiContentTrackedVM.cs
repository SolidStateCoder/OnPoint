using Autofac;
using DynamicData;
using DynamicData.Binding;
using OnPoint.Universal;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// An extension of <see cref="MultiContentVM"/> that reports when its contents have changed.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="MultiContentVM.Contents"/></typeparam>
    public class MultiContentTrackedVM<T> : MultiContentVM<T> where T : class, IIsChanged
    {
        /// <summary>
        /// Set to true whenever a <see cref="Contents"/> <see cref="IIsChanged.IsChanged"/> property is set to true.
        /// </summary>
        public bool HasChangedContents { get => _HasChangedContents; set => this.RaiseAndSetIfChanged(ref _HasChangedContents, value); }
        private bool _HasChangedContents = default;

        protected IObservable<bool> WhenHasChanges_NotBusy { get; }

#if DEBUG
        private static string NL { get; } = Environment.NewLine;
        public override string DebugOutput => base.DebugOutput +
            $"HasChangedContents: {HasChangedContents}{NL}";
#endif

        public MultiContentTrackedVM(ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default) : base(lifeTimeScope, viewModelTypeId, screen, urlPathSegment)
        {
            WhenHasChanges_NotBusy = this.WhenAny(vm => vm.IsBusy, vm => vm.HasChangedContents, (busy, changed) => !busy.Value && changed.Value);
        }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            Contents
                .ToObservableChangeSet()
                .ObserveOn(RxApp.MainThreadScheduler)
                .WhenAnyPropertyChanged(nameof(IIsChanged.IsChanged))
                .Subscribe(x => ContentHasChanged(x))
                .DisposeWith(disposable);

#if DEBUG
            Observable.Merge(
                this.WhenAnyValue(x => x.HasChangedContents).Select(_ => Unit.Default)
                )
                .Subscribe(_ => this.RaisePropertyChanged(nameof(DebugOutput)))
                .DisposeWith(disposable);
#endif

            return await base.Activated(disposable);
        }

        protected virtual void ContentHasChanged(T item) => SetHasChangedContents();

        protected override void ContentsChanged(IChangeSet<T> changeSet)
        {
            base.ContentsChanged(changeSet);
            SetHasChangedContents();
        }

        private void SetHasChangedContents() => HasChangedContents = Contents.Any(x => x.IsChanged);
    }
}