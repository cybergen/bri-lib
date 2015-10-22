using System;
using System.Collections;
using System.Collections.Generic;

public static class StaticExtensions
{
    public static void ForEach(this IEnumerable list, Action<object> action)
    {
        for (var enumerator = list.GetEnumerator(); enumerator.MoveNext();)
        {
            action.Execute(enumerator.Current);
        }
    }

    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
        for (var enumerator = list.GetEnumerator(); enumerator.MoveNext();)
        {
            action.Execute(enumerator.Current);
        }
    }

    public static void Execute(this Action action)
    {
        if (action != null) { action(); }
    }

    public static void Execute<T>(this Action<T> action, T obj)
    {
        if (action != null) { action(obj); }
    }
}
