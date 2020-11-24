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
        private int _IdIndex = 1;

        public PersonService()
        {
            _People.Add(new Person(_IdIndex++, "John", "Doe", (PackIconMaterialKind)_IconIndex++));
        }

        public async Task<Person> CreateNewItemAsync(CancellationToken token)
        {
            await Task.Delay(1000, token);
            return new Person() { Icon = (PackIconMaterialKind)_IconIndex++, IsChanged = true };
        }

        public async Task<bool> DeleteItemAsync(CancellationToken token, Person person)
        {
            await Task.Delay(1000, token);
            bool retVal = false;
            lock (_People)
            {
                if (_People.Contains(person))
                {
                    _People.Remove(person);
                    retVal = true;
                }
            }
            return retVal;
        }

        public async Task<IEnumerable<Person>> RefreshItemsAsync(CancellationToken token)
        {
            await Task.Delay(1000, token);
            return _People.Where(x => x.ID > 0);
        }

        public async Task<Person> SaveItemAsync(CancellationToken token, Person person)
        {
            await Task.Delay(1000, token);
            if(person != null)
            {
                if(person.ID < 1)
                {
                    person.ID = _IdIndex++;
                }
                //
                if(!_People.Contains(person))
                {
                    _People.Add(person);
                }
                person.IsChanged = false;
            }
            return person;
        }

        public async Task<IEnumerable<Person>> SaveChangedItemsAsync(CancellationToken token, IEnumerable<Person> people)
        {
            await Task.Delay(1000, token);
            if (people != null)
            {
                foreach (Person person in people)
                {
                    if (person.ID < 1)
                    {
                        person.ID = _IdIndex++;
                    }
                    //
                    if (!_People.Contains(person))
                    {
                        _People.Add(person);
                    }
                    person.IsChanged = false;
                }
            }
            return _People;
        }

        public async Task<IEnumerable<Person>> SearchItemsAsync(CancellationToken token, params Expression<Func<Person, bool>>[] filters)
        {
            await Task.Delay(1000, token);
            List<Person> retVal = new List<Person>();
            if (filters != null)
            {
                lock (_People)
                {
                    retVal = _People.Where(GeneratePredicate(true, filters).Compile())?.ToList();
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