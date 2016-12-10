using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
public class Manager: MonoBehaviour {
  [Inject]
  public IUtil Util { get; set; }

  [Inject]
  public ITimeManager TimeManager { get; set; }

  [Inject]
  public IPrefabReferences PrefabReferences { get; set; }

  public GameObject InteractableIconGameObject;

  void Awake() {
    PrefabReferences.InteractableIcon = InteractableIconGameObject.GetComponent<InteractableIcon>();
  }

  void Update() {
    TimeManager.Update();
  }
}