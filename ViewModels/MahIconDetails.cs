using OnPoint.Universal;
using ReactiveUI;

namespace OnPoint.ViewModels
{
    public class MahIconDetails<TIcon, TForeground> : IconDetails where TForeground : class
    {
        public TIcon Icon { get => _Icon; set => this.RaiseAndSetIfChanged(ref _Icon, value); }
        private TIcon _Icon = default;

        public TIcon LargeIcon { get => _LargeIcon; set => this.RaiseAndSetIfChanged(ref _LargeIcon, value); }
        private TIcon _LargeIcon = default;

        public TForeground Foreground { get => _Foreground; set => this.RaiseAndSetIfChanged(ref _Foreground, value); }
        private TForeground _Foreground = default;

        public TForeground Background { get => _Background; set => this.RaiseAndSetIfChanged(ref _Background, value); }
        private TForeground _Background = default;

        public MahIconDetails() { }

        public MahIconDetails(TIcon icon, TForeground foreground, double width, double height, GridPosition gridPosition, IconFlipOrientation flip, double rotation, bool spin, double spinDuration, object spinEasingFunction, bool spinAutoReverse)
            : base(width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse)
        {
            Icon = icon;
            LargeIcon = icon;
            Foreground = foreground;
        }

        public MahIconDetails(TIcon icon, TForeground foreground = default, double width = 16, double height = 16, GridPosition gridPosition = GridPosition.Fill)
            : this(icon, foreground, width, height, gridPosition, IconFlipOrientation.Normal, 0, false, 0, null, false)
        {
        }
    }
}