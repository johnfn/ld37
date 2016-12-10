using UnityEngine;
using UnityEngine.UI;
using Zenject;

[DisallowMultipleComponent]
public class Interactable : Entity {
  [Inject]
  public IGroups Groups { get; set; }

  void Awake() {
    Groups.Interactables.Add(this);
  }
}