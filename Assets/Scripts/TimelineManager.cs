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

    public List<InteractableTypes> PeopleTalkedTo;
  }

  public class TimeSlice {
    public TimeSpan TimeSpan;

    public List<NPCAtTime> NPCsAndActions;
  }

  public struct CoroutineAndTimeSpan {
    public TimeSpan TimeSpan;

    public Coroutine Coroutine;
  }

  [DisallowMultipleComponent]
  public class TimelineManager: Entity {
    [Inject]
    public ITimeManager _timeManager;

    [Inject]
    private IPrefabReferences _prefabReferences;

    [Inject]
    private IGroups _groups;

    public List<TimeSlice> Timeline = new List<TimeSlice>();

    public HashSet<TimeSpan> SlicesProcessed = new HashSet<TimeSpan>();

    public List<CoroutineAndTimeSpan> ActiveCoroutines = new List<CoroutineAndTimeSpan>();

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
            PeopleTalkedTo = new List<InteractableTypes>()
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
          // Figure out if previous time slice was successful!

          var lastNpcAtTime = Timeline.Last().NPCsAndActions.Find(_ => _.NPC == NPC);
          var desire = NPC.GetRelevantDesire(time);
          var newNpcAtTime = new NPCAtTime {
            NPC = NPC,
            Desire = NPC.GetRelevantDesire(time),
            PeopleTalkedTo = lastNpcAtTime.PeopleTalkedTo,
          };

          switch (lastNpcAtTime.Desire.Type) {
            case DesireType.Zen:
              newNpcAtTime.Position = lastNpcAtTime.Position;

              break;
            case DesireType.Talk:
              // TODO - Actually see if they were able to talk!

              newNpcAtTime.Position = lastNpcAtTime.Position;
              newNpcAtTime.PeopleTalkedTo.Add(lastNpcAtTime.Desire.PersonIWannaTalkTo);

              break;
            case DesireType.Walk:
              // TODO - actually pathfind... somehow... lel

              newNpcAtTime.Position = lastNpcAtTime.Desire.Destination;

              break;
            case DesireType.WalkToSomeone:
              var desiredPerson = _groups.Interactables.Find(_ => _.InteractType == lastNpcAtTime.Desire.PersonIWannaTalkTo);

              newNpcAtTime.Position = (Vector2) desiredPerson.transform.position + new Vector2(0.32f, 0f);

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

      foreach (var pair in ActiveCoroutines) {
        if (!pair.TimeSpan.Contains(currentTime)) {
          StopCoroutine(pair.Coroutine);
        }
      }

      ActiveCoroutines = new List<CoroutineAndTimeSpan>();

      // A new slice has started.

      // Put every NPC in their correct place (this may be wrong)

      foreach (var npcAction in relevantTimeSlice.NPCsAndActions) {
        var npc = npcAction.NPC;

        npc.transform.position = npcAction.Position;
      }

      foreach (var npcAction in relevantTimeSlice.NPCsAndActions) {
        var npc = npcAction.NPC;
        var timeSpan = relevantTimeSlice.TimeSpan;

        switch (npcAction.Desire.Type) {
          case DesireType.Zen:

            break;

          case DesireType.Talk:
          {
            StartCoroutineEx(TalkNPCCo(npcAction), npcAction.Desire.TimeSpan);
            break;
          }

          case DesireType.Walk:
          {
            var path = _prefabReferences.MapController.PathFind(npc.transform.position, npcAction.Desire.Destination);

            StartCoroutineEx(WalkNPCCo(npcAction.NPC, path), npcAction.Desire.TimeSpan);

            break;
          }

          case DesireType.WalkToSomeone:
          {
            var person = npcAction.Desire.PersonIWannaTalkTo;
            var desiredPerson = _groups.Interactables.Find(_ => _.InteractType == person);
            var destPos = (Vector2) desiredPerson.transform.position + new Vector2(0.32f, 0f);
            var path = _prefabReferences.MapController.PathFind(npc.transform.position, destPos);

            StartCoroutineEx(WalkNPCCo(npcAction.NPC, path), npcAction.Desire.TimeSpan);

            break;
          }
        }
      }
    }

    public void StartCoroutineEx(IEnumerator co, TimeSpan timeSpan) {
      StartCoroutine(StartCoroutineExHelper(co, timeSpan));
    }

    private IEnumerator StartCoroutineExHelper(IEnumerator co, TimeSpan timeSpan) {
      Coroutine runningCoroutine = StartCoroutine(co);

      ActiveCoroutines.Add(new CoroutineAndTimeSpan {
        Coroutine = runningCoroutine,
        TimeSpan = timeSpan
      });

      yield return runningCoroutine;

      ActiveCoroutines = ActiveCoroutines.Where(_ => _.Coroutine != runningCoroutine).ToList();
    }

    public IEnumerator TalkNPCCo(NPCAtTime npcAction) {
      var guy1 = npcAction.Desire.PersonIWannaTalkTo;
      var guy2 = npcAction.NPC.GetComponent<Interactable>().InteractType;

      var guy1Obj = _groups.Interactables.Find(_ => _.InteractType == guy1).gameObject;
      var guy2Obj = _groups.Interactables.Find(_ => _.InteractType == guy2).gameObject;

      var dialog = Dialog.GetDialog(guy1, guy2, _timeManager.MinutesSinceMidnight);

      FollowText followText;

      foreach (var element in dialog) {
        if (element.Speaker == guy1.ToString()) {
          followText = _prefabReferences.CreateFollowText(guy1Obj, element.Content, false);
        } else {
          followText = _prefabReferences.CreateFollowText(guy2Obj, element.Content, false);
        }

        yield return StartCoroutineExHelper(followText.ShowTextHelperCo(element.Content), npcAction.Desire.TimeSpan);

        Destroy(followText.gameObject);
      }

      yield return null;
    }

    public IEnumerator WalkNPCCo(Entity npc, List<Vector2> path) {
      var movementSpeed = 0.5f;

      while (path.Count > 0) {
        var nextCell = path.FirstOrDefault();

        if (Vector2.Distance(nextCell, npc.transform.position) < movementSpeed * Time.deltaTime * 2) {
          npc.transform.position = nextCell;

          path.Remove(nextCell);

          continue;
        }

        var moveVector = (nextCell - (Vector2) npc.transform.position).normalized * movementSpeed * Time.deltaTime;

        npc.transform.Translate(moveVector);

        yield return null;
      }
    }

  }
}