using UnityEngine;
using UnityEngine.UI;
using Zenject;

[DisallowMultipleComponent]
public class Interactor : Entity {
  [Inject]
  public IGroups Groups { get; set; }

  [Inject]
  public IPrefabReferences PrefabReferences;

  private InteractableIcon _icon;

  private Interactable currentTarget = null;

  void Awake() {
    _icon = Instantiate(PrefabReferences.InteractableIcon, transform.position, Quaternion.identity);
  }

  void Update() {
    var closest = Groups.InteractableClosestTo(gameObject);

    _icon.SetTarget(closest);
  }
}