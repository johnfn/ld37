using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace johnfn {
  public struct TimeSpan {
    public int Start;

    public int Stop;

    public bool Contains(int time) {
      return Start < time && Stop >= time;
    }
  }

  public enum ActionType {
    Walking,
    Standing,
  }

  public class WalkingAction {
    public Vector2 Destination;
  }

  public struct NPCAtTime {
    public Entity NPC;

    public Vector2 Position;

    public ActionType Action;

    public WalkingAction WalkingAction;
  }

  public struct TimeSlice {
    public TimeSpan TimeSpan;

    public List<NPCAtTime> NPCsAndActions;
  }

  [DisallowMultipleComponent]
  public class TimelineManager: MonoBehaviour {
    [Inject]
    public ITimeManager _timeManager;

    public List<TimeSlice> Timeline = new List<TimeSlice>();

    void Start() {
      CalculateAllOfTimeAndSpace();
    }

    // Calculate everything that the NPCs will eventually do.

    private void CalculateAllOfTimeAndSpace() {
      Timeline = new List<TimeSlice>();

      // 15 minute resolution

      for (var time = 0; time < 12 * 60; time += 15) {
        var npcsAndActions = new List<NPCAtTime>();

        // Figure out what each NPC wants to be doing

        // Calculate a new state for each NPC based on the old one
        foreach (var NPC in NPC.NPCs) {
          var desire = NPC.GetRelevantDesire(time);
          var npcAtTime = new NPCAtTime {
            NPC = NPC, // This is wrong too. There could be uninstantiated NPCS.
            Position = NPC.transform.position, // Definitely wrong. Should take from previous frame I think
            Action = ActionType.Standing,
          };

          switch (desire.Type) {
            case DesireType.Zen:
              npcAtTime.Action = ActionType.Standing;

              break;
            case DesireType.Walk:
              npcAtTime.Action = ActionType.Walking;
              npcAtTime.WalkingAction = new WalkingAction {
                Destination = desire.Location
              };

              break;
          }

          npcsAndActions.Add(npcAtTime);
        }

        var newTimeSlice = new TimeSlice {
          TimeSpan = new TimeSpan { Start = time, Stop = time + 15 },
          NPCsAndActions = npcsAndActions,
        };

        Timeline.Add(newTimeSlice);
      }
    }

    // Actually make them appear to do the stuff.

    public void Update() {
      var currentTime = _timeManager.MinutesSinceMidnight;
      var relevantTimeSlice = Timeline.Find(_ => _.TimeSpan.Contains(currentTime));

      foreach (var npcAction in relevantTimeSlice.NPCsAndActions) {
        Debug.Log(npcAction.NPC + " is " + npcAction.Action + " ing");
      }
    }
  }
}