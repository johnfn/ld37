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

  public GameObject FollowTextGameObject;

  public GameObject Canvas;

  public GameObject TimeSelectionCanvas;

  void Awake() {
    PrefabReferences.InteractableIcon = InteractableIconGameObject.GetComponent<InteractableIcon>();
    PrefabReferences.FollowText = FollowTextGameObject.GetComponent<FollowText>();
    PrefabReferences.Canvas = Canvas;
    PrefabReferences.TimeSelectionCanvas = TimeSelectionCanvas;
  }

  void Update() {
    TimeManager.Update();
  }
}