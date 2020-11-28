using Autofac;
using ReactiveUI;
using System.Windows.Input;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// Used to represent an <see cref="ICommand"/> on the UI; usually bound to a button.
    /// </summary>
    public class CommandVM : IconVM
    {
        /// <summary>
        /// The command to execute or bind; usually bound to a Button.
        /// </summary>
        public ICommand Command { get => _Command; set => this.RaiseAndSetIfChanged(ref _Command, value); }
        private ICommand _Command = default;

        /// <summary>
        /// The parameter for the command binding.
        /// </summary>
        public object CommandParameter { get => _CommandParameter; set => this.RaiseAndSetIfChanged(ref _CommandParameter, value); }
        private object _CommandParameter = default;

        /// <summary>
        /// The text to display for the command.
        /// </summary>
        public string CommandText { get => _CommandText; set => this.RaiseAndSetIfChanged(ref _CommandText, value); }
        private string _CommandText = default;

        /// <summary>
        /// Tooltip for the UI.
        /// </summary>
        public string ToolTip { get => _ToolTip; set => this.RaiseAndSetIfChanged(ref _ToolTip, value); }
        private string _ToolTip = default;

        /// <summary>
        /// The width of the UI element bound to the command.
        /// </summary>
        public int Width { get => _Width; set => this.RaiseAndSetIfChanged(ref _Width, value); }
        private int _Width = default;

        /// <summary>
        /// The height of the UI element bound to the command.
        /// </summary>
        public int Height { get => _Height; set => this.RaiseAndSetIfChanged(ref _Height, value); }
        private int _Height = default;

        /// <summary>
        /// The index the command should be placed in.
        /// </summary>
        public int SortOrder { get => _SortOrder; set => this.RaiseAndSetIfChanged(ref _SortOrder, value); }
        private int _SortOrder = default;

        /// <summary>
        /// Create a new CommandVM.
        /// </summary>
        public CommandVM(ICommand command = default, int width = default, int height = default, string commandText = default, IconDetails iconDetails = default, string toolTip = default, object commandParameter = default,
            ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default)
            : base(iconDetails, lifeTimeScope, viewModelTypeId, screen, urlPathSegment)
        {
            CommandText = Title = commandText;
            Command = command;
            IconDetails = iconDetails;
            ToolTip = toolTip;
            CommandParameter = commandParameter;
            Width = width;
            Height = height;
        }
    }
}