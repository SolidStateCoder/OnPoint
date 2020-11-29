using OnPoint.Universal;
using ReactiveUI;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// Models icons from https://github.com/MahApps/MahApps.Metro.IconPacks
    /// </summary>
    /// <typeparam name="TIcon">The type of the built in icon</typeparam>
    /// <typeparam name="TForeground">The color (Brush) of the icon</typeparam>
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

        public MahIconDetails(TIcon icon, TForeground foreground, double width, double height, GridPosition gridPosition, IconFlipOrientation flip, double rotation, bool spin, double spinDuration, object spinEasingFunction, bool spinAutoReverse, string toolTip)
            : base(width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip)
        {
            Icon = icon;
            LargeIcon = icon;
            Foreground = foreground;
        }

        public MahIconDetails(TIcon icon, TForeground foreground = default, double width = 16, double height = 16, GridPosition gridPosition = GridPosition.Fill, string toolTip = default)
            : this(icon, foreground, width, height, gridPosition, IconFlipOrientation.None, 0, false, 0, null, false, toolTip)
        {
        }
    }
}