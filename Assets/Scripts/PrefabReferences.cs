using UnityEngine;

namespace johnfn {

  public class PrefabReferences : IPrefabReferences {
    public InteractableIcon InteractableIcon { get; set; }

    public FollowText FollowText { get; set; }

    public GameObject Canvas { get; set; }

    public GameObject TimeSelectionCanvas { get; set; }

    public GameObject FadeOverlay { get; set; }

    public GameObject Dialog { get; set; }

    public FollowText CreateFollowText(GameObject target, string text) {
      if (FollowText.Instance) {
        return null;
      }

      var result = GameObject.Instantiate(FollowText, Vector3.zero, Quaternion.identity);

      result.transform.SetParent(Canvas.transform, false);
      result.GetComponent<FollowText>().Target = target;
      result.GetComponent<FollowText>().ShowText("There's nothing there.");

      return result;
    }
  }

  public interface IPrefabReferences {
    InteractableIcon InteractableIcon { get; set; }

    FollowText FollowText { get; set; }

    GameObject Canvas { get; set; }

    FollowText CreateFollowText(GameObject target, string text);

    GameObject TimeSelectionCanvas { get; set; }

    GameObject FadeOverlay { get; set; }

    GameObject Dialog { get; set; }
  }
}