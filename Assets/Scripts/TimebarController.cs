using UnityEngine;
using UnityEngine.UI;
using Zenject;

[DisallowMultipleComponent]
public class TimebarController : Entity {
  [Inject]
  public ITimeManager TimeManager { get; set; }

  public Image Sun;

  public Image SunEndPosition;

  public Text TimeText;

  private float _distanceForSunToGo;

  private float _sunStartX;

  void Start() {
    _sunStartX = Sun.rectTransform.localPosition.x;
    _distanceForSunToGo = SunEndPosition.rectTransform.localPosition.x  - Sun.rectTransform.localPosition.x;

    // You have served your purpose. I have no need for you now.
    Destroy(SunEndPosition);
  }

  void Update() {
    TimeText.text = TimeManager.currentTime;

    Sun.rectTransform.localPosition = new Vector2(
      _sunStartX + _distanceForSunToGo * TimeManager.percentageTimePassed,
      Sun.rectTransform.localPosition.y
    );
  }
}