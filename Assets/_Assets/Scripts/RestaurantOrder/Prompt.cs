using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class PromptMessage {
  [JsonConverter(typeof(StringEnumConverter))]
  public enum Role { system, assistant, user }

  public string content;
  public Role role;

  public PromptMessage(string content, Role role) {
    this.content = content;
    this.role = role;
  }
}

[Serializable]
public class Prompt {
  public string model;
  public PromptMessage[] messages;

  public Prompt() {
    this.model = "gpt-3.5-turbo";
    this.messages = new PromptMessage[1];
    messages[0] = new(
      "You are a polite and attentive waiter in an exquisite restaurant. Keep your replies short and natural, as if you're speaking to a customer who is dining alone. Don't provide information unless asked. You should respond in a friendly and helpful tone. Speak strictly in English.",
      PromptMessage.Role.system
    );
    Debug.Log("AAA");
  }

  public Prompt(PromptMessage[] previousMessages, PromptMessage latestMessage) {
    model = "gpt-3.5-turbo";
    messages = new PromptMessage[previousMessages.Length + 1];
    for(int i = 0; i < previousMessages.Length; i++) {
      messages[i] = previousMessages[i];
    }
    messages[^1] = latestMessage;
  }

  public string ToJSON() {
    return JsonConvert.SerializeObject(this);
  }
}
