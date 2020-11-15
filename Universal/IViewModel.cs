using System.ComponentModel;

namespace OnPoint.Universal
{
    // The base features of every view model.
    public interface IViewModel : INotifyPropertyChanged
    {
        string BusyMessage { get; }
        string ErrorMessage { get; }
        bool IsActivated { get; }
        bool IsBusy { get; }
        bool IsCancelEnabled { get; }
        bool IsEnabled { get; }
        bool IsShowingErrorMessage { get; }
        bool IsShowingHUDMessage { get; }
        bool IsVisible { get; }
        string HUDMessage { get; }
        string Title { get; }
        long ViewModelId { get; }
        uint ViewModelTypeId { get; }
    }
}