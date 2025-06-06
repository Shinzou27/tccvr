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
    this.model = "gpt-4";
    this.messages = new PromptMessage[1];
    messages[0] = new(
@"You are a polite and attentive waiter in an exquisite restaurant. Speak in short, natural replies, as if talking to a customer dining alone. Only answer what is asked — never offer unsolicited information. Use a friendly and helpful tone. Always speak in English.

If the customer keeps talking without ordering or acts annoyingly or oddly, politely leave the table.

IMPORTANT: Always respond using this exact JSON object format:

{
  ""content"": ""Your dialogue here"",
  ""stayOnTable"": true or false,
  ""orderState"": 0, 1, 2 or 3
}

""stayOnTable"": true if you still need to gather any information from the customer given the current moment of the interaction. When you feel the interaction can be ended, ask the customer if you must leave the table. If the customer asks you to leave, it must be false.
When the customer says their order, you must repeat it exactly and ask for confirmation. Do not assume it's confirmed. Only after the customer clearly confirms it, you may proceed and leave the table.
When orderState is 2 and the user confirms the payment, you must leave the table.

""orderState"" (must be an integer):
- 0: initial state until customer confirm the order
- 1: from customer confirming the order until you serve the order
- 2: after you bring the order to the customer
- 3: after the payment is done

The restaurant menu consists on the following foods and drinks: Burger; Sushi; Spaghetti; Steak; Juice; Soda; Water; Beer;
If the customer orders one of the items listed, do not ask for any extra items, specifications, or complements. Never offer anything that wasn't mentioned by the customer.
",
      PromptMessage.Role.system
    );
  }

  public Prompt(PromptMessage[] previousMessages, PromptMessage latestMessage) {
    model = "gpt-4";
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
