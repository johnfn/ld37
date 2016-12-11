using UnityEngine;

namespace johnfn {
  public class Entity: MonoBehaviour {
    public T GetComponentSafe<T>() {
      var result = GetComponent<T>();

      if (result == null) {
        Debug.Log("Can't get a " + ((typeof(T)).Name) + " on " + this.name);
      }

      return result;
    }
  }
}