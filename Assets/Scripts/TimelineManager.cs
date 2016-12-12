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

  public struct NPCAtTime {
    public Entity NPC;

    /**
     * Where the NPC started in this time slice
     */
    public Vector2 Position;

    public Desire Desire;
  }

  public struct TimeSlice {
    public TimeSpan TimeSpan;

    public List<NPCAtTime> NPCsAndActions;
  }

  [DisallowMultipleComponent]
  public class TimelineManager: MonoBehaviour {
    [Inject]
    public ITimeManager _timeManager;

    [Inject]
    private IPrefabReferences _prefabReferences;

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
          };

          switch (desire.Type) {
            case DesireType.Zen:
              npcAtTime.Desire = desire;

              break;
            case DesireType.Walk:
              npcAtTime.Desire = desire;

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

      SlicesProcessed.Add(relevantTimeSlice.TimeSpan);

      // Find and kill old coroutines.

      foreach (var co in ActiveCoroutines) {
        StopCoroutine(co);
      }

      ActiveCoroutines = new List<Coroutine>();

      // A new slice has started.

      foreach (var npcAction in relevantTimeSlice.NPCsAndActions) {
        var npc = npcAction.NPC;

        switch (npcAction.Desire.Type) {
          case DesireType.Zen:

            break;

          case DesireType.Walk:
            ActiveCoroutines.Add(StartCoroutine(WalkNPCCo(npcAction)));

            break;
        }
      }
    }

    public IEnumerator WalkNPCCo(NPCAtTime npcAction) {
      var npc = npcAction.NPC;
      var movementSpeed = 0.3f;
      var path = _prefabReferences.MapController.PathFind(npc.transform.position, npcAction.Desire.Destination);

      while (path.Count > 0) {
        var nextCell = path.FirstOrDefault();

        if (Vector2.Distance(nextCell, npc.transform.position) < movementSpeed * 1.5f) {
          path.Remove(nextCell);
          continue;
        }

        var moveVector = (nextCell - (Vector2) npc.transform.position).normalized * movementSpeed * Time.deltaTime;

        npcAction.NPC.transform.Translate(moveVector);

        yield return null;
      }
    }
  }
}