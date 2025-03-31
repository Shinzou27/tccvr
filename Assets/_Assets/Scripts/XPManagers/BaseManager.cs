using UnityEngine;

public class BaseManager<T> : MonoBehaviour where T : BaseManager<T>
{
    public static T Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}