using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace johnfn {
  [DisallowMultipleComponent]
  public class MapController : Entity {
    public GameObject AllWallObjects;

    // public List<Transform> WallPositions = new List<Transform>();

    public bool[,] HasWall;

    [HideInInspector]
    public float x;

    [HideInInspector]
    public float y;

    [HideInInspector]
    public float maxX;

    [HideInInspector]
    public float maxY;

    [HideInInspector]
    public float spriteWidth;

    [HideInInspector]
    public float spriteHeight;

    [HideInInspector]
    public int mapWidth;

    [HideInInspector]
    public int mapHeight;

    void Awake() {

    }

    void Start() {
      var allTransforms = new List<Transform>();

      foreach (Transform child in AllWallObjects.transform) {
        allTransforms.Add(child);
      }

      x = allTransforms.Min(_ => _.localPosition.x);
      y = allTransforms.Min(_ => _.localPosition.y);

      maxX = allTransforms.Max(_ => _.localPosition.x);
      maxY = allTransforms.Max(_ => _.localPosition.y);

      spriteWidth = allTransforms[0].gameObject.GetComponentInChildren<Renderer>().bounds.extents.x;
      spriteHeight = allTransforms[0].gameObject.GetComponentInChildren<Renderer>().bounds.extents.y;

      mapWidth  = (int) ((maxX - x) / spriteWidth);
      mapHeight = (int) ((maxY - y) / spriteHeight);

      HasWall = new bool[mapWidth + 1, mapHeight + 1];

      foreach (Transform child in AllWallObjects.transform) {
        var spriteX = (int) ((child.localPosition.x - x) / spriteWidth);
        var spriteY = (int) ((child.localPosition.y - y) / spriteHeight);

        HasWall[spriteX, spriteY] = true;

        Destroy(child.gameObject);
      }
    }

    public Vector2 WorldToMapPos(Vector2 worldPos) {
      return new Vector2(
        (int) ((worldPos.x - x) / spriteWidth),
        (int) ((worldPos.y - y) / spriteHeight)
      );
    }

    public List<Vector2> PathFind(Vector2 startWorld, Vector2 stopWorld) {
      var start = WorldToMapPos(startWorld);
      var stop  = WorldToMapPos(stopWorld);

      return new List<Vector2> {
        start,
        stop,
      };
    }
  }
}