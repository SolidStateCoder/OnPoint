using MahApps.Metro.IconPacks;
using OnPoint.Universal;
using OnPoint.ViewModels;
using OnPoint.WpfDotNet5;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OnPoint.WpfTestApp
{
    public class PeopleVM : CrudServiceVM<Person>
    {
        public PeopleVM(ICrudService<Person> crudService) : base(crudService)
        {
            Title = "People";
        }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            var result = await CrudService.RefreshItemsAsync(CancellationToken.None);
            Contents.AddAll(result.ToArray());
            return await base.Activated(disposable);
        }

        protected override IObservable<bool> CanSaveItem() => this.WhenAnyValue(vm => vm.SelectedContent, vm => vm.SelectedContent.IsChanged, vm => vm.IsBusy, (x, y,z) => x != null && y && !z);

        protected override List<Expression<Func<Person, bool>>> GetSearchCriteria()
        {
            var retVal = base.GetSearchCriteria();
            string searchTerm = SearchTerm.ToLower();
            retVal.Add(x => x.FirstName.ToLower().Contains(searchTerm) || x.LastName.ToLower().Contains(searchTerm));
            return retVal;
        }

        protected override IconDetails GetCreateNewItemIconDetails() => new MaterialIconDetails(PackIconMaterialKind.Plus, new SolidColorBrush(Colors.DarkGoldenrod), 12, 12);
        protected override IconDetails GetDeleteItemIconDetails() => new BootstrapIconsIconDetails(PackIconBootstrapIconsKind.Trash, new SolidColorBrush(Colors.Maroon), 12, 12);
        protected override IconDetails GetSaveItemIconDetails() => new FontAwesomeIconDetails(PackIconFontAwesomeKind.SaveRegular, new SolidColorBrush(Colors.DarkGreen), 12, 12);
        protected override IconDetails GetSaveChangedItemsIconDetails() => new BoxIconsIconDetails(PackIconBoxIconsKind.RegularSave, new SolidColorBrush(Colors.DarkGreen), 12, 12);
        protected override IconDetails GetRefreshItemsIconDetails() => new PackIconEntypoDetails(PackIconEntypoKind.Download, new SolidColorBrush(Colors.DarkGreen), 12, 12);
        protected override IconDetails GetSearchItemsIconDetails() => new EvaIconDetails(PackIconEvaIconsKind.Search, new SolidColorBrush(Colors.DarkBlue), 12, 12);
    }
}