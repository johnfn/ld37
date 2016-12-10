using UnityEngine;
using Zenject;

public enum InteractableTypes {
  NOT_SET,
  BED,
}

[DisallowMultipleComponent]
public class Interactable : Entity {
  [Inject]
  public IGroups Groups { get; set; }

  public string InteractVerb = "Dont know!";

  public InteractableTypes InteractType = InteractableTypes.NOT_SET;

  void Awake() {
    Groups.Interactables.Add(this);
  }
}