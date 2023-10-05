// Author: Devon Wayman - 1/15/2022
using UnityEngine;

/// <summary>
/// Create a non-persistent singleton of the desired class that inherits from the method
/// </summary>
/// <typeparam name="T">T is a generic and allows us to pass any component in to create a singleton of it</typeparam>
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

                // If no instance of the requested singleton is found, create a game object in the scene that wont show
                // in the hieracrchy nor exist once game/play mode is exited, and add the class to it we have requested
                // to retreieve a singleton of
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

/// <summary>
/// Create persistent singleton reference; recommended for GUIManager, AudioManagers, etc.
/// </summary>
/// <typeparam name="T">T is a generic and allows us to pass any component in to create a singleton of it</typeparam>
public class DevSingletonPersistent<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}