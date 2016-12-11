using UnityEngine;
using System;
using System.Collections.Generic;

public struct HiddenStuff {
  public bool IsHidden;
}

public static class Extensions {
  public static void SetX(this Transform transform, float x) {
     Vector3 newPosition =
        new Vector3(x, transform.position.y, transform.position.z);

     transform.position = newPosition;
  }

  public static void SetY(this Transform transform, float y) {
     Vector3 newPosition =
        new Vector3(transform.position.x, y, transform.position.z);

     transform.position = newPosition;
  }

  private static Dictionary<GameObject, HiddenStuff> HiddenInfo = new Dictionary<GameObject, HiddenStuff>();

  // Is Unity just a troll framework?

  /**
   * This is my stupid function and it's better than every single function that
   * exists in Unity because it actually works
   */
  public static void Hide(this GameObject target) {
    if (!Extensions.HiddenInfo.ContainsKey(target)) {
      Extensions.HiddenInfo[target] = new HiddenStuff {
        IsHidden = false
      };
    }

    HiddenStuff info = Extensions.HiddenInfo[target];

    if (!info.IsHidden) {
      foreach (var r in target.GetComponentsInChildren<Renderer>()) {
        r.enabled = false;
      }

      foreach (var r in target.GetComponentsInChildren<CanvasRenderer>()) {
        r.SetAlpha(0f);
      }
    }

    Extensions.HiddenInfo[target] = new HiddenStuff {
      IsHidden = true
    };
  }

  public static void Show(this GameObject target) {
    if (!Extensions.HiddenInfo.ContainsKey(target)) {
      Extensions.HiddenInfo[target] = new HiddenStuff {
        IsHidden = false
      };
    }

    HiddenStuff info = Extensions.HiddenInfo[target];

    if (info.IsHidden) {
      foreach (var r in target.GetComponentsInChildren<Renderer>()) {
        r.enabled = true;
      }

      foreach (var r in target.GetComponentsInChildren<CanvasRenderer>()) {
        r.SetAlpha(1f);
      }
    }

    Extensions.HiddenInfo[target] = new HiddenStuff {
      IsHidden = false
    };
  }

  public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
      Func<TSource, TKey> selector) {
      return source.MinBy(selector, null);
  }

  public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
      Func<TSource, TKey> selector, IComparer<TKey> comparer) {
      if (source == null) throw new ArgumentNullException("source");
      if (selector == null) throw new ArgumentNullException("selector");
      comparer = comparer ?? Comparer<TKey>.Default;

      using (var sourceIterator = source.GetEnumerator()) {
          if (!sourceIterator.MoveNext()) {
              throw new InvalidOperationException("Sequence contains no elements");
          }
          var min = sourceIterator.Current;
          var minKey = selector(min);
          while (sourceIterator.MoveNext()) {
              var candidate = sourceIterator.Current;
              var candidateProjected = selector(candidate);
              if (comparer.Compare(candidateProjected, minKey) < 0) {
                  min = candidate;
                  minKey = candidateProjected;
              }
          }
          return min;
      }
  }
}