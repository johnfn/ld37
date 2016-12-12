using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace johnfn {
  public enum DesireType {
    Walk,
    Talk,

    WalkToSomeone,
    LeaveHotel,
    EnterHotel,

    // No desire
    Zen,




    // (get it)
  }

  public struct Desire {
    public TimeSpan TimeSpan;

    public DesireType Type;

    public Vector2 Destination;

    public InteractableTypes PersonIWannaTalkTo;
  }

  [DisallowMultipleComponent]
  public class NPC : Entity {
    [Inject]
    private ITimeManager _timeManager;

    [Inject]
    private IPrefabReferences _prefabReferences;

    public List<Desire> Desires = new List<Desire> {
      new Desire {
        Type = DesireType.WalkToSomeone,
        PersonIWannaTalkTo = InteractableTypes.NPC_REC,
        TimeSpan = new TimeSpan { Start = 6 * 60, Stop = 6 * 60 + 15 },
      },

      new Desire {
        Type = DesireType.Talk,
        PersonIWannaTalkTo = InteractableTypes.NPC_REC,
        TimeSpan = new TimeSpan { Start = 6 * 60 + 15, Stop = 7 * 60 }
      }
    };

    public static List<NPC> NPCs = new List<NPC>();

    void Awake() {
      NPC.NPCs.Add(this);
    }

    void Start() {

    }

    public Desire GetRelevantDesire(int currentTime) {
      foreach (var desire in Desires) {
        if (desire.TimeSpan.Contains(currentTime)) {
          return desire;
        }
      }

      // none found...

      return new Desire {
        Type = DesireType.Zen,
        TimeSpan = new TimeSpan { Start = currentTime, Stop = currentTime + 15 }
      };
    }
  }
}