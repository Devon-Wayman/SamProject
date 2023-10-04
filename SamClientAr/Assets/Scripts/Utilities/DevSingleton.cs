using UnityEngine;

public class DevSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"{objs.Length} instances of {typeof(T).Name} found");
#endif

                if (objs.Length > 0) instance = objs[0];

                if (instance == null)
                {
                    GameObject obj = new GameObject() { hideFlags = HideFlags.HideAndDontSave };
                    instance = obj.AddComponent<T>();
                }

            }
            return instance;
        }

    }
}

public class DevSingletonPersistent<T> : MonoBehaviour where T : Component
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }
    }
}
