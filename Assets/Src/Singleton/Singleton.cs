using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected bool dontDestroyOnLoad = true;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    instance = new GameObject().AddComponent<T>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (dontDestroyOnLoad)
            {
                var root = transform.root;
                if (root != transform)
                {
                    DontDestroyOnLoad(root);
                }
                else
                {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}