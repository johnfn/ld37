using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace johnfn {
  [DisallowMultipleComponent]
  public class FollowText : Entity, ICanPressSpace {
    public static int FollowTextCount;

    public GameObject Target;

    private Vector2 TargetSize;

    private Text Text;

    private bool _finishedTyping = false;

    private IEnumerator _typingCoroutine;

    private string _fullMessage;

    void Awake() {
      Text = GetComponent<Text>();

      FollowTextCount++;
    }

    void Start() {
      TargetSize = Target.Size();
    }

    public void ShowText(string text) {
      StartCoroutine(_typingCoroutine = ShowTextHelperCo(text));
    }

    public bool PressSpace() {
      if (!_finishedTyping) {
        _finishedTyping = true;
        Text.text = _fullMessage;
        StopCoroutine(_typingCoroutine);

        return false;
      } else {
        Destroy(gameObject);

        return true;
      }
    }

    public IEnumerator ShowTextHelperCo(string text) {
      _finishedTyping = false;
      _fullMessage = text;

      Text.text = "";

      for (var i = 0; i <= text.Length; i++) {
        Text.text = text.Substring(0, i);

        yield return new WaitForSeconds(0.1f);
      }

      _finishedTyping = false;
    }

    void Update() {
      Text.rectTransform.position = new Vector2(
        Target.transform.position.x + TargetSize.x / 2 + 0.2f,
        Target.transform.position.y + TargetSize.y / 2
      );
    }

    void OnDestroy() {
      FollowTextCount--;
    }
  }
}