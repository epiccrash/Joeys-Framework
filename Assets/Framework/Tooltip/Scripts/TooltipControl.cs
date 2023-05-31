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

        // Actions
        public static Action HideAction;
        public static Action ShowAction;

        // RectTransform component
        private RectTransform rect;

        public bool IsVisible => gameObject.activeSelf;

        public override void Awake()
        {
            rect = GetComponent<RectTransform>();
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

                // Basic relative mouse positioning
                finalPosition.x += rectSize.x / 2 * defaultXPosition;
                finalPosition.y += rectSize.y / 2 * defaultYPosition;

                // Flipping the tooltip at screen boundaries
                if (flipPositionAtScreenBoundaries)
                {
                    if (defaultXPosition < 0 && mousePosition.x - rectSize.x <= 0) finalPosition.x += rectSize.x * Mathf.Abs(defaultXPosition);
                    else if (defaultXPosition > 0 && mousePosition.x + rectSize.x >= Screen.width) finalPosition.x -= rectSize.x * Mathf.Abs(defaultXPosition);

                    if (defaultYPosition < 0 && mousePosition.y - rectSize.y <= 0) finalPosition.y += rectSize.y * Mathf.Abs(defaultYPosition);
                    else if (defaultYPosition > 0 && mousePosition.y + rectSize.y >= Screen.height) finalPosition.y -= rectSize.y * Mathf.Abs(defaultYPosition);
                }

                // Placing the tooltip around the mouse cursor's margin
                if (finalPosition.x < mousePosition.x) finalPosition.x -= leftMargin;
                else if (finalPosition.x > mousePosition.x) finalPosition.x += rightMargin;

                if (finalPosition.y < mousePosition.y) finalPosition.y -= bottomMargin;
                else if (finalPosition.y > mousePosition.y) finalPosition.y += topMargin;

                // Bounding the tooltip within the screen
                if (!allowXOffscreenOverflow) finalPosition.x = Mathf.Clamp(finalPosition.x, leftPadding + rectSize.x / 2, Screen.width - (rightPadding + rectSize.x / 2));
                if (!allowYOffscreenOverflow) finalPosition.y = Mathf.Clamp(finalPosition.y, bottomPadding + rectSize.y / 2, Screen.height - (topPadding + rectSize.y / 2));

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
