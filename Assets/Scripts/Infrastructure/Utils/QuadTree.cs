using System.Collections.Generic;
using UnityEngine;

namespace FormForge.Utils
{
    /// <summary>
    /// Represents a quadtree structure for spatial partitioning.
    /// </summary>
    public class QuadTree
    {
        private List<GameObject> m_theThingsToCheck = new List<GameObject>();
        private List<QuadNode> m_leafNodes = new List<QuadNode>();
        private QuadNode m_treeRoot = null;

        /// <summary>
        /// Draws the quadtree visualization when the object is selected in the editor.
        /// </summary>
        public void OnDrawGizmosSelected()
        {
            VisualizeTree(m_treeRoot);
        }

        /// <summary>
        /// Recursively visualizes the quadtree nodes.
        /// </summary>
        public void VisualizeTree(QuadNode node)
        {
            Gizmos.color = (!node.Closed) ? Color.green : Color.blue;
            Gizmos.DrawWireCube(new Vector3(node.Rect.center.x, 0.0f, node.Rect.center.y), new Vector3(node.Rect.width, 1.0f, node.Rect.height));
            Gizmos.color = Color.white;

            if (node.Children != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    VisualizeTree(node.Children[i]);
                }
            }
        }

        /// <summary>
        /// Calculates the bounding rectangle that contains all objects.
        /// </summary>
        public Rect CalcRectContainsAllThings()
        {
            float xMin = 0;
            float xMax = 0;
            float zMin = 0;
            float zMax = 0;

            for (int i = 0; i < m_theThingsToCheck.Count; i++)
            {
                xMin = Mathf.Min(xMin, m_theThingsToCheck[i].transform.position.x);
                zMin = Mathf.Min(zMin, m_theThingsToCheck[i].transform.position.z);

                xMax = Mathf.Max(xMax, m_theThingsToCheck[i].transform.position.x);
                zMax = Mathf.Max(zMax, m_theThingsToCheck[i].transform.position.z);
            }
            return new Rect(xMin, zMin, (Mathf.Abs(xMin) + Mathf.Abs(xMax)), (Mathf.Abs(zMin) + Mathf.Abs(zMax)));
        }

        /// <summary>
        /// Calculates the quadtree structure.
        /// </summary>
        public QuadNode CalculateQuadTree()
        {
            m_treeRoot = new QuadNode();
            m_treeRoot.Rect = CalcRectContainsAllThings();
            CalculateQuadTree(m_treeRoot);
            return m_treeRoot;
        }

        /// <summary>
        /// Recursively calculates the quadtree structure for a given node.
        /// </summary>
        public void CalculateQuadTree(QuadNode node)
        {
            // this is the check to decide if it should divide, should be replaced with somethign else, vert count probably
            if (NeedsToDivideFromVerts(node, 1))
            {
                node.Divide();
                for (int i = 0; i < 4; i++)
                {
                    CalculateQuadTree(node.Children[i]);
                }
            }
        }

        /// <summary>
        /// Determines if a node needs to be divided based on the number of vertices.
        /// </summary>
        public bool NeedsToDivideFromVerts(QuadNode node, int numVerts)
        {
            for (int i = 0; i < m_theThingsToCheck.Count; i++)
            {
                bool bContains = node.Contains(new Vector2(m_theThingsToCheck[i].transform.position.x, m_theThingsToCheck[i].transform.position.z));

                if (bContains)
                {
                    node.Contents.Add(m_theThingsToCheck[i]);
                    MeshFilter mf = m_theThingsToCheck[i].GetComponent<MeshFilter>();
                    if (mf != null)
                    {
                        node.TotalNumVerts += mf.sharedMesh.vertexCount;
                    }
                    else
                    {
                        int numChildren = m_theThingsToCheck[i].transform.childCount;
                        for (int j = 0; j < numChildren; j++)
                        {
                            Transform transform = m_theThingsToCheck[i].transform.GetChild(j);
                            if (transform.gameObject.name.EndsWith("_Lit"))
                            {
                                node.TotalNumVerts += transform.gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount;
                            }
                        }
                    }
                }

                if (node.TotalNumVerts > 65000)
                {
                    node.Contents.Clear(); //we want this node to be cleared so we can add them to the divided/children nodes
                    node.Closed = true;
                    node.TotalNumVerts = -1;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the leaf nodes of the quadtree.
        /// </summary>
        public List<QuadNode> GetLeafNodes()
        {
            GetLeafNodes(m_treeRoot);
            return m_leafNodes;
        }

        private void GetLeafNodes(QuadNode node)
        {
            if (node.Closed)
            {
                for (int i = 0; i < node.Children.Length; i++)
                {
                    GetLeafNodes(node.Children[i]);
                }
            }
            else
            {
                m_leafNodes.Add(node);
            }
        }

        /// <summary>
        /// Determines if a node needs to be divided based on the number of contents.
        /// </summary>
        public bool NeedsToDivideFromPoints(QuadNode node, int numContents)
        {
            // check to see if there are more than numContents in the node, if so it needs dividing
            for (int i = 0; i < m_theThingsToCheck.Count; i++)
            {
                bool bContains = node.Contains(new Vector2(m_theThingsToCheck[i].transform.position.x, m_theThingsToCheck[i].transform.position.z));

                if (bContains)
                {
                    node.Contents.Add(m_theThingsToCheck[i]);
                }

                if (node.Contents.Count > numContents)
                {
                    node.Contents.Clear(); //we want this node to be cleared so we can add them to the divided/children nodes
                    node.Closed = true;
                    return true;
                }
            }
            return false;
        }
    }
}