using OnPoint.Universal;
using ReactiveUI;
using System;

namespace OnPoint.ViewModels
{
    public class ApplicationRootVM : ContentVM<IViewModel>
    {
        public double AppHeight { get => _AppHeight; set => this.RaiseAndSetIfChanged(ref _AppHeight, value); }
        private double _AppHeight = 0;

        public double AppWidth { get => _AppWidth; set => this.RaiseAndSetIfChanged(ref _AppWidth, value); }
        private double _AppWidth = 0;

        public double AppMinHeight { get => _AppMinHeight; set => this.RaiseAndSetIfChanged(ref _AppMinHeight, value); }
        private double _AppMinHeight = 0;

        public double AppMinWidth { get => _AppMinWidth; set => this.RaiseAndSetIfChanged(ref _AppMinWidth, value); }
        private double _AppMinWidth = 0;

        public double AppLeft { get => _AppLeft; set => this.RaiseAndSetIfChanged(ref _AppLeft, value); }
        private double _AppLeft = 0;

        public double AppTop { get => _AppTop; set => this.RaiseAndSetIfChanged(ref _AppTop, value); }
        private double _AppTop = 0;

        public virtual void UnhandledExceptionOccurred(Exception exception) { }
    }
}
