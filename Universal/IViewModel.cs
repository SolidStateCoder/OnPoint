using System.ComponentModel;

namespace OnPoint.Universal
{
    /// <summary>
    /// The base features of every view model in the OnPoint library.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged, IIsSelected
    {
        /// <summary>
        /// Text intended to be displayed when the view model is busy.
        /// </summary>
        string BusyMessage { get; }

        /// <summary>
        /// Indicates the view model is busy.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Text intended to be displayed when an error occurs.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Indicates the view model is showing <see cref="ErrorMessage"/>.
        /// </summary>
        bool IsShowingErrorMessage { get; }

        /// <summary>
        /// Text intended to be shown to the user.
        /// </summary>
        string HUDMessage { get; }

        /// <summary>
        /// Indicates the view model is showing <see cref="HUDMessage"/>.
        /// </summary>
        bool IsShowingHUDMessage { get; }

        /// <summary>
        /// Indicates the view model has been activated by Reactive UI.
        /// </summary>
        bool IsActivated { get; }

        /// <summary>
        /// Indicates the Cancel command is available.
        /// </summary>
        bool IsCancelEnabled { get; }

        /// <summary>
        /// Indicates the view model is enabled on the UI.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Indicates the view model is visible on the UI.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Headline text to display to the user.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// A runtime specific unique ID for this view model.
        /// </summary>
        long ViewModelId { get; }

        /// <summary>
        /// Type type of the view model; can be mapped to an enumeration.
        /// </summary>
        uint ViewModelTypeId { get; }
    }
}