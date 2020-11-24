using MahApps.Metro.IconPacks;
using OnPoint.Universal;
using OnPoint.ViewModels;
using OnPoint.WpfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
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

        protected override List<Expression<Func<Person, bool>>> GetSearchCriteria()
        {
            return base.GetSearchCriteria();
        }

        protected override IconDetails GetAddNewItemIconDetails() => new MaterialIconDetails(PackIconMaterialKind.Plus, new SolidColorBrush(Colors.DarkGoldenrod), 12, 12);
        protected override IconDetails GetDeleteItemIconDetails() => new BootstrapIconsIconDetails(PackIconBootstrapIconsKind.Trash, new SolidColorBrush(Colors.Maroon), 12, 12);
        protected override IconDetails GetSaveChangedItemsIconDetails() => new BoxIconsIconDetails(PackIconBoxIconsKind.RegularSave, new SolidColorBrush(Colors.DarkGreen), 12, 12);
        protected override IconDetails GetRefreshItemsIconDetails() => new PackIconEntypoDetails(PackIconEntypoKind.Download, new SolidColorBrush(Colors.DarkGreen), 12, 12);
        protected override IconDetails GetSearchItemsIconDetails() => new EvaIconDetails(PackIconEvaIconsKind.Search, new SolidColorBrush(Colors.DarkBlue), 12, 12);
    }
}