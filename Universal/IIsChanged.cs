using System.ComponentModel;

namespace OnPoint.Universal
{
    public interface IIsChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Indicates the item has changed since its creation.
        /// </summary>
        bool IsChanged { get; set; }
    }
}