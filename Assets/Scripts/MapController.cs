using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace johnfn {
  public class AStarNode {
    public AStarNode CameFrom;

    public Vector2 Position;

    public int Distance;

    public float HeuristicDistanceLeft;

    public bool ExpandedFromHereYet;
  }

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
      var allTransforms = new List<Transform>();

      foreach (Transform child in AllWallObjects.transform) {
        allTransforms.Add(child);
      }

      x = allTransforms.Min(_ => _.localPosition.x);
      y = allTransforms.Min(_ => _.localPosition.y);

      maxX = allTransforms.Max(_ => _.localPosition.x);
      maxY = allTransforms.Max(_ => _.localPosition.y);

      spriteWidth = allTransforms[0].gameObject.GetComponentInChildren<Renderer>().bounds.size.x;
      spriteHeight = allTransforms[0].gameObject.GetComponentInChildren<Renderer>().bounds.size.y;

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

    public Vector2 MapToWorldPos(Vector2 mapPos) {
      return new Vector2(
        (float) (mapPos.x * spriteWidth + x),
        (float) (mapPos.y * spriteWidth + y)
      );
    }

    public List<Vector2> PathFind(Vector2 startWorld, Vector2 stopWorld) {
      var start = WorldToMapPos(startWorld);
      var goal  = WorldToMapPos(stopWorld);

      var aStarNodes = new List<AStarNode> {
        new AStarNode {
          Distance = 1,
          Position = start,
          CameFrom = null,
          HeuristicDistanceLeft = (start - goal).magnitude,
          ExpandedFromHereYet = false,
        }
      };

      var dxdy = new List<Vector2> {
        new Vector2(0 , -1),
        new Vector2(0 ,  1),
        new Vector2(-1,  0),
        new Vector2(1 ,  0),
      };

      while (true) {
        if (aStarNodes.Find(x => x.Position == goal) != null) {
          break;
        }

        if (aStarNodes.Count == 0) {
          Debug.Log("Oh god... no... NO!!!");

          break;
        }

        var current = aStarNodes
          .Where(_ => !_.ExpandedFromHereYet)
          .MinBy(_ => _.Distance + _.HeuristicDistanceLeft);
        var neighborDistance = current.Distance + 1;

        current.ExpandedFromHereYet = true;

        foreach (var diff in dxdy) {
          var neighborPosition = current.Position + diff;
          var neighborLookup = aStarNodes.Find(_ => _.Position == neighborPosition);
          var newNode = new AStarNode {
            Distance = neighborDistance,
            Position = neighborPosition,
            CameFrom = current,
            HeuristicDistanceLeft = (neighborPosition - goal).magnitude,
            ExpandedFromHereYet = false,
          };

          if (neighborPosition.x < 0 || neighborPosition.y < 0 ||
              neighborPosition.x >= mapWidth || neighborPosition.y >= mapHeight) {
            continue;
          }

          if (HasWall[(int) neighborPosition.x, (int) neighborPosition.y]) {
            continue;
          }

          if (neighborLookup == null) {
            aStarNodes.Add(newNode);
          } else if (neighborLookup.Distance > neighborDistance) {
            aStarNodes.Remove(neighborLookup);
            aStarNodes.Add(newNode);
          }
        }
      }

      var end = aStarNodes.Find(x => x.Position == goal);

      if (end == null) {
        return new List<Vector2>();
      }

      var result = new List<AStarNode> {
        end
      };

      while (result.Last().Position != start) {
        result.Add(result.Last().CameFrom);
      }

      return result.Select(_ => MapToWorldPos(_.Position)).Reverse().ToList();
    }
  }
}