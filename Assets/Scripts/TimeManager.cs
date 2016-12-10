using UnityEngine;

[DisallowMultipleComponent]
public class TimeManager : ITimeManager {
  private float _minutesSinceDawn = 0;

  private int startHour = 6;

  private int startMinute = 0;

  public string currentTime {
    get {
      var hoursPassed = Mathf.Floor(_minutesSinceDawn / 60);
      var currentHour = (startHour + hoursPassed) % 12;
      var currentMinute = ((startMinute + _minutesSinceDawn) % 60).ToString("00");
      var ampm = "AM";

      if (startHour + hoursPassed >= 12) {
        ampm = "PM";
      }

      if (currentHour == 0) {
        currentHour = 12;
      }

      return string.Format("{0}:{1} {2}", currentHour, currentMinute, ampm);
    }
  }

  public float percentageTimePassed {
    get {
      return _minutesSinceDawn / (60 * 24);
    }
  }

  public void Update() {
    _minutesSinceDawn += Time.deltaTime * 30;
  }
}

public interface ITimeManager {
  string currentTime { get; }

  float percentageTimePassed { get; }

  void Update();
}