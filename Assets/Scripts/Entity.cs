using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

namespace johnfn {
  public class Entity: MonoBehaviour {
    public T GetComponentSafe<T>() {
      var result = GetComponent<T>();

      if (result == null) {
        Debug.Log("Can't get a " + ((typeof(T)).Name) + " on " + this.name);
      }

      return result;
    }

    public static void Log(params object[] v) {
      var o = "";

      for (int i = 0; i < v.Length; i++) {
        if (v[i] == null) {
          o += "null ";
        } else if (v[i].GetType().ToString().Contains("Generic.List")) {
          if (v[i] is IEnumerable) {

          }
          var result = "[";

          foreach (var x in (v[i] as IEnumerable<object>)) {
            result += Convert.ToString(x) + ",";
          }

          // Log(string.Join(" ", path.Select(_ => _.ToString()).ToArray()));

          o += result + " ";
        } else {
          o += v[i].ToString() + " ";
        }
      }

      Debug.Log(o);
    }
  }
}