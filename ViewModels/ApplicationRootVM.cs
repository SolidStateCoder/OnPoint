using OnPoint.Universal;
using ReactiveUI;
using System;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// Models a top level view model that can be used to track an application's position. Useful mostly for desktop applications
    /// but can also be used for other devices.
    /// </summary>
    public class ApplicationRootVM : ContentVM<IViewModel>
    {
        /// <summary>
        /// The height of the app or window.
        /// </summary>
        public double AppHeight { get => _AppHeight; set => this.RaiseAndSetIfChanged(ref _AppHeight, value); }
        private double _AppHeight = 0;

        /// <summary>
        /// The width of the app or window.
        /// </summary>
        public double AppWidth { get => _AppWidth; set => this.RaiseAndSetIfChanged(ref _AppWidth, value); }
        private double _AppWidth = 0;

        /// <summary>
        /// The minimum legal height of the app or window.
        /// </summary>
        public double AppMinHeight { get => _AppMinHeight; set => this.RaiseAndSetIfChanged(ref _AppMinHeight, value); }
        private double _AppMinHeight = 0;

        /// <summary>
        /// The minimum legal width of the app or window.
        /// </summary>
        public double AppMinWidth { get => _AppMinWidth; set => this.RaiseAndSetIfChanged(ref _AppMinWidth, value); }
        private double _AppMinWidth = 0;

        /// <summary>
        /// The app or window's position from the left boundary.
        /// </summary>
        public double? AppLeft { get => _AppLeft; set => this.RaiseAndSetIfChanged(ref _AppLeft, value); }
        private double? _AppLeft = default;

        /// <summary>
        /// The app or window's position from the top boundary.
        /// </summary>
        public double? AppTop { get => _AppTop; set => this.RaiseAndSetIfChanged(ref _AppTop, value); }
        private double? _AppTop = default;

        /// <summary>
        /// The state the app or window is currently being displayed in.
        /// </summary>
        public AppDisplayState DisplayState { get => _DisplayState; set => this.RaiseAndSetIfChanged(ref _DisplayState, value); }
        private AppDisplayState _DisplayState = AppDisplayState.Normal;

        /// <summary>
        /// Services a point for the top most view model to handle unhandled exceptions.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> that occurred.</param>
        public virtual void UnhandledExceptionOccurred(Exception exception) { }
    }
}
