using UnityEngine;

namespace johnfn {
  [DisallowMultipleComponent]
  public class TimeManager : ITimeManager {
    private float _minutesSinceMidnight = 6 * 60;

    public float ArbitarySpeedupAmount = 30f;

    public string currentTime {
      get {
        return MinutesSinceDawnToString(_minutesSinceMidnight);
      }
    }

    public int MinutesSinceMidnight {
      get {
        return (int) _minutesSinceMidnight;
      }
    }

    public void SetCurrentTime(float minutesSinceMidnight) {
      _minutesSinceMidnight = minutesSinceMidnight;
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
        return (_minutesSinceMidnight - 6 * 60) / (60 * 24);
      }
    }

    public void Update() {
      if (EffectedByModes.CurrentMode == Mode.Normal) {
        _minutesSinceMidnight += Time.deltaTime * ArbitarySpeedupAmount;
      }
    }
  }

  public interface ITimeManager {
    string currentTime { get; }

    void SetCurrentTime(float minutesSinceMidnight);

    float percentageTimePassed { get; }

    int MinutesSinceMidnight { get; }

    void Update();
  }
}