using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace johnfn {
  public class TimeSpan {
    public int Start;

    public int Stop;

    public bool Contains(int time) {
      return Start <= time && Stop > time;
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

    public HashSet<TimeSpan> SlicesProcessed = new HashSet<TimeSpan>();

    public List<Coroutine> ActiveCoroutines = new List<Coroutine>();

    void Start() {
      CalculateAllOfTimeAndSpace(0);
    }

    // Calculate everything that the NPCs will eventually do.

    private void CalculateAllOfTimeAndSpace(int fromTime) {
      Timeline = new List<TimeSlice>();

      // 15 minute resolution

      for (var time = fromTime; time < 12 * 60; time += 15) {
        var processedSlice = SlicesProcessed.FirstOrDefault(_ => _.Contains(time));

        if (processedSlice != null) {
          SlicesProcessed.Remove(processedSlice);
        }

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

      // This slice is currently running.
      if (SlicesProcessed.Contains(relevantTimeSlice.TimeSpan)) {
        return;
      }

      // A new slice has started.

      // TODO - Find any running coroutines and kill them.

      foreach (var npcAction in relevantTimeSlice.NPCsAndActions) {
        var npc = npcAction.NPC;

        switch (npcAction.Action) {
          case ActionType.Standing:

            break;

          case ActionType.Walking:
            ActiveCoroutines.Add(StartCoroutine(WalkNPCCo(npcAction)));

            break;
        }
      }
    }

    public IEnumerator WalkNPCCo(NPCAtTime npcAction) {
      while (true) {
        npcAction.NPC.transform.Translate(new Vector3(0.01f * Time.deltaTime, 0f, 0f));

        yield return null;
      }
    }
  }
}