using System.Collections.Generic;
using UnityEngine;

public class Groups : IGroups {
  private List<Interactable> _interactables;

  public List<Interactable> Interactables {
    get {
      var result = _interactables;

      if (result == null) {
        _interactables = new List<Interactable>();
      }

      return _interactables;
    }

    set {
      _interactables = value;
    }
  }

  public Interactable ClosestTo(GameObject target) {
    Interactable closestSoFar = null;

    foreach (var i in Interactables) {
      if (closestSoFar == null ||
          Util.Distance(i.gameObject, target.gameObject) < Util.Distance(closestSoFar.gameObject, i.gameObject)) {
        closestSoFar = i;
      }
    }

    return closestSoFar;
  }
}

public interface IGroups {
  List<Interactable> Interactables { get; set; }

  Interactable ClosestTo(GameObject target);
}