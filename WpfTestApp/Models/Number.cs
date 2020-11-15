using ReactiveUI;

namespace OnPoint.WpfTestApp
{
    public class Number : ReactiveObject
    {
        public int Value { get => _Value; set => this.RaiseAndSetIfChanged(ref _Value, value); }
        private int _Value = default;

        public Number(int number)
        {
            Value = number;
        }

        public override string ToString() => $"The # '{Value}'";
    }
}