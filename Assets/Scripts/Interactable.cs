using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace johnfn {
  public enum InteractableTypes {
    NOT_SET,
    BED,
    NPC_REC,
    NPC_DRIFTER,
  }

  public struct InteractStuff {
    public string Verb;
    public InteractableTypes Type;
  }

  [DisallowMultipleComponent]
  public class Interactable : Entity {
    public static Dictionary<string, InteractStuff> InteractTypes = new Dictionary<string, InteractStuff>
    {
      { "Bed"       , new InteractStuff { Verb = "Go to sleep", Type = InteractableTypes.BED } },
      { "NPCRec"    , new InteractStuff { Verb = "Talk"       , Type = InteractableTypes.NPC_REC } },
      { "NPCDrifter", new InteractStuff { Verb = "Talk"       , Type = InteractableTypes.NPC_DRIFTER } }
    };

    [Inject]
    public IGroups Groups { get; set; }

    public string InteractVerb = "Dont know!";

    public InteractableTypes InteractType = InteractableTypes.NOT_SET;

    void Awake() {
      Groups.Interactables.Add(this);
    }
  }
}