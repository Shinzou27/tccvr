using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseExperienceController : MonoBehaviour
{
  [SerializeField] private ExperienceInfo experience;
  private readonly int maxConnection = 20;
  private const string FRUIT_SHOP = "FruitShop";
  private const string RESTAURANT_ORDER = "RestaurantOrder";
  public Action<EnterExperienceParams> UpdateEnterExperienceParams;

  async void Awake()
  {
    await UnityServices.InitializeAsync();
    await AuthenticationService.Instance.SignInAnonymouslyAsync();
  }

  public void GoToFruitShop()
  {
    GoToScene(FRUIT_SHOP);
  }

  public void GoToRestaurantOrder()
  {
    GoToScene(RESTAURANT_ORDER);
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
    Debug.LogError("Criando a sala, sou o primeiro a entrar");
    Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);
    string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    StoredNetworkData.allocation = allocation;
    StoredNetworkData.joinCode = joinCode;
    Debug.LogError(joinCode);
    EnterExperienceParams _params = new()
    {
      joinCode = joinCode,
    };
    UpdateEnterExperienceParams.Invoke(_params);
    EnterExperienceRequest.Instance.StartRequest(_params, callback);
  }
}
