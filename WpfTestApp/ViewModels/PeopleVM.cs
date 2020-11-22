using OnPoint.Universal;
using OnPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OnPoint.WpfTestApp
{
    public class PeopleVM : CrudServiceVM<Person>
    {
        public PeopleVM(ICrudService<Person> crudService) : base(crudService)
        {
            Title = "People";

            Contents.AddAll(crudService.RefreshItems().ToArray());
        }

        protected override List<Expression<Func<Person, bool>>[]> GetSearchCriteria()
        {
            var retVal = base.GetSearchCriteria();
            return retVal;
        }
    }
}