using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace johnfn {
  public struct DialogElement {
    public string Speaker;
    public string Content;
  }

  public enum DialogID {
    NPCRec1,

    None,
  }

  [DisallowMultipleComponent]
  public class Dialog : Entity {
    public static Dictionary<DialogID, List<DialogElement> > AllDialog = new Dictionary<DialogID, List<DialogElement> > {
      { DialogID.NPCRec1, new List<DialogElement> {
          new DialogElement { Speaker = "You", Content = "Umm, Hello." },
          new DialogElement { Speaker = "Receptionist", Content = "Sup." },
          new DialogElement { Speaker = "You", Content = "Nm nm just chillin u?" },
          new DialogElement { Speaker = "Receptionist", Content = "Ima literally kill u rn." },
        }
      }
    };

    public Text DialogText;

    public Text SpeakerText;

    private List<DialogElement> _activeDialog = null;

    void Awake() {
      gameObject.Hide();
    }

    public void StartDialog(DialogID id) {
      if (id == DialogID.None) {
        return;
      }

      EffectedByModes.SetMode(Mode.UsingUI);

      gameObject.Show();

      _activeDialog = AllDialog[id];

      SpeakerText.text = _activeDialog[0].Speaker;
      DialogText.text = _activeDialog[0].Content;
    }
  }
}