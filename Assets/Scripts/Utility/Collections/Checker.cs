using UnityEngine;

namespace Comma.Utility.Collections
{
    public class Checker 
    {
        /// <summary>
        /// Check whether the point is within/inside the collider
        /// </summary>
        /// <param name="col"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsWithin(Collider2D col, Vector2 point)
        {
            return col.ClosestPoint(point) == point;
        }
    }
}
