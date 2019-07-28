using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AillieoUtils.UI
{

    public class UIButtonExt : Button
    {
        public float longPressThreshold = 0.8f;

        public ButtonClickedEvent onDoubleClick = new ButtonClickedEvent();
        public ButtonClickedEvent onLongPressed = new ButtonClickedEvent();

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (eventData.clickCount == 1 && Time.unscaledTime - eventData.clickTime > longPressThreshold)
            {
                onLongPressed?.Invoke();
                Debug.LogError("长按");
                return;
            }

            if (eventData.clickCount == 2)
            {
                onDoubleClick?.Invoke();
                Debug.LogError("双击");
                return;
            }

            base.OnPointerClick(eventData);
        }
    }

}
