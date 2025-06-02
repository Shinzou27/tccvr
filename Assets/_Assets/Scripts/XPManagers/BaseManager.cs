using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private IEnumerator CountdownToLeave(int time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadSceneAsync("WaitingRoom");
    }
    public void OnEndSession(int time)
    {
        StartCoroutine(CountdownToLeave(time));
    }
}