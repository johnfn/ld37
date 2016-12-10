using UnityEngine;
using UnityEngine.UI;
using Zenject;

[DisallowMultipleComponent]
public class Interactable : Entity {
  [Inject]
  public IGroups Groups { get; set; }

  public string InteractVerb = "Dont know!";

  void Awake() {
    Groups.Interactables.Add(this);
  }
}