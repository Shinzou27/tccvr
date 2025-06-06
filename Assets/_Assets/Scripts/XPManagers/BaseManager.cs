using System;
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

    private IEnumerator CountdownToLeave(int time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
        SceneManager.LoadSceneAsync("WaitingRoom");
    }
    public void OnEndSession(int time, Action callback)
    {
        StartCoroutine(CountdownToLeave(time, callback));
    }
}