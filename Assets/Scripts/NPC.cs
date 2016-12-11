using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace johnfn {
  public enum DesireType {
    TalkToRec,
    TalkToYou,
    LeaveHotel,
    EnterHotel,

    // No desire
    Zen,




    // (get it)
  }

  public struct TimeSpan {
    public int Start;

    public int Stop;
  }

  public struct Desire {
    public TimeSpan TimeSpan;

    public DesireType Type;
  }

  [DisallowMultipleComponent]
  public class NPC : Entity {
    [Inject]
    private ITimeManager _timeManager;

    [Inject]
    private IPrefabReferences _prefabReferences;

    public List<Desire> Desires = new List<Desire> {
      new Desire {
        Type = DesireType.TalkToRec,
        TimeSpan = new TimeSpan { Start = 6 * 60, Stop = 9 * 60 },
      }
    };

    public static List<NPC> NPCs = new List<NPC>();

    void Awake() {
      NPC.NPCs.Add(this);
    }

    void Start() {
      CalculateAllOfTimeAndSpace();
    }

    private void CalculateAllOfTimeAndSpace() {
      // Oh my god
    }

    public void Update() {
      var currentDesire = GetRelevantDesire();

      switch (currentDesire) {
        case DesireType.TalkToRec:
          var result = _prefabReferences.MapController.PathFind(transform.position, (Vector2) transform.position + new Vector2(4f, 4f));

          Debug.Log(result);

          break;

        case DesireType.TalkToYou:
          Debug.Log("I wanna talk to you.");

          break;

        case DesireType.Zen:
          Debug.Log("Zennnn");

          break;

        default:
          Debug.Log("NPC doesn't know what to do!");

          break;
      }
    }

    public DesireType GetRelevantDesire() {
      var currentTime = _timeManager.MinutesSinceMidnight;

      foreach (var desire in Desires) {
        if (desire.TimeSpan.Start < currentTime && desire.TimeSpan.Stop >= currentTime) {
          return desire.Type;
        }
      }

      // none found...

      return DesireType.Zen;
    }
  }
}