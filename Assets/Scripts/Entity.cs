using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity: MonoBehaviour {
  public T GetComponentSafe<T>() {
    var result = GetComponent<T>();

    if (result == null) {
      Debug.Log("Can't get a " + ((typeof(T)).Name) + " on " + this.name);
    }

    return result;
  }
}