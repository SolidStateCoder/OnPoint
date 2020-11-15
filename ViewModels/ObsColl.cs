using DynamicData;
using DynamicData.Binding;
using System;
using System.Collections.Generic;

namespace OnPoint.ViewModels
{
    // "ObsColl" because "ObservableCollectionExtendedExtended" seemed a bit much.
    public class ObsColl<T> : ObservableCollectionExtended<T>
    {
        public ObsColl()
        {
            this.ToObservableChangeSet()
                .Subscribe(x => CollectionHasChanged(x));
        }

        public ObsColl(params T[] items) : this()
        {
            AddRangeSilent(items);
        }

        public void AddRangeSilent(IEnumerable<T> items)
        {
            using (SuspendNotifications())
            {
                AddRange(items);
            }
        }

        public void ClearSilent()
        {
            using (SuspendNotifications())
            {
                Clear();
            }
        }

        public void ClearAndAddRangeSilent(IEnumerable<T> items)
        {
            using (SuspendNotifications())
            {
                Clear();
                if (items != null)
                {
                    AddRange(items);
                }
            }
        }

        protected virtual void CollectionHasChanged(IChangeSet<T> changeSet)
        {
        }
    }
}