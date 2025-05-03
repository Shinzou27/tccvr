
using System;

[Serializable]
public class AssistantResponse
{
  public string content;
  public bool stayOnTable;
  public int orderState;
}

[Serializable]
public class OpenAIResponse
{
    public Choice[] choices;
}

[Serializable]
public class Choice
{
    public Message message { get; set; }
}

[Serializable]
public class Message
{
    public string content { get; set; }
}
