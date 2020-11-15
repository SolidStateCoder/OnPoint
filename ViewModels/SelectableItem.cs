using ReactiveUI;

namespace OnPoint.ViewModels
{
    public class SelectableItem<T> : ReactiveObject
    {
        public T Item { get => _Item; set => this.RaiseAndSetIfChanged(ref _Item, value); }
        private T _Item = default;

        public bool IsSelected { get => _IsSelected; set => this.RaiseAndSetIfChanged(ref _IsSelected, value); }
        private bool _IsSelected = default;

        public string DisplayName { get => _DisplayName; set => this.RaiseAndSetIfChanged(ref _DisplayName, value); }
        private string _DisplayName = default;

        public SelectableItem(T item, string displayName = default, bool isSelected = false)
        {
            Item = item;
            DisplayName = displayName ?? Item?.ToString();
            IsSelected = isSelected;
        }

        public override string ToString() => DisplayName;
    }
}