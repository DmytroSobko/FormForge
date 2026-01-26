using System.Collections.Generic;
using UnityEngine;

namespace FormForge.Utils
{
    /// <summary>
    /// Provides utility functions for unity gameobjects.
    /// </summary>
    public static class GameObjectUtils
    {
        /// <summary>
        /// Finds child objects by name recursively.
        /// </summary>
        static public Transform RecursiveFind(Transform source, string name, bool ignoreRoot = false)
        {
            List<Transform> newTransforms = new List<Transform>();
            List<Transform> transforms = new List<Transform>();

            transforms.Add(source);

            while (transforms.Count > 0)
            {
                for (int i = 0; i < transforms.Count; i++)
                {
                    if (string.Equals(transforms[i].name, name))
                    {
                        if (!ignoreRoot || source != transforms[i])
                        {
                            return transforms[i];
                        }
                    }
                    for (int j = 0; j < transforms[i].childCount; j++)
                    {
                        newTransforms.Add(transforms[i].GetChild(j));
                    }
                }

                transforms = newTransforms;
                newTransforms = new List<Transform>();
            }

            return null;
        }

        /// <summary>
        /// Sets the layer of a game object and all its children.
        /// </summary>
        public static void SetLayerRecursive(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            for (int i = 0; i < t.childCount; i++)
            {
                SetLayerRecursive(t.GetChild(i), layer);
            }
        }

    }
}
