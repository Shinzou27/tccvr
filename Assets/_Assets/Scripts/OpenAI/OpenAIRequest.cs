public static class OpenAIRequest {
  public static void Perform(string userSpeech) {
    PromptMessage newMessage = new(userSpeech, PromptMessage.Role.USER);
    Prompt prompt = RestaurantOrder.Instance.UpdatePrompt(newMessage);
  }

}