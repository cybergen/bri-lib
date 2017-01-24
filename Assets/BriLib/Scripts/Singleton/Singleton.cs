using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : UnityEngine.Component
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
    }
}
