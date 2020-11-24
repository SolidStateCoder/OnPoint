using MahApps.Metro.IconPacks;
using OnPoint.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace OnPoint.WpfTestApp
{
    public class Person : ChangeableObject, IEquatable<Person>
    {
        private static int _IdIndex = 0;

        public int ID { get => _ID; set => this.RaiseAndSetIfChanged(ref _ID, value); }
        private int _ID = default;

        public string FirstName { get => _FirstName; set => this.RaiseAndSetIfChanged(ref _FirstName, value); }
        private string _FirstName = default;

        public string LastName { get => _LastName; set => this.RaiseAndSetIfChanged(ref _LastName, value); }
        private string _LastName = default;

        public PackIconMaterialKind Icon { get => _Icon; set => this.RaiseAndSetIfChanged(ref _Icon, value); }
        private PackIconMaterialKind _Icon = default;

        public string DisplayName => ToString();

        public Person()
        {
            ID = _IdIndex--;

            Observable.Merge
            (
                this.WhenAnyValue(x => x.FirstName).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.LastName).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.Icon).Select(_ => Unit.Default)
            )
            .Subscribe(_ => { IsChanged = true; this.RaisePropertyChanged(nameof(DisplayName)); });
        }

        public Person(int id, string firstName, string lastName, PackIconMaterialKind icon) : this()
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Icon = icon;
            IsChanged = false;
        }

        public override string ToString() => $"{LastName}, {FirstName}";

        public bool Equals(Person other) => ID == other?.ID;
    }
}