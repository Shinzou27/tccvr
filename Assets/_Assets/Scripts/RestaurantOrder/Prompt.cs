using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;

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
@"You are a polite and attentive waiter in an exquisite restaurant. Speak in short, natural replies, as if talking to a customer dining alone. Only answer what is asked — never offer unsolicited information. Use a friendly and helpful tone. Always speak in English.

If the customer keeps talking without ordering, politely leave the table.

IMPORTANT: Always respond using this exact JSON object format:
{
  ""content"": ""Your dialogue here"",
  ""stayOnTable"": true or false,
  ""orderState"": 0, 1 or 2
}


""stayOnTable"": true if you still need to gather any information from the customer given the current moment of the interaction. Set to false if you should leave the table (e.g., go to the kitchen or balcony). If the customer asks you to leave, it also must be false.

""orderState"" (must be an integer):
- 0: welcoming phase
- 1: taking or confirming the order
- 2: after the order is served
",
      PromptMessage.Role.system
    );
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
    if (messages.Length < 2)
      return JsonConvert.SerializeObject(this);

    PromptMessage originalSystem = messages[0];
    StringBuilder extendedSystemContent = new();
    extendedSystemContent.AppendLine(originalSystem.content.Trim());
    extendedSystemContent.AppendLine();
    extendedSystemContent.AppendLine("All the conversation until now is described below. W stands for waiter and C stands for customer.");

    for (int i = 1; i < messages.Length - 1; i++) {
      PromptMessage msg = messages[i];
      char subject = msg.role == PromptMessage.Role.assistant ? 'W' : 'C';
      extendedSystemContent.AppendLine($"{subject}: {msg.content.Trim()}");
    }

    PromptMessage newSystemMessage = new (extendedSystemContent.ToString().Trim(), PromptMessage.Role.system);
    PromptMessage lastUserMessage = messages[^1];

    PromptMessage[] newMessages = new PromptMessage[] {
      newSystemMessage,
      lastUserMessage
    };

    Prompt newPrompt = new()
    {
      model = this.model,
      messages = newMessages
    };

    return JsonConvert.SerializeObject(newPrompt);
  }

}
