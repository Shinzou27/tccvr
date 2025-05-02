using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text;

public class OpenAIRequest : MonoBehaviour {
  void Start()
  {
    EventManager.Instance.OnPlayerFinishedSpeaking += Perform;
  }
  public void Perform(object sender, string userSpeech) {
    PromptMessage newMessage = new(userSpeech, PromptMessage.Role.user);
    Prompt prompt = RestaurantOrder.Instance.UpdatePrompt(newMessage);
    StartCoroutine(Post(prompt.ToJSON()));
  }
  private IEnumerator Post(string prompt) {
    Debug.Log(prompt);
    PromptMessage assistantResponse = new("Hello! It's friday. Who did it, did it.", PromptMessage.Role.assistant);
    RestaurantOrder.Instance.UpdatePrompt(assistantResponse);
    EventManager.Instance.OnOpenAIResponse?.Invoke(this, assistantResponse.content);
    yield return null;
    // string url = "https://api.openai.com/v1/chat/completions";
    // var request = new UnityWebRequest(url, "POST");
    // byte[] bodyRaw = Encoding.UTF8.GetBytes(prompt);
    // request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    // request.downloadHandler = new DownloadHandlerBuffer();
    // request.SetRequestHeader("Content-Type", "application/json");
    // request.SetRequestHeader("Authorization", "Bearer " + API.apiKey);

    // yield return request.SendWebRequest();

    // if (request.result == UnityWebRequest.Result.Success)
    // {
    //     Debug.Log("Resposta: " + request.downloadHandler.text);
    //     PromptMessage assistantResponse = new(request.downloadHandler.text, PromptMessage.Role.assistant);
    //     RestaurantOrder.Instance.UpdatePrompt(assistantResponse);
    //     EventManager.Instance.OnOpenAIResponse?.Invoke(this, assistantResponse.content);
    // }
    // else
    // {
    //     Debug.LogError("Erro: " + request.error);
    // }
  }
}