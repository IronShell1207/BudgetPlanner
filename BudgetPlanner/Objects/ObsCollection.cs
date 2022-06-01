using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Objects
{
    public class ObsCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {   
        public ObsCollection()
            : base()
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(ObsCollection_CollectionChanged);
        }

        public ObsCollection(List<T> list) : base( list)
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(ObsCollection_CollectionChanged);
        }
        void ObsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(a);
        }
    }
}
