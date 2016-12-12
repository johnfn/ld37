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

  public class NPCAtTime {
    public Entity NPC;

    /**
     * Where the NPC started in this time slice
     */
    public Vector2 Position;

    public Desire Desire;
  }

  public class TimeSlice {
    public TimeSpan TimeSpan;

    public List<NPCAtTime> NPCsAndActions;
  }

  [DisallowMultipleComponent]
  public class TimelineManager: Entity {
    [Inject]
    public ITimeManager _timeManager;

    [Inject]
    private IPrefabReferences _prefabReferences;

    public List<TimeSlice> Timeline = new List<TimeSlice>();

    public HashSet<TimeSpan> SlicesProcessed = new HashSet<TimeSpan>();

    public List<Coroutine> ActiveCoroutines = new List<Coroutine>();

    public static TimelineManager Instance;

    void Awake() {
      Instance = this;
    }

    void Start() {
      CalculateAllOfTimeAndSpace(0);
    }

    public void GoBackwardsTo(int minutesSinceMidnight) {
      SlicesProcessed = new HashSet<TimeSpan>(SlicesProcessed.Where(_ => _.Start < minutesSinceMidnight));
    }

    // Calculate everything that the NPCs will eventually do.

    private void CalculateAllOfTimeAndSpace(int fromTime) {
      Timeline = Timeline.Take((int) Mathf.Floor(fromTime / 15)).ToList();

      if (Timeline.Count == 0) {
        // Necessarily special case first time slice

        var initialConditions = new List<NPCAtTime>();

        foreach (var NPC in NPC.NPCs) {
          var npcAtTime = new NPCAtTime {
            NPC = NPC, // This is wrong too. There could be uninstantiated NPCS.
            Position = NPC.transform.position, // Definitely wrong. Should take from previous frame I think
            Desire = NPC.GetRelevantDesire(0),
          };

          initialConditions.Add(npcAtTime);
        }

        Timeline.Add(new TimeSlice {
          NPCsAndActions = initialConditions,
          TimeSpan = new TimeSpan { Start = fromTime, Stop = fromTime + 15 }
        });
      }

      // 15 minute resolution

      for (var time = fromTime + 15; time < 24 * 60; time += 15) {
        var processedSlice = SlicesProcessed.FirstOrDefault(_ => _.Contains(time));

        if (processedSlice != null) {
          SlicesProcessed.Remove(processedSlice);
        }

        var npcsAndActions = new List<NPCAtTime>();

        // Figure out what each NPC wants to be doing

        // Calculate a new state for each NPC based on the old one

        foreach (var NPC in NPC.NPCs) {
          var desire = NPC.GetRelevantDesire(time);
          var newNpcAtTime = new NPCAtTime {
            NPC = NPC,
            Desire = NPC.GetRelevantDesire(time)
          };

          // Figure out if previous time slice was successful!

          var lastNpcAtTime = Timeline.Last().NPCsAndActions.Find(_ => _.NPC == NPC);

          switch (lastNpcAtTime.Desire.Type) {
            case DesireType.Zen:
              newNpcAtTime.Position = lastNpcAtTime.Position;

              break;
            case DesireType.Walk:
              // TODO - actually pathfind... somehow... lel

              newNpcAtTime.Position = lastNpcAtTime.Desire.Destination;

              break;
            default:
              Debug.Log("Don't know how to propagate desire forward in time " + lastNpcAtTime.Desire.Type);

              break;
          }

          npcsAndActions.Add(newNpcAtTime);
        }

        var newTimeSlice = new TimeSlice {
          TimeSpan = new TimeSpan { Start = time, Stop = time + 15 },
          NPCsAndActions = npcsAndActions,
        };

        Timeline.Add(newTimeSlice);
      }
    }

    // Actually make them appear to do the stuff. // <- best comment ever

    public void Update() {
      var currentTime = _timeManager.MinutesSinceMidnight;
      var relevantTimeSlice = Timeline.Find(_ => _.TimeSpan.Contains(currentTime));

      if (relevantTimeSlice == null) {
        Debug.Log("Oh no, this is bad!");
      }

      if (SlicesProcessed.Contains(relevantTimeSlice.TimeSpan)) {
        // This slice is currently running.

        return;
      }

      SlicesProcessed.Add(relevantTimeSlice.TimeSpan);

      // Find and kill old coroutines.

      foreach (var co in ActiveCoroutines) {
        StopCoroutine(co);
      }

      ActiveCoroutines = new List<Coroutine>();

      // A new slice has started.

      // Put every NPC in their correct place (this may be wrong)

      foreach (var npcAction in relevantTimeSlice.NPCsAndActions) {
        var npc = npcAction.NPC;

        npc.transform.position = npcAction.Position;

        Log("Position to...", npcAction.Position);
      }

      foreach (var npcAction in relevantTimeSlice.NPCsAndActions) {
        var npc = npcAction.NPC;

        switch (npcAction.Desire.Type) {
          case DesireType.Zen:

            break;

          case DesireType.Walk:
            StartCoroutineEx(WalkNPCCo(npcAction));

            break;
        }
      }
    }

    public void StartCoroutineEx(IEnumerator co) {
      StartCoroutine(StartCoroutineExHelper(co));
    }

    private IEnumerator StartCoroutineExHelper(IEnumerator co) {
      Coroutine runningCoroutine = StartCoroutine(co);

      ActiveCoroutines.Add(runningCoroutine);

      yield return runningCoroutine;

      ActiveCoroutines.Remove(runningCoroutine);
    }

    public IEnumerator WalkNPCCo(NPCAtTime npcAction) {
      var npc = npcAction.NPC;
      var movementSpeed = 0.5f;
      var path = _prefabReferences.MapController.PathFind(npc.transform.position, npcAction.Desire.Destination);

      while (path.Count > 0) {
        var nextCell = path.FirstOrDefault();

        if (Vector2.Distance(nextCell, npc.transform.position) < movementSpeed * Time.deltaTime * 2) {
          npcAction.NPC.transform.position = nextCell;

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