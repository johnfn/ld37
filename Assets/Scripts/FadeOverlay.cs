using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FadeOverlay : Entity {
  public int FramesToSleep = 30;

  private Image _imageToFade;

  public void Awake() {
    _imageToFade = GetComponent<Image>();
  }

  public IEnumerator FadeInAndOut() {
    for (var i = 0; i < FramesToSleep; i++) {
      _imageToFade.color = new Color(0, 0, 0, (float) i / (float) FramesToSleep);

      yield return new WaitForEndOfFrame();
    }

    for (var i = 0; i < FramesToSleep; i++) {
      yield return new WaitForEndOfFrame();
    }

    for (var i = 0; i < FramesToSleep; i++) {
      _imageToFade.color = new Color(0, 0, 0, (float) (FramesToSleep - i) / (float) FramesToSleep);

      yield return new WaitForEndOfFrame();
    }
  }
}