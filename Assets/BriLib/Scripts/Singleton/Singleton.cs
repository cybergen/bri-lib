using UnityEngine;

namespace BriLib
{
    public abstract class Singleton<T> : MonoBehaviour, ISingleton where T : UnityEngine.Component, ISingleton
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject(typeof(T).GetType().ToString());
                    obj.AddComponent<T>();
                    _instance = obj.GetComponent<T>();
                    _instance.OnCreate();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private static T _instance;

        private void Awake()
        {
            Instance = gameObject.GetComponent<T>();
            Instance.OnCreate();
        }

        public abstract void OnCreate();

        public abstract void Begin();

        public abstract void End();
    }
}
