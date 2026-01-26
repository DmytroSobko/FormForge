using System.Collections.Generic;
using UnityEngine;

namespace FormForge.Utils
{
    /// <summary>
    /// Represents a node in the quadtree.
    /// </summary>
    public class QuadNode
    {
        public List<GameObject> Contents = new List<GameObject>();
        public QuadNode[] Children = null;
        public Rect Rect = new Rect();
        public bool Closed = false;
        public int TotalNumVerts = 0;

        /// <summary>
        /// Divides the current node into four child nodes.
        /// </summary>
        public void Divide()
        {
            Children = new QuadNode[4];

            float halfWidth = Rect.width * 0.5f;
            float halfHeight = Rect.height * 0.5f;

            Children[0] = new QuadNode();
            Children[0].Rect = new Rect(Rect.xMin, Rect.yMin, halfWidth, halfHeight);

            Children[1] = new QuadNode();
            Children[1].Rect = new Rect(Rect.xMin + halfWidth, Rect.yMin, halfWidth, halfHeight);

            Children[2] = new QuadNode();
            Children[2].Rect = new Rect(Rect.xMin, Rect.yMin + halfHeight, halfWidth, halfHeight);

            Children[3] = new QuadNode();
            Children[3].Rect = new Rect(Rect.xMin + halfWidth, Rect.yMin + halfHeight, halfWidth, halfHeight);
        }

        /// <summary>
        /// Checks if a point is contained within the node's rectangle.
        /// </summary>
        public bool Contains(Vector2 point)
        {
            if (point.x > Rect.xMin && point.x < Rect.xMax && point.y > Rect.yMin && point.y < Rect.yMax)
            {
                return true;
            }
            return false;
        }
    }
}
