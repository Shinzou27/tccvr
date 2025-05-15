using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class EnterExperienceRequest : MonoBehaviour
{
  public static EnterExperienceRequest Instance;
  public EnterExperienceResponse response;
  void Awake()
  {
    if (Instance == null)
    Instance = this;
  }
  public void StartRequest(EnterExperienceParams _params, Action callback, Action<string> errorHandler)
  {
    StartCoroutine(Perform(_params, callback, errorHandler));
  }
  private IEnumerator Perform(EnterExperienceParams _params, Action callback, Action<string> errorHandler)
  {
    string server = "";
    server = "https://tcc-backend-4khc.onrender.com";
    string url = $"{server}/experience/enter";
    byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_params));
    var request = UnityWebRequest.Put(url, bodyRaw);
    request.method = "PATCH";
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    Debug.Log(bodyRaw);
    Debug.Log(request.url);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    // request.SetRequestHeader("Authorization", "Bearer " + API.apiKey);

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
      response = JsonConvert.DeserializeObject<EnterExperienceResponse>(request.downloadHandler.text);
      Debug.Log($"código: {response.joinCode}");
      callback.Invoke();
    } else {
      errorHandler(request.error);
    }
  }
}

[Serializable]
public class EnterExperienceParams {
  public string pin;
  public string studentId;
  public string joinCode;
}

[Serializable]
public class EnterExperienceResponse {
  public string joinCode;
}