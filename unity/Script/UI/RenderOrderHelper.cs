using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AillieoUtils
{

    public class RenderOrderHelper : MonoBehaviour
    {

        [SerializeField]
        private int m_order;

        public int order
        {
            get { return m_order; }
            set
            {
                if (m_order != value)
                {
                    m_order = value;
                    UpdateOrder();
                }
            }
        }


        void Start()
        {
            if (m_order != 0)
            {
                UpdateOrder();
            }
        }


        void UpdateOrder()
        {
            if (GetComponent<RectTransform>() != null || GetComponent<Graphics>() != null)
            {
                // 认为是UI
                Canvas c = Utils.GetOrAddComponent<Canvas>(gameObject);
                c.overrideSorting = true;
                c.sortingOrder = order;
                if (GetComponent<GraphicRaycaster>() == null)
                {
                    gameObject.AddComponent<GraphicRaycaster>();
                }
            }
            else
            {
                Renderer[] renders = GetComponentsInChildren<Renderer>(true);
                foreach (Renderer render in renders)
                {
                    render.sortingOrder = order;
                }
            }
        }
    }
}