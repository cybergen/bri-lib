using UnityEngine;

namespace BriLib
{
  /// <summary>
  /// A singleton that you can easily initialize direct from code
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class Singleton<T> : ISingleton where T : class, ISingleton, new()
  {
    public static T Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new T();
          _instance.OnCreate();
          _instance.Begin();
        }
        return _instance;
      }
      private set
      {
        _instance = value;
      }
    }

    private static T _instance;

    public virtual void OnCreate() { }

    public virtual void Begin() { }

    public virtual void End()
    {
      Instance = null;
    }
  }
}
