using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

[DisallowMultipleComponent]
public class InteractableIcon : Entity {
  private Interactable _target;

  private IEnumerator coroutine;

  private Tweener activeTween = null;

  private SpriteRenderer _renderer;

  void Awake() {
    _renderer = GetComponent<SpriteRenderer>();
  }

  public void SetTarget(Interactable target) {
    _renderer.enabled = (target != null);

    if (target == null) {
      _target = null;

      StopCoroutine(coroutine);

      coroutine = null;

      return;
    }

    if (_target != target) {

      if (coroutine != null) {
        StopCoroutine(coroutine);
      }

      _target = target;

      var size = _target.GetComponentInChildren<MeshRenderer>().bounds.size;

      transform.position = new Vector2(
        _target.transform.position.x + size.x / 2,
        _target.transform.position.y + size.y
      );

      coroutine = Animate();

      StartCoroutine(coroutine);
    }
  }

  IEnumerator Animate() {
    // Super tricky evil bug of death
    // (The old tween will keep tweening unless you explicitly tell it to stop)
    if (activeTween != null) {
      activeTween.Complete();
    }

    while (true) {
      var size = _target.GetComponentInChildren<MeshRenderer>().bounds.size;

      var start = new Vector2(
        _target.transform.position.x + size.x / 2,
        _target.transform.position.y + size.y
      );

      transform.position = start;

      activeTween = transform.DOMoveY(start.y + 1f, 1f)
        .SetEase(Ease.InOutSine);

      yield return activeTween.WaitForCompletion();

      activeTween = transform.DOMoveY(start.y, 1f)
        .SetEase(Ease.InOutSine);

      yield return activeTween.WaitForCompletion();
    }
  }
}