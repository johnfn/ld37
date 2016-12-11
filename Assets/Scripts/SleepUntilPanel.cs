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

  public bool ActiveOnStart = false;

  private GameObject _activeIndicator {
    get {
      return Indicators[_indicatorIndex];
    }
  }

  private int _targetTime {
    get {
      return _targetHour * 60 + _targetMinute + (_targetIsAM ? 0 : 60 * 12);
    }
  }

  private int _targetHour = 8;

  private int _targetMinute = 0;

  private bool _targetIsAM = true;

  private int _indicatorIndex;

  public void Awake() {
    _indicatorIndex = 0;
    _targetHour = 8;
    _targetMinute = 0;
    _targetIsAM = true;

    Indicators = new List<GameObject> { HourIndicator, MinuteIndicator, AMPMIndicator };

    gameObject.SetActive(ActiveOnStart);
  }

  public void Update() {
    HandleKeyEvents();
    Render();
  }

  public void Show() {
    gameObject.SetActive(true);
  }

  public void Hide() {
    gameObject.SetActive(false);
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

    if (Input.GetKeyDown(KeyCode.UpArrow)) {
      // hour
      if (_indicatorIndex == 0) {
        _targetHour += 1;
      }

      // minute
      if (_indicatorIndex == 1) {
        _targetMinute += 15;
      }

      if (_indicatorIndex == 2) {
        _targetIsAM = !_targetIsAM;
      }

      // cascading updates

      if (_targetMinute > 59) {
        _targetMinute = 0;
        _targetHour += 1;
      }

      if (_targetHour > 11) {
        _targetHour = 0;
        _targetIsAM = !_targetIsAM;
      }
    }

    if (Input.GetKeyDown(KeyCode.DownArrow)) {
      // hour
      if (_indicatorIndex == 0) {
        _targetHour -= 1;
      }

      // minute
      if (_indicatorIndex == 1) {
        _targetMinute -= 15;
      }

      if (_indicatorIndex == 2) {
        _targetIsAM = !_targetIsAM;
      }

      // cascading updates

      if (_targetMinute < 0) {
        _targetMinute = 45;
        _targetHour -= 1;
      }

      if (_targetHour < 0) {
        _targetHour = 11;
        _targetIsAM = !_targetIsAM;
      }
    }

    if (Input.GetKeyDown(KeyCode.Return)) {
      Hide();
    }
  }
}