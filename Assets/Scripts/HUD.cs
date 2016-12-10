using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class HUD : Entity {
  public Text InteractTextHUD;

  public GameObject Player;

  private Interactor _playerInteractor;

  void Start() {
    _playerInteractor = Player.GetComponent<Interactor>();
  }

  void Update() {
    // interactor text

    var interactTarget = _playerInteractor.GetTarget();

    if (interactTarget != null) {
      InteractTextHUD.text = string.Format("Space: {0}", interactTarget.InteractVerb);
    } else {
      InteractTextHUD.text = string.Format("Space: {0}", "Probably nothing.");
    }
  }
}