using Shared;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Framework.Tooltip
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class TooltipControl : Singleton<TooltipControl>
    {
        [Header("Mouse Margin")]
        [SerializeField]
        private float topMargin = 8.0f;
        [SerializeField]
        private float leftMargin = 8.0f;
        [SerializeField]
        private float bottomMargin = 8.0f;
        [SerializeField]
        private float rightMargin = 8.0f;

        [Header("Screen Padding")]
        [SerializeField]
        private float topPadding = 8.0f;
        [SerializeField]
        private float leftPadding = 8.0f;
        [SerializeField]
        private float bottomPadding = 8.0f;
        [SerializeField]
        private float rightPadding = 8.0f;
        [SerializeField]
        private bool useAsPercents = false;

        [Header("Positioning")]
        [Range(-1.0f, 1.0f)]
        [SerializeField]
        private float defaultXPosition = 1.0f;
        [Range(-1.0f, 1.0f)]
        [SerializeField]
        private float defaultYPosition = 1.0f;

        [Header("Configuration")]
        [SerializeField]
        private bool lockPositionAfterOneUpdate = false;
        [SerializeField]
        private bool flipPositionAtScreenBoundaries = false;
        [SerializeField]
        private bool allowXOffscreenOverflow = false;
        [SerializeField]
        private bool allowYOffscreenOverflow = false;

        private RectTransform canvasRect;

        // Actions
        public static Action HideAction;
        public static Action ShowAction;

        // RectTransform component
        private RectTransform rect;

        public bool IsVisible => gameObject.activeSelf;

        public override void Awake()
        {
            rect = GetComponent<RectTransform>();
            canvasRect = GetComponentInParent<CanvasScaler>().GetComponent<RectTransform>();

            CanvasGroup group = GetComponent<CanvasGroup>();
            group.blocksRaycasts = false;

            base.Awake();
        }

        private void Start()
        {
            Hide();
        }

        private IEnumerator UpdatePosition()
        {
            bool singleUpdateFired = false;
            while (true)
            {
                if (lockPositionAfterOneUpdate)
                {
                    if (!singleUpdateFired)
                    {
                        SetPosition();
                        singleUpdateFired = true;
                    }
                }
                else
                {
                    SetPosition();
                    singleUpdateFired = false;
                }

                yield return null;
            }
        }

        private void SetPosition()
        {
            Vector2 mousePosition = Input.mousePosition;

            if (mousePosition.x >= 0 && mousePosition.x <= Screen.width && mousePosition.y >= 0 && mousePosition.y <= Screen.height)
            {
                Vector2 rectSize = rect.sizeDelta;
                Vector2 finalPosition = mousePosition;

                // Set scaling properties
                Vector2 scaledPosition = new Vector2(rectSize.x / 2 * canvasRect.localScale.x, rectSize.y / 2 * canvasRect.localScale.y);
                float scaledLeftPadding = leftPadding * canvasRect.localScale.x;
                float scaledRightPadding = rightPadding * canvasRect.localScale.x;
                float scaledBottomPadding = bottomPadding * canvasRect.localScale.y;
                float scaledTopPadding = topPadding * canvasRect.localScale.y;
                float scaledSizeDeltaX = rectSize.x * canvasRect.localScale.x;
                float scaledSizeDeltaY = rectSize.y * canvasRect.localScale.y;

                // Screen border properties
                if (useAsPercents)
                {
                    scaledLeftPadding = scaledLeftPadding / 100 * Screen.width;
                    scaledRightPadding = scaledRightPadding / 100 * Screen.width;
                    scaledBottomPadding = scaledBottomPadding / 100 * Screen.height;
                    scaledTopPadding = scaledTopPadding / 100 * Screen.height;
                }

                // Basic relative mouse positioning
                finalPosition.x += scaledPosition.x * defaultXPosition;
                finalPosition.y += scaledPosition.y * defaultYPosition;

                // Flipping the tooltip at screen boundaries
                if (flipPositionAtScreenBoundaries)
                {
                    if (defaultXPosition < 0 && mousePosition.x - scaledSizeDeltaX - leftMargin <= scaledLeftPadding) finalPosition.x += scaledSizeDeltaX * Mathf.Abs(defaultXPosition);
                    else if (defaultXPosition > 0 && mousePosition.x + scaledSizeDeltaX + rightMargin >= Screen.width - scaledRightPadding) finalPosition.x -= scaledSizeDeltaX * Mathf.Abs(defaultXPosition);

                    if (defaultYPosition < 0 && mousePosition.y - scaledSizeDeltaY - bottomMargin <= scaledBottomPadding) finalPosition.y += scaledSizeDeltaY * Mathf.Abs(defaultYPosition);
                    else if (defaultYPosition > 0 && mousePosition.y + scaledSizeDeltaY + topMargin >= Screen.height - scaledTopPadding) finalPosition.y -= scaledSizeDeltaY * Mathf.Abs(defaultYPosition);
                }

                // Placing the tooltip around the mouse cursor's margin
                if (finalPosition.x < mousePosition.x) finalPosition.x -= leftMargin;
                else if (finalPosition.x > mousePosition.x) finalPosition.x += rightMargin;

                if (finalPosition.y < mousePosition.y) finalPosition.y -= bottomMargin;
                else if (finalPosition.y > mousePosition.y) finalPosition.y += topMargin;

                // Bounding the tooltip within the screen
                if (!allowXOffscreenOverflow) finalPosition.x = Mathf.Clamp(finalPosition.x, scaledLeftPadding + scaledPosition.x, Screen.width - scaledRightPadding - scaledPosition.x);
                if (!allowYOffscreenOverflow) finalPosition.y = Mathf.Clamp(finalPosition.y, scaledBottomPadding + scaledPosition.y, Screen.height - scaledTopPadding - scaledPosition.y);
                
                rect.position = finalPosition;
            }
        }

        public void Hide()
        {
            HideAction?.Invoke();
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            // failsafe
            StopAllCoroutines();
            StartCoroutine(UpdatePosition());
            ShowAction?.Invoke();
        }
    }
}
