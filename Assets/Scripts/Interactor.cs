using UnityEngine;
using Zenject;

namespace johnfn {
  [DisallowMultipleComponent]
  public class Interactor : Entity {
    [Inject]
    public IGroups Groups { get; set; }

    [Inject]
    public IPrefabReferences PrefabReferences;

    public float DistanceToInteract = 2f;

    private InteractableIcon _icon;

    private Interactable currentTarget = null;

    void Awake() {
      _icon = Instantiate(PrefabReferences.InteractableIcon, transform.position, Quaternion.identity);
    }

    void Update() {
      var closest = Groups.InteractableClosestTo(gameObject);
      var distance = Util.Distance(closest.gameObject, gameObject);

      currentTarget = distance < DistanceToInteract ? closest : null;

      _icon.SetTarget(currentTarget);
    }

    public Interactable GetTarget() {
      return currentTarget;
    }
  }
}