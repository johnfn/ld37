using UnityEngine;
using System.Collections.Generic;

public struct HiddenStuff {
  public bool IsHidden;
}

public static class Extensions {
  public static void SetX(this Transform transform, float x) {
     Vector3 newPosition =
        new Vector3(x, transform.position.y, transform.position.z);

     transform.position = newPosition;
  }

  public static void SetY(this Transform transform, float y) {
     Vector3 newPosition =
        new Vector3(transform.position.x, y, transform.position.z);

     transform.position = newPosition;
  }

  private static Dictionary<GameObject, HiddenStuff> HiddenInfo = new Dictionary<GameObject, HiddenStuff>();

  // Is Unity just a troll framework?

  /**
   * This is my stupid function and it's better than every single function that
   * exists in Unity because it actually works
   */
  public static void Hide(this GameObject target) {
    if (!Extensions.HiddenInfo.ContainsKey(target)) {
      Extensions.HiddenInfo[target] = new HiddenStuff {
        IsHidden = false
      };
    }

    HiddenStuff info = Extensions.HiddenInfo[target];

    if (!info.IsHidden) {
      foreach (var r in target.GetComponentsInChildren<Renderer>()) {
        r.enabled = false;
      }

      foreach (var r in target.GetComponentsInChildren<CanvasRenderer>()) {
        r.SetAlpha(0f);
      }
    }

    Extensions.HiddenInfo[target] = new HiddenStuff {
      IsHidden = true
    };
  }

  public static void Show(this GameObject target) {
    Debug.Log("Show!");

    if (!Extensions.HiddenInfo.ContainsKey(target)) {
      Extensions.HiddenInfo[target] = new HiddenStuff {
        IsHidden = false
      };
    }

    HiddenStuff info = Extensions.HiddenInfo[target];

    if (info.IsHidden) {
      foreach (var r in target.GetComponentsInChildren<Renderer>()) {
        r.enabled = true;
      }

      foreach (var r in target.GetComponentsInChildren<CanvasRenderer>()) {
        r.SetAlpha(1f);
      }
    }

    Extensions.HiddenInfo[target] = new HiddenStuff {
      IsHidden = false
    };
  }
}