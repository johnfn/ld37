using UnityEngine;

public static class Extensions
{
   public static void SetX(this Transform transform, float x)
   {
      Vector3 newPosition =
         new Vector3(x, transform.position.y, transform.position.z);

      transform.position = newPosition;
   }
}