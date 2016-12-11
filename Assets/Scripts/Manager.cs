using UnityEngine;
using Zenject;

namespace johnfn {

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

    public GameObject FadeOverlay;

    public GameObject TimeSelectionCanvas;

    public GameObject Dialog;

    public GameObject MapController;

    public bool Debug = true;

    void Awake() {
      PrefabReferences.InteractableIcon = InteractableIconGameObject.GetComponent<InteractableIcon>();
      PrefabReferences.FollowText = FollowTextGameObject.GetComponent<FollowText>();
      PrefabReferences.Canvas = Canvas;
      PrefabReferences.TimeSelectionCanvas = TimeSelectionCanvas;
      PrefabReferences.FadeOverlay = FadeOverlay;
      PrefabReferences.Dialog = Dialog;
      PrefabReferences.MapController = MapController.GetComponent<MapController>();

      Util.Debug = Debug;
    }

    void Update() {
      TimeManager.Update();
    }
  }
}