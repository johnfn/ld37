using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class SleepUntilPanel : Entity {
  public GameObject HourIndicator;

  public GameObject MinuteIndicator;

  public GameObject AMPMIndicator;

  public Text TimeText;

  public List<GameObject> Indicators;

  private GameObject _activeIndicator {
    get {
      return Indicators[_indicatorIndex];
    }
  }

  private int _targetTime = 0;

  private int _indicatorIndex;

  public void Awake() {
    _indicatorIndex = 0;
    _targetTime = 8 * 60;

    Indicators = new List<GameObject> { HourIndicator, MinuteIndicator, AMPMIndicator };
  }

  public void Update() {
    HandleKeyEvents();
    Render();
  }

  private void Render() {
    // Indicators

    for (var i = 0; i < Indicators.Count; i++) {
      var image = Indicators[i].GetComponent<Image>();

      if (Indicators[i] == _activeIndicator) {
        image.color = Color.red;
      } else {
        image.color = Color.white;
      }
    }

    // Time

    TimeText.text = TimeManager.MinutesSinceDawnToString(_targetTime);
  }

  private void HandleKeyEvents() {
    if (Input.GetKeyDown(KeyCode.LeftArrow)) {
      _indicatorIndex = _indicatorIndex - 1;

      if (_indicatorIndex < 0) {
        _indicatorIndex = Indicators.Count - 1;
      }
    }

    if (Input.GetKeyDown(KeyCode.RightArrow)) {
      _indicatorIndex = (_indicatorIndex + 1) % Indicators.Count;
    }
  }
}