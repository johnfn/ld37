using UnityEngine;
using UnityEngine.UI;
using Zenject;

[DisallowMultipleComponent]
public class Interactor : Entity {
  [Inject]
  public IGroups Groups { get; set; }

  void Update() {
    Debug.Log("Hello");

    Debug.Log(Groups.ClosestTo(gameObject));
  }
}