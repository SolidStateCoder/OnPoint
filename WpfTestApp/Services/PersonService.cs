using LinqKit;
using MahApps.Metro.IconPacks;
using OnPoint.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace OnPoint.WpfTestApp
{
    public class PersonService : ICrudService<Person>
    {
        private List<Person> _People = new List<Person>();
        private int _IconIndex = 14;

        public PersonService()
        {
            _People.Add(new Person("John", "Doe", (PackIconMaterialKind)_IconIndex++));
        }

        public async Task<Person> CreateNewItemAsync(CancellationToken token)
        {
            await Task.Delay(2000, token);
            return new Person() { Icon = (PackIconMaterialKind)_IconIndex++, IsChanged = true };
        }

        public bool DeleteItem(Person person)
        {
            bool retVal = false;
            lock (_People)
            {
                if (_People.Contains(person))
                {
                    _People.Remove(person);
                }
            }
            return retVal;
        }

        public IEnumerable<Person> RefreshItems() => _People;

        public IEnumerable<Person> SaveItems(IEnumerable<Person> people)
        {
            foreach (Person person in people)
            {
                person.IsChanged = false;
            }
            return _People;
        }

        public IEnumerable<Person> SearchItems(params Expression<Func<Person, bool>>[] filters)
        {
            List<Person> retVal = new List<Person>();
            if (filters != null)
            {
                lock (_People)
                {
                    retVal = _People.Where(GeneratePredicate(true, filters).Compile()).ToList();
                }
            }
            return retVal;
        }

        protected Expression<Func<Person, bool>> GeneratePredicate(bool isAnd, params Expression<Func<Person, bool>>[] filters)
        {
            var retVal = PredicateBuilder.New<Person>(true);
            if (filters != null)
            {
                foreach (var filter in filters.Where(x => x != null))
                {
                    if (isAnd)
                    {
                        retVal = PredicateBuilder.And(retVal, filter);
                    }
                    else
                    {
                        retVal = PredicateBuilder.Or(retVal, filter);
                    }
                }
            }
            return retVal;
        }
    }
}