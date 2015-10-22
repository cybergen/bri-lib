using System;
using System.Collections;
using System.Collections.Generic;

public interface IObservableCollection : IList, IObservable
{
    Action<int, object> OnAdded { get; set; }
    Action<int, object> OnRemoved { get; set; }
    Action<int, object, object> OnReplaced { get; set; }
    Action OnCleared { get; set; }

    IObservableCollection Map(Func<object, object> mapper);
    IObservableCollection Filter(Func<object, bool> filter);
    T Reduce<T>(T seed, Action<object, T> reducer);
    IObservableCollection Union(IObservableCollection other);
    IObservableCollection Sort(Func<object, object, int> comparer);
}

public interface IObservableCollection<T> : 
    IList<T>, 
    IObservable<IObservableCollection<T>>, 
    IObservableCollection
{
    new Action<int, T> OnAdded { get; set; }
    new Action<int, T> OnRemoved { get; set; }
    new Action<int, T, T> OnReplaced { get; set; }
    new Action OnCleared { get; set; }

    IObservableCollection<L> Map<L>(Func<T, L> mapper);
    IObservableCollection<T> Filter(Func<T, bool> filter);
    K Reduce<K>(K seed, Action<T, K> reducer);
    IObservableCollection<T> Union(IObservableCollection<T> other);
    IObservableCollection<T> Sort(Func<T, T, int> comparer);
}