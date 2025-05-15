using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class GetExperienceStudentList : MonoBehaviour
{
  public static GetExperienceStudentList Instance;
  void Awake()
  {
    if (Instance == null)
    Instance = this;
  }
  public void StartRequest(string pin, Action callback, Action<string> errorHandler)
  {
    StartCoroutine(Perform(pin, callback, errorHandler));
  }
  private IEnumerator Perform(string pin, Action callback, Action<string> errorHandler)
  {
    string server = "https://tcc-backend-4khc.onrender.com";
    string url = $"{server}/experience/getOne?pin={pin}";
    var request = new UnityWebRequest(url, "GET");
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    // request.SetRequestHeader("Authorization", "Bearer " + API.apiKey);

    errorHandler(request.url);
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
      ExperienceResponse response = JsonConvert.DeserializeObject<ExperienceResponse>(request.downloadHandler.text);
      GetComponent<ExperienceInfo>().response = response;
      callback.Invoke();
      errorHandler(request.url);
    }
    else
    {
      errorHandler(request.error);
    }
  }
}
