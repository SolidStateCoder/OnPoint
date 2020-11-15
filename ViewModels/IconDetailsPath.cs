using OnPoint.Universal;
using ReactiveUI;

namespace OnPoint.ViewModels
{
    public class IconDetailsPath : IconDetails
    {
        public string IconPath { get => _IconPath; set => this.RaiseAndSetIfChanged(ref _IconPath, value); }
        private string _IconPath = default;

        public string LargeIconPath { get => _LargeIconPath; set => this.RaiseAndSetIfChanged(ref _LargeIconPath, value); }
        private string _LargeIconPath = default;

        public IconDetailsPath() { }

        public IconDetailsPath(string iconPath, double width, double height, string largeIconPath = default) : base(width, height)
        {
            IconPath = iconPath;
            LargeIconPath = largeIconPath ?? IconPath;
        }

        public IconDetailsPath(string iconPath, double width, double height, GridPosition gridPosition, IconFlipOrientation flip, double rotation, bool spin, double spinDuration, object spinEasingFunction, bool spinAutoReverse, string largeIconPath = default)
            : base(width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse)
        {
            IconPath = iconPath;
            LargeIconPath = largeIconPath ?? IconPath;
        }
    }
}