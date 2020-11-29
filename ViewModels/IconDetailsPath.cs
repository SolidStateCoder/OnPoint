using OnPoint.Universal;
using ReactiveUI;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// Models an icon or image that is referenced by a file path; example: pack://application:,,,/OnPointWpfTestApp;component/Images/Bullseye64.png
    /// </summary>
    public class IconDetailsPath : IconDetails
    {
        /// <summary>
        /// The path to the normal, or small, version of the image.
        /// </summary>
        public string IconPath { get => _IconPath; set => this.RaiseAndSetIfChanged(ref _IconPath, value); }
        private string _IconPath = default;

        /// <summary>
        /// The path to the large version of the image.
        /// </summary>
        public string LargeIconPath { get => _LargeIconPath; set => this.RaiseAndSetIfChanged(ref _LargeIconPath, value); }
        private string _LargeIconPath = default;

        public IconDetailsPath() { }

        public IconDetailsPath(string iconPath, double width, double height, string largeIconPath = default) : base(width, height)
        {
            IconPath = iconPath;
            LargeIconPath = largeIconPath ?? IconPath;
        }

        public IconDetailsPath(string iconPath, double width, double height, GridPosition gridPosition, IconFlipOrientation flip, double rotation, bool spin, double spinDuration, object spinEasingFunction, bool spinAutoReverse, string toolTip, string largeIconPath = default)
            : base(width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip)
        {
            IconPath = iconPath;
            LargeIconPath = largeIconPath ?? IconPath;
        }
    }
}