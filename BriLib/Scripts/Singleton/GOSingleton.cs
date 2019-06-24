using UnityEngine;

namespace BriLib
{
  /// <summary>
  /// A form of singleton where we expect some prefab or game object to exist in world. If the game object does not yet exist,
  /// we automatically make it. If the game object DOES exist on create, we bail out,
  /// as we should not attempt to create more than one of a singleton
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class GOSingleton<T> : MonoBehaviour, ISingleton where T : Component, ISingleton
  {
    public static T Instance
    {
      get
      {
        if (_instance == null)
        {
          var obj = new GameObject(typeof(T).GetType().ToString());
          obj.AddComponent<T>();
          obj.name = typeof(T).ToString();
          _instance = obj.GetComponent<T>();
        }
        return _instance;
      }
    }

    private static T _instance;

    private void Awake()
    {
      if (_instance != null)
      {
        throw new DuplicateInstanceException("Attempted to initialize already initialized singleton " + typeof(T));
      }
      _instance = gameObject.GetComponent<T>();
      _instance.OnCreate();
    }

    private void Start()
    {
      Begin();
    }

    public virtual void OnCreate()
    {
      DontDestroyOnLoad(gameObject);
    }

    public virtual void Begin() { }

    public virtual void End()
    {
      _instance = null;
      Destroy(gameObject);
    }
  }
}
