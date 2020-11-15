using OnPoint.Universal;
using ReactiveUI;

namespace OnPoint.ViewModels
{
    public abstract class IconDetails : ReactiveObject
    {
        public IconFlipOrientation Flip { get => _Flip; set => this.RaiseAndSetIfChanged(ref _Flip, value); }
        private IconFlipOrientation _Flip = IconFlipOrientation.Normal;

        public double Rotation { get => _Rotation; set => this.RaiseAndSetIfChanged(ref _Rotation, value); }
        private double _Rotation = default;

        public bool Spin { get => _Spin; set => this.RaiseAndSetIfChanged(ref _Spin, value); }
        private bool _Spin = false;

        public double SpinDuration { get => _SpinDuration; set => this.RaiseAndSetIfChanged(ref _SpinDuration, value); }
        private double _SpinDuration = default;

        public object SpinEasingFunction { get => _SpinEasingFunction; set => this.RaiseAndSetIfChanged(ref _SpinEasingFunction, value); }
        private object _SpinEasingFunction = default;

        public bool SpinAutoReverse { get => _SpinAutoReverse; set => this.RaiseAndSetIfChanged(ref _SpinAutoReverse, value); }
        private bool _SpinAutoReverse = false;

        public double Width { get => _Width; set => this.RaiseAndSetIfChanged(ref _Width, value); }
        private double _Width = default;

        public double Height { get => _Height; set => this.RaiseAndSetIfChanged(ref _Height, value); }
        private double _Height = default;

        public GridPosition GridPosition { get => _GridPosition; set => this.RaiseAndSetIfChanged(ref _GridPosition, value); }
        private GridPosition _GridPosition = GridPosition.Fill;

        public IconDetails() { }

        public IconDetails(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public IconDetails(double width, double height, GridPosition gridPosition, IconFlipOrientation flip, double rotation, bool spin, double spinDuration, object spinEasingFunction, bool spinAutoReverse)
            : this(width, height)
        {
            GridPosition = gridPosition;
            Flip = flip;
            Rotation = rotation;
            Spin = spin;
            SpinDuration = spinDuration;
            SpinEasingFunction = spinEasingFunction;
            SpinAutoReverse = spinAutoReverse;
        }
    }
}