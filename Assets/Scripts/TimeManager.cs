using UnityEngine;

namespace johnfn {
  [DisallowMultipleComponent]
  public class TimeManager : ITimeManager {
    private float _minutesSinceDawn = 0;

    private int startHour = 6;

    public string currentTime {
      get {
        return MinutesSinceDawnToString(_minutesSinceDawn + (startHour * 60));
      }
    }

    public static string MinutesSinceDawnToString(float minutes) {
        var hoursPassed = Mathf.Floor(minutes / 60);
        var currentHour = hoursPassed % 12;
        var currentMinute = (minutes % 60).ToString("00");
        var ampm = Mathf.Floor(hoursPassed / 12) % 2 == 0 ? "AM" : "PM";

        if (currentHour == 0) {
          currentHour = 12;
        }

        return string.Format("{0}:{1} {2}", currentHour, currentMinute, ampm);
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
}