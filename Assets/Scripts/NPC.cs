using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace johnfn {
  public enum DesireType {
    Walk,

    TalkToRec,
    TalkToYou,
    LeaveHotel,
    EnterHotel,

    // No desire
    Zen,




    // (get it)
  }

  public struct Desire {
    public TimeSpan TimeSpan;

    public DesireType Type;

    public Vector2 Location;
  }

  [DisallowMultipleComponent]
  public class NPC : Entity {
    [Inject]
    private ITimeManager _timeManager;

    [Inject]
    private IPrefabReferences _prefabReferences;

    public List<Desire> Desires = new List<Desire> {
      new Desire {
        Type = DesireType.Walk,
        Location = new Vector2(4f, 4f), // TODO - totally random lloll
        TimeSpan = new TimeSpan { Start = 6 * 60, Stop = 9 * 60 },
      }
    };

    public static List<NPC> NPCs = new List<NPC>();

    void Awake() {
      NPC.NPCs.Add(this);
    }

    void Start() {
      var result = _prefabReferences.MapController.PathFind(transform.position, (Vector2) transform.position + new Vector2(1f, 1f));

      Debug.Log("HEllo!");
      Debug.Log(result.Count);

      foreach (var vector in result) {
        GameObject.Instantiate(_prefabReferences.DebugTile, vector, Quaternion.identity);
      }
    }

    public Desire GetRelevantDesire(int currentTime) {
      foreach (var desire in Desires) {
        if (desire.TimeSpan.Contains(currentTime)) {
          return desire;
        }
      }

      // none found...

      return new Desire {
        Type = DesireType.Zen
      };
    }
  }
}