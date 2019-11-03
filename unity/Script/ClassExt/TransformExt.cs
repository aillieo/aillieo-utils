using UnityEngine;

namespace AillieoUtils
{
    public static class TransformExt
    {

        public static void SetLayer(this Transform trans, int layer)
        {
            trans.gameObject.layer = layer;
            for (int i = 0, imax = trans.childCount; i < imax; i++)
            {
                SetLayer(trans.GetChild(i), layer);
            }
        }

        public static Transform FindRecursive(this Transform transform, string name)
        {
            foreach (Transform t in transform)
            {
                if (t.name == name)
                {
                    return t;
                }
                else
                {
                    Transform result = t.FindRecursive(name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

    }
}
