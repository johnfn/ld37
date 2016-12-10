using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

[DisallowMultipleComponent]
public class InteractableIcon : Entity {
  void Start() {
    StartCoroutine(Animate());
  }

  IEnumerator Animate() {
    Tweener res;

    while (true) {
      res = transform.DOMoveY(transform.position.y + 0.5f, 1f)
        .SetEase(Ease.InOutSine);

      yield return res.WaitForCompletion();

      res = transform.DOMoveY(transform.position.y - 0.5f, 1f)
        .SetEase(Ease.InOutSine);

      yield return res.WaitForCompletion();
    }
  }
}