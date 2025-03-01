using UnityEngine;

/// <summary>
/// --- Singleton Design Pattern ---
/// Singleton base class for single objects in the project. 
/// </summary>
/// <typeparam name="T">Type of object which is single in the project.</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected bool m_isDestroyOnLoad = true;
    /// <summary>
    /// Encapsulated field for the instance of T.
    /// </summary>
    private static T instance;

    /// <summary>
    /// Public field for the encapsulated field of instance.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<T>();
                if (!instance)
                {
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (!m_isDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
