using UnityEngine;

namespace johnfn {
  public class Util : IUtil {
    private float _mapWidth = 10f;
    private float _mapHeight = 10f;

    public float MapWidth {
      get { return _mapWidth; }
      set { _mapWidth = value; }
    }

    public float MapHeight {
      get { return _mapHeight; }
      set { _mapHeight = value; }
    }

    public static float Distance(GameObject a, GameObject b) {
      return (a.transform.position - b.transform.position).magnitude;
    }

    public bool Debug { get; set; }
  }

  public interface IUtil {
    float MapWidth { get; set; }
    float MapHeight { get; set; }

    bool Debug { get; set; }
  }
}