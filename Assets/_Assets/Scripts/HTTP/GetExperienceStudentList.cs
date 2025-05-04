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
  public void StartRequest(string pin, Action callback)
  {
    StartCoroutine(Perform(pin, callback));
  }
  private IEnumerator Perform(string pin, Action callback)
  {
    string url = "http://localhost:3000/experience/getOne";
    var request = new UnityWebRequest(url, "GET");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(@"{""pin"": """ + pin + @"""}");
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    // request.SetRequestHeader("Authorization", "Bearer " + API.apiKey);

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
      ExperienceResponse response = JsonConvert.DeserializeObject<ExperienceResponse>(request.downloadHandler.text);
      GetComponent<ExperienceInfo>().response = response;
      callback.Invoke();
    } else {
      Debug.Log(request.error);
    }
  }
}
