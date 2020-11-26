using OnPoint.Universal;
using ReactiveUI;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// An object that tracks if it has changed since its creation.
    /// Usage:
    /// public YourClass()
    /// {
    ///     System.Reactive.Linq.Observable.Merge
    ///     (
    ///         this.WhenAnyValue(x => x.Property1).Select(_ => Unit.Default),
    ///         this.WhenAnyValue(x => x.Property2).Select(_ => Unit.Default)
    ///     )
    ///     .Subscribe(_ => IsChanged = true);
    /// }
    /// </summary>
    public abstract class ChangeableObject : ReactiveObject, IIsChanged
    {
        /// <summary>
        /// Indicates this object has changed from a "business perspective".
        /// </summary>
        public bool IsChanged { get => _IsChanged; set => this.RaiseAndSetIfChanged(ref _IsChanged, value); }
        private bool _IsChanged = default;
    }
}