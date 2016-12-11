using UnityEngine;
using System.Collections.Generic;

namespace johnfn {
  public struct DialogElement {
    public string Speaker;
    public string Content;
  }

  [DisallowMultipleComponent]
  public class Dialog : Entity {

    public static Dictionary<string, List<DialogElement> > AllDialog = new Dictionary<string, List<DialogElement> > {
      { "NPCRec1", new List<DialogElement> {
          new DialogElement { Speaker = "You", Content = "Umm, Hello." },
          new DialogElement { Speaker = "Receptionist", Content = "Sup." },
          new DialogElement { Speaker = "You", Content = "Nm nm just chillin u?" },
          new DialogElement { Speaker = "Receptionist", Content = "Ima literally kill u rn." },
        }
      }
    };

    void Awake() {
      gameObject.Hide();
    }

    void Update() {

    }
  }
}