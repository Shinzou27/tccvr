using System;
[Serializable]
public class PromptMessage {
  public enum Role {SYSTEM, ASSISTANT, USER}
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
    this.messages = new PromptMessage[1];
  }
  public Prompt(PromptMessage[] previousMessages, PromptMessage latestMessage) {
    this.messages = new PromptMessage[previousMessages.Length + 1];
    for(int i = 0; i < this.messages.Length; i++) {
      this.messages[i] = previousMessages[i];
    }
    this.messages[^1] = latestMessage;
  }
}