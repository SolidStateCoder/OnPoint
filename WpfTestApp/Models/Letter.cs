using ReactiveUI;

namespace OnPoint.WpfTestApp
{
    public class Letter : ReactiveObject
    {
        public char Value { get => _Value; set => this.RaiseAndSetIfChanged(ref _Value, value); }
        private char _Value = default;

        public Letter(char letter)
        {
            Value = letter;
        }

        public override string ToString() => $"The letter '{Value}'";
    }
}