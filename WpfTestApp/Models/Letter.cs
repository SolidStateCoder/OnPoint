using ReactiveUI;

namespace OnPoint.WpfTestApp
{
    public class Letter : ReactiveObject
    {
        public string Value { get => _Value; set => this.RaiseAndSetIfChanged(ref _Value, value); }
        private string _Value = default;

        public Letter(string letter)
        {
            Value = letter;
        }

        public override string ToString() => $"The letter '{Value}'";
    }
}