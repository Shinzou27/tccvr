using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseConnectionMethod : MonoBehaviour
{
  void Update()
  {
    if (Input.GetKey(KeyCode.Escape)) {
        ChooseHost();
    }
  }
  public void ChooseHost() {
        Utils.isHost = true;
        SceneManager.LoadSceneAsync("FruitShop");
    }
    public void ChooseClient() {
        Utils.isHost = false;
        SceneManager.LoadSceneAsync("FruitShop");
    }
}
