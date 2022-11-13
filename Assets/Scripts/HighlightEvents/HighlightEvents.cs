using UnityEngine;
using UnityEngine.EventSystems;

namespace HighlightEvents
{
    public class HighlightEvents : EventTrigger
    {
        public override void OnPointerEnter(PointerEventData data)
        {
            gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
