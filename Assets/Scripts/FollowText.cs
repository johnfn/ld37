using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FollowText : Entity {
  public GameObject Target;

  private Vector2 TargetSize;

  private Text Text;

  void Awake() {
    Text = GetComponent<Text>();

  }

  void Start() {
    TargetSize = Target.GetComponent<SpriteRenderer>().bounds.size;
  }

  void Update() {
    Text.rectTransform.position = new Vector2(
      Target.transform.position.x + TargetSize.x / 2 + 0.2f,
      Target.transform.position.y + TargetSize.y / 2
    );
  }
}