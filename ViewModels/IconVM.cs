using Autofac;
using ReactiveUI;

namespace OnPoint.ViewModels
{
    public class IconVM : ViewModelBase
    {
        public IconDetails IconDetails { get => _IconDetails; set => this.RaiseAndSetIfChanged(ref _IconDetails, value); }
        private IconDetails _IconDetails = default;

        public IconVM(IconDetails iconDetails) : this(iconDetails, null, 0, null, null) { }

        public IconVM(IconDetails iconDetails = default, ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default) : base(lifeTimeScope, viewModelTypeId, screen, urlPathSegment)
        {
            IconDetails = iconDetails;
        }
    }
}