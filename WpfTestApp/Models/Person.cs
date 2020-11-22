using OnPoint.ViewModels;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System;
using MahApps.Metro.IconPacks;

namespace OnPoint.WpfTestApp
{
    public class Person : ChangeableObject
    {
        public string FirstName { get => _FirstName; set => this.RaiseAndSetIfChanged(ref _FirstName, value); }
        private string _FirstName = default;

        public string LastName { get => _LastName; set => this.RaiseAndSetIfChanged(ref _LastName, value); }
        private string _LastName = default;

        public PackIconMaterialKind Icon { get => _Icon; set => this.RaiseAndSetIfChanged(ref _Icon, value); }
        private PackIconMaterialKind _Icon = default;

        public Person()
        {
            Observable.Merge
            (
                this.WhenAnyValue(x => x.FirstName).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.LastName).Select(_ => Unit.Default),
                this.WhenAnyValue(x => x.Icon).Select(_ => Unit.Default)
            )
            .Subscribe(_ => IsChanged = true);
        }

        public Person(string firstName, string lastName, PackIconMaterialKind icon) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Icon = icon;
            IsChanged = false;
        }

        public override string ToString() => $"{LastName}, {FirstName}";
    }
}