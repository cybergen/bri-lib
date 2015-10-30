using System;
using System.Collections;
using System.Collections.Generic;

namespace BriLib
{
    public interface IObservableCollection : IList, IDisposable
    {
        Action<int, object> OnAdded { get; set; }
        Action<int, object> OnRemoved { get; set; }
        Action<int, object, object> OnReplaced { get; set; }
        Action OnCleared { get; set; }

        IObservableCollection Map(Func<object, object> mapper);
        IObservableCollection Filter(Func<object, bool> filter);
        T ReduceNonGeneric<T>(T seed, Func<object, T, T> reducer);
        IObservableCollection Union(IObservableCollection other);
        IObservableCollection Sort(Func<object, object, int> comparer);
    }

    public interface IObservableCollection<T> : IList<T>, IObservableCollection
    {
        new Action<int, T> OnAdded { get; set; }
        new Action<int, T> OnRemoved { get; set; }
        new Action<int, T, T> OnReplaced { get; set; }
        new Action OnCleared { get; set; }

        IObservableCollection<L> Map<L>(Func<T, L> mapper);
        IObservableCollection<T> Filter(Func<T, bool> filter);
        K Reduce<K>(K seed, Func<T, K, K> reducer);
        IObservableCollection<T> Union(IObservableCollection<T> other);
        IObservableCollection<T> Sort(Func<T, T, int> comparer);
    }
}
