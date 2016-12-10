using UnityEngine;
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
    var distance = Util.Distance(closest.gameObject, gameObject);

    if (distance < 1f) {
      currentTarget = closest;
    } else {
      closest = null;
    }

    _icon.SetTarget(closest);
  }

  public Interactable GetTarget() {
    return currentTarget;
  }
}