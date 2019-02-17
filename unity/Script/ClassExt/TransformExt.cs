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

    }
}
