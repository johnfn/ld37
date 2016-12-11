using System.Collections.Generic;
using UnityEngine;

namespace johnfn {
  public enum Mode {
    Normal,
    TextHoveroverThingy,
    UsingUI,
  }

  [DisallowMultipleComponent]
  public class EffectedByModes : Entity {
    public static List<EffectedByModes> AllModeGameObjects = new List<EffectedByModes>();

    public static Mode CurrentMode = Mode.Normal;

    public Mode ActiveFor = Mode.Normal;

    public void Awake() {
      AllModeGameObjects.Add(this);
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

          comp.enabled = isActive;
        }
      }
    }
  }
}