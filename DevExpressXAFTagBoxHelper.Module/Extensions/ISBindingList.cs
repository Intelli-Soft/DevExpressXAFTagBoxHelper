using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpressXAFTagBoxHelper.Module.Extensions
{
    public class ISBindingList<T> : BindingList<T> where T : class

    {
        public delegate void RemovedDelegate(T deletedItem);
        public delegate void RemovingDelegate(T deletingItem, CancelEventArgs eventArgs);

        public event RemovedDelegate Removed;
        public event RemovingDelegate Removing;

        protected override void RemoveItem(int itemIndex)
        {
            var locCancel = false;
            T locDeletedItem = this.Items[itemIndex];

            if (Removing != null)
            {
               var locCancelEventArgs = new CancelEventArgs();
               Removing(locDeletedItem, locCancelEventArgs);
               locCancel = locCancelEventArgs.Cancel;

            }
            if (locCancel == false)
            {
                base.RemoveItem(itemIndex);
                if (Removed != null)
                    Removed(locDeletedItem);
            }
        }

    }
}
