using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace AillieoUtils
{
    public class RaycastTargetDetector : MonoBehaviour
    {
        public bool display = true;
        static readonly Vector3[] fourCorners = new Vector3[4];
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!display)
                return;
            foreach (MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>())
            {
                if (g.raycastTarget)
                {
                    RectTransform rectTransform = g.transform as RectTransform;
                    rectTransform.GetWorldCorners(fourCorners);
                    Gizmos.color = Color.blue;
                    for (int i = 0; i < 4; i++)
                        Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
                    for (int i = 0; i < 2; i++)
                        Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 2)]);

                }
            }
        }
#endif
    }
}

