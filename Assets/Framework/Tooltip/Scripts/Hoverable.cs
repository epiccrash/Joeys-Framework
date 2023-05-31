using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Tooltip
{
    public class Hoverable : MonoBehaviour
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

        private void Start()
        {
            if (isUI) StartCoroutine(WatchMouseUI());
        }

        public IEnumerator WatchMouseUI()
        {
            while (true)
            {
                Vector2 mousePosition = Input.mousePosition;
                Vector2 lowerLeft = new Vector2(rect.position.x - rect.sizeDelta.x / 2, rect.position.y - rect.sizeDelta.y / 2);
                Vector2 upperRight = new Vector2(rect.position.x + rect.sizeDelta.x / 2, rect.position.y + rect.sizeDelta.y / 2);
                if (mousePosition.x >= lowerLeft.x && mousePosition.x <= upperRight.x &&
                    mousePosition.y >= lowerLeft.y && mousePosition.y <= upperRight.y)
                {
                    if (useTooltip && !isOverUIObject) TooltipControl.Instance.Show();
                    isOverUIObject = true;
                }
                else
                {
                    if (useTooltip && isOverUIObject) TooltipControl.Instance.Hide();
                    isOverUIObject = false;
                }

                yield return null;
            }
        }

        public void OnMouseEnter()
        {
            if (!isUI && useTooltip) TooltipControl.Instance.Show();
        }

        public void OnMouseExit()
        {
            if (!isUI && useTooltip) TooltipControl.Instance.Hide();
        }
    }
}
