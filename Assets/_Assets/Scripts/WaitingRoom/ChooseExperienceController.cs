using System;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseExperienceController : MonoBehaviour
{
  [SerializeField] private ExperienceInfo experience;
  // [SerializeField] private TextMeshProUGUI errorLog;
  private const string FRUIT_SHOP = "FruitShop";
  private const string RESTAURANT_ORDER = "RestaurantOrder";
  public Action<EnterExperienceParams> UpdateEnterExperienceParams;
  private bool clicked = false;

  async void Awake()
  {
    await UnityServices.InitializeAsync();
    await AuthenticationService.Instance.SignInAnonymouslyAsync();
  }

  public void GoToFruitShop()
  {
    if (!clicked) {
      clicked = true;
      GoToScene(FRUIT_SHOP);
    }
  }

  public void GoToRestaurantOrder()
  {
    if (!clicked) {
      clicked = true;
      GoToScene(RESTAURANT_ORDER);
    }
  }

  public void GoToScene(string scene)
  {
    Utils.isHost = experience.response.joinCode == null;
    if (Utils.isHost)
    {
      CreateRelay(() => SceneManager.LoadSceneAsync(scene));
    }
    else
    {
      StoredNetworkData.joinCode = experience.response.joinCode;
      SceneManager.LoadSceneAsync(scene);
    }
  }

  private async void CreateRelay(Action callback)
  {
    await StoredNetworkData.CreateRelay();
    Utils.OnlineHandle(() =>
    {
      EnterExperienceParams _params = new()
      {
        joinCode = StoredNetworkData.joinCode,
      };
      UpdateEnterExperienceParams.Invoke(_params);
      EnterExperienceRequest.Instance.StartRequest(_params, callback, error => Debug.LogError(error));
    });
    Utils.OfflineHandle(callback);
  }
}
