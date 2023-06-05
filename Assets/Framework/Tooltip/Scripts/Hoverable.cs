using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Tooltip
{
    public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private bool useTooltip = true;

        private bool isUI = false;
        private RectTransform rect;
        private bool isOverUIObject = false;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            isUI = rect != null;
        }

        public void OnMouseEnter()
        {
            if (!isUI && useTooltip) TooltipControl.Instance.Show();
        }

        public void OnMouseExit()
        {
            if (!isUI && useTooltip) TooltipControl.Instance.Hide();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipControl.Instance.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipControl.Instance.Hide();
        }
    }
}
