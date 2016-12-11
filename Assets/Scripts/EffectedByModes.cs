using System.Collections.Generic;
using UnityEngine;

namespace johnfn {
  public enum Mode {
    Normal,
    TextHoveroverThingy,
    UsingUI,
    Dialog,
  }

  [DisallowMultipleComponent]
  public class EffectedByModes : Entity {
    public static List<EffectedByModes> AllModeGameObjects = new List<EffectedByModes>();

    public static Mode CurrentMode = Mode.Normal;

    public static Dictionary<MonoBehaviour, bool> OldEnabled = new Dictionary<MonoBehaviour, bool>();

    public Mode ActiveFor = Mode.Normal;

    public void Awake() {
      AllModeGameObjects.Add(this);

      foreach (var comp in gameObject.GetComponents<MonoBehaviour>()) {
        EffectedByModes.OldEnabled[comp] = comp.enabled;
      }
    }

    void Start() {
      EffectedByModes.SetMode(EffectedByModes.CurrentMode);
    }

    public static void SetMode(Mode newMode) {
      EffectedByModes.CurrentMode = newMode;

      foreach (var obj in EffectedByModes.AllModeGameObjects) {
        var isActive = obj.ActiveFor == newMode;

        foreach (var comp in obj.GetComponents<MonoBehaviour>()) {
          /*

          // I don't get it...

          if (comp is Renderer || comp is CanvasRenderer) {
            Debug.Log("???");

            continue;
          }
          */

          /*
          if (isActive) {
            comp.enabled = OldEnabled[comp];

            Debug.Log("Set comp " + comp + " to " + OldEnabled[comp]);
          } else {
            Debug.Log("Turn off" + comp + ", store" + comp.enabled);

            OldEnabled[comp] = comp.enabled;

            comp.enabled = false;
          }
          */

          comp.enabled = isActive;
        }
      }
    }
  }
}