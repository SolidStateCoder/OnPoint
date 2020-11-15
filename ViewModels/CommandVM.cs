using Autofac;
using ReactiveUI;
using System.Windows.Input;

namespace OnPoint.ViewModels
{
    public class CommandVM : IconVM
    {
        public ICommand Command { get => _Command; set => this.RaiseAndSetIfChanged(ref _Command, value); }
        private ICommand _Command = default;

        public object CommandParameter { get => _CommandParameter; set => this.RaiseAndSetIfChanged(ref _CommandParameter, value); }
        private object _CommandParameter = default;

        public string CommandText { get => _CommandText; set => this.RaiseAndSetIfChanged(ref _CommandText, value); }
        private string _CommandText = default;

        public string ToolTip { get => _ToolTip; set => this.RaiseAndSetIfChanged(ref _ToolTip, value); }
        private string _ToolTip = default;

        public bool IsSelected { get => _IsSelected; set => this.RaiseAndSetIfChanged(ref _IsSelected, value); }
        private bool _IsSelected = default;

        public int Width { get => _Width; set => this.RaiseAndSetIfChanged(ref _Width, value); }
        private int _Width = default;

        public int Height { get => _Height; set => this.RaiseAndSetIfChanged(ref _Height, value); }
        private int _Height = default;

        public int SortOrder { get => _SortOrder; set => this.RaiseAndSetIfChanged(ref _SortOrder, value); }
        private int _SortOrder = default;

        public CommandVM(ICommand command = default, int width = default, int height = default, string commandText = default, IconDetails iconDetails = default, string toolTip = default, object commandParameter = default,
            ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default)
            : base(iconDetails, lifeTimeScope, viewModelTypeId, screen, urlPathSegment)
        {
            Command = command;
            CommandText = commandText;
            IconDetails = iconDetails;
            ToolTip = toolTip;
            CommandParameter = commandParameter;
            Width = width;
            Height = height;
        }
    }
}