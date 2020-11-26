using DynamicData;
using DynamicData.Binding;
using Splat;
using System;
using System.Collections.Generic;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// Adds the ability to call <see cref="SuspendNotifications"/> automatically.
    /// Named "ObsColl" because "ObservableCollectionExtendedExtended" seemed a bit much.  ;-)
    /// </summary>
    /// <typeparam name="T">The type of items being stored</typeparam>
    public class ObsColl<T> : ObservableCollectionExtended<T>, IEnableLogger
    {
        public ObsColl()
        {
            this.ToObservableChangeSet()
                .Subscribe(x => CollectionHasChanged(x));
        }

        public ObsColl(params T[] items) : this() => AddRangeSilent(items);

        public void AddRangeSilent(IEnumerable<T> items)
        {
            this.Log().Debug("AddRangeSilent");
            using (SuspendNotifications())
            {
                AddRange(items);
            }
        }

        public void ClearSilent()
        {
            this.Log().Debug("ClearSilent");
            using (SuspendNotifications())
            {
                Clear();
            }
        }

        public void ClearAndAddRange(IEnumerable<T> items)
        {
            this.Log().Debug("ClearAndAddRange");
            Clear();
            if (items != null)
            {
                AddRange(items);
            }
        }

        public void ClearAndAddRangeSilent(IEnumerable<T> items)
        {
            this.Log().Debug("ClearAndAddRangeSilent");
            using (SuspendNotifications())
            {
                ClearAndAddRange(items);
            }
        }

        protected virtual void CollectionHasChanged(IChangeSet<T> changeSet) { }
    }
}