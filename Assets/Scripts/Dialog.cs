using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace johnfn {
  public struct DialogElement {
    public string Speaker;
    public string Content;
  }

  public struct DialogID {
    public InteractableTypes Speaker;
    public TimeSpan TimeSpan;
  }

  [DisallowMultipleComponent]
  public class Dialog : Entity {
    public static Dictionary<DialogID, List<DialogElement> > AllDialog = new Dictionary<DialogID, List<DialogElement> > {
      {
        new DialogID {
          Speaker = InteractableTypes.NPC_REC,
          TimeSpan = new TimeSpan { Start = 0 * 60, Stop = 8 * 60 }
        },
        new List<DialogElement> {
          new DialogElement { Speaker = "Receptionist", Content = "Good morning, Sam!" },
          new DialogElement { Speaker = "Receptionist", Content = "You're up bright and early, aren't you?" },
          new DialogElement { Speaker = "You", Content = "Grahhgh..." },
          new DialogElement { Speaker = "Narrator", Content = "(You hate mornings.)" },
        }
      },

      {
        new DialogID {
          Speaker = InteractableTypes.NPC_REC,
          TimeSpan = new TimeSpan { Start = 8 * 60, Stop = 16 * 60 }
        },
        new List<DialogElement> {
          new DialogElement { Speaker = "Receptionist", Content = "Good afternoon, Sam!" },
          new DialogElement { Speaker = "Receptionist", Content = "It looks like it's going to be another beautiful day!" },
          new DialogElement { Speaker = "You", Content = "I have the strangest feeling of deja vu right now." },
          new DialogElement { Speaker = "Narrator", Content = "(The receptionist is not quite sure what to say.)" },
        }
      },

    };

    public Text DialogText;

    public Text SpeakerText;

    private List<DialogElement> _activeDialog = null;

    void Awake() {
      gameObject.Hide();
    }

    public void StartDialog(InteractableTypes guy, int time) {
      foreach (KeyValuePair<DialogID, List<DialogElement>> entry in AllDialog) {
        if (entry.Key.TimeSpan.Contains(time) && entry.Key.Speaker == guy) {
          _activeDialog = entry.Value;
        }
      }

      if (_activeDialog == null) {
        return;
      }

      EffectedByModes.SetMode(Mode.Dialog);
      gameObject.Show();

      StartCoroutine(HandleDialog());
    }

    private IEnumerator HandleDialog() {
      for (var i = 0; i < _activeDialog.Count; i++) {
        SpeakerText.text = _activeDialog[i].Speaker;

        for (var j = 0; j < _activeDialog[i].Content.Length; j++) {
          DialogText.text = _activeDialog[i].Content.Substring(0, j);

          yield return null;

          if (Input.GetKeyDown(KeyCode.Space)) {
            break;
          }
        }

        DialogText.text = _activeDialog[i].Content;

        yield return null;

        while (!Input.GetKeyDown(KeyCode.Space)) {
          // Note: WaitUntilEndOfFrame does NOT work here! it is not
          // interchangable with null, for some terrible reason!

          yield return null;
        }
      }

      EffectedByModes.SetMode(Mode.Normal);
      gameObject.Hide();
    }
  }
}