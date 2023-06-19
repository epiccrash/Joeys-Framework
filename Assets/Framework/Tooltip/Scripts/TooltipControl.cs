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
    /// <summary>
    /// Controls rendering and positioning of a tooltip. This is a tooltip's primary class.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class TooltipControl : Singleton<TooltipControl>
    {
        /// <summary>
        /// The margin of space in pixels between the bottom of this tooltip and the top of the mouse cursor.
        /// </summary>
        [Tooltip("The margin of space in pixels between the bottom of this tooltip and the top of the mouse cursor.")]
        [SerializeField]
        private float topMargin = 8.0f;
        /// <summary>
        /// The margin of space in pixels between the right side of this tooltip and the left side of the mouse cursor.
        /// </summary>
        [Tooltip("The margin of space in pixels between the right side of this tooltip and the left side of the mouse cursor.")]
        [SerializeField]
        private float leftMargin = 8.0f;
        /// <summary>
        /// The margin of space in pixels between the top of this tooltip and the bottom of the mouse cursor.
        /// </summary>
        [Tooltip("The margin of space in pixels between the top of this tooltip and the bottom of the mouse cursor.")]
        [SerializeField]
        private float bottomMargin = 8.0f;
        /// <summary>
        /// The margin of space in pixels between the left side of this tooltip and the right side of the mouse cursor.
        /// </summary>
        [Tooltip("The margin of space in pixels between the left side of this tooltip and the right side of the mouse cursor.")]
        [SerializeField]
        private float rightMargin = 8.0f;

        /// <summary>
        /// The padding in pixels between the bottom of this tooltip and the top of the screen.
        /// </summary>
        [Header("Screen Padding")]
        [Tooltip("The padding in pixels between the bottom of this tooltip and the top of the screen.")]
        [SerializeField]
        private float topPadding = 8.0f;
        /// <summary>
        /// The padding in pixels between the right side of this tooltip and the left side of the screen.
        /// </summary>
        [Tooltip("The padding in pixels between the right side of this tooltip and the left side of the screen.")]
        [SerializeField]
        private float leftPadding = 8.0f;
        /// <summary>
        /// The padding in pixels between the top of this tooltip and the bottom of the screen.
        /// </summary>
        [Tooltip("The padding in pixels between the top of this tooltip and the bottom of the screen.")]
        [SerializeField]
        private float bottomPadding = 8.0f;
        /// <summary>
        /// The padding in pixels between the left side of this tooltip and the right side of the screen.
        /// </summary>
        [Tooltip("The padding in pixels between the left side of this tooltip and the right side of the screen.")]
        [SerializeField]
        private float rightPadding = 8.0f;
        /// <summary>
        /// Whether to use the padding as percentages rather than pixels (e.g., 10.0f = 10.0%).
        /// </summary>
        [Tooltip("Whether to use the padding as percentages rather than pixels (e.g., 10.0f = 10.0%).")]
        [SerializeField]
        private bool useAsPercents = false;

        /// <summary>
        /// The default horizontal positioning of the tooltip. -1 positions this tooltip to the left of the cursor, 1 positions it to the right of the cursor.
        /// </summary>
        [Header("Positioning")]
        [Range(-1.0f, 1.0f)]
        [Tooltip("The default horizontal positioning of the tooltip. -1 positions this tooltip to the left of the cursor, 1 positions it to the right of the cursor.")]
        [SerializeField]
        private float defaultXPosition = 1.0f;
        /// <summary>
        /// The default vertical positioning of the tooltip. -1 positions this tooltip to the bottom of the cursor, 1 positions it to the top of the cursor.
        /// </summary>
        [Range(-1.0f, 1.0f)]
        [Tooltip("The default vertical positioning of the tooltip. -1 positions this tooltip to the bottom of the cursor, 1 positions it to the top of the cursor.")]
        [SerializeField]
        private float defaultYPosition = 1.0f;

        /// <summary>
        /// Whether to flip this tooltip's position when it reaches the boundaries of the screen, with padding included.
        /// </summary>
        [Header("Additional Configuration")]
        [Tooltip("Whether to flip this tooltip's position when it reaches the boundaries of the screen, with padding included.")]
        [SerializeField]
        private bool flipPositionAtScreenBoundaries = false;
        /// <summary>
        /// Whether to only update this tooltip's position once, freezing it in place after one hover over an object. This is not recommended (because it can look janky) but can be enabled.
        /// </summary>
        [Tooltip("Whether to only update this tooltip's position once, freezing it in place after one hover over an object. This is not recommended (because it can look janky) but can be enabled.")]
        [SerializeField]
        private bool lockPositionAfterOneUpdate = false;
        /// <summary>
        /// Whether to allow the tooltip to go overflow offscreen if it exceeds the horizontal screen boundaries.
        /// </summary>
        [Tooltip("Whether to allow the tooltip to go overflow offscreen if it exceeds the horizontal screen boundaries.")]
        [SerializeField]
        private bool allowXOffscreenOverflow = false;
        /// <summary>
        /// Whether to allow the tooltip to go overflow offscreen if it exceeds the vertical screen boundaries.
        /// </summary>
        [Tooltip("Whether to allow the tooltip to go overflow offscreen if it exceeds the vertical screen boundaries.")]
        [SerializeField]
        private bool allowYOffscreenOverflow = false;

        /// <summary>
        /// An action called when the tooltip is hidden. This can be useful for things like disabling visual effects on objects.
        /// </summary>
        public static Action HideAction;
        /// <summary>
        /// An action called when the tooltip is shown. This can be useful for things like enabling visual effects on objects, playing a sound, or updating tooltip properties.
        /// </summary>
        public static Action ShowAction;

        /// <summary>
        /// The RectTransform of the tooltip. Obtained in Awake.
        /// </summary>
        private RectTransform rect;
        /// <summary>
        /// The rect of the parent Canvas. Obtained in Awake.
        /// </summary>
        private RectTransform canvasRect;

        /// <summary>
        /// Accessor that gets whether this tooltip is visible.
        /// </summary>
        public bool IsVisible => gameObject.activeSelf;

        /// <summary>
        /// Gets needed components when this object is first created.
        /// </summary>
        public override void Awake()
        {
            // Get components
            rect = GetComponent<RectTransform>();
            canvasRect = GetComponentInParent<CanvasScaler>().GetComponent<RectTransform>();

            // Disable raycasts on the tooltip so it can be more accurately detected
            CanvasGroup group = GetComponent<CanvasGroup>();
            group.blocksRaycasts = false;

            // Call the base Singleton Awake
            base.Awake();
        }

        /// <summary>
        /// Hides the tooltip on startup.
        /// </summary>
        private void Start()
        {
            Hide();
        }

        /// <summary>
        /// Updates the position of the tooltip every frame, unless set to lock the tooltip's position after one update.
        /// </summary>
        /// <returns>A yield return every frame, while this tooltip is active.</returns>
        private IEnumerator UpdatePosition()
        {
            bool singleUpdateFired = false;
            while (true)
            {
                // Lock the tooltip's position after one update if enabled
                if (lockPositionAfterOneUpdate)
                {
                    if (!singleUpdateFired)
                    {
                        SetPosition();
                        singleUpdateFired = true;
                    }
                }
                // Otherwise continuously update the tooltip's position
                else
                {
                    SetPosition();
                    singleUpdateFired = false;
                }

                yield return null;
            }
        }

        /// <summary>
        /// Sets the position of this tooltip based on the mouse position.
        /// </summary>
        private void SetPosition()
        {
            // Get the current position of the mouse
            Vector2 mousePosition = Input.mousePosition;

            // If the mouse is within bounds, update the tooltip's position
            if (mousePosition.x >= 0 && mousePosition.x <= Screen.width && mousePosition.y >= 0 && mousePosition.y <= Screen.height)
            {
                Vector2 rectSize = rect.sizeDelta;
                Vector2 finalPosition = mousePosition;

                // Set scaling properties
                Vector2 scaledPosition = new Vector2(rectSize.x / 2 * canvasRect.localScale.x, rectSize.y / 2 * canvasRect.localScale.y);
                float scaledTopPadding = topPadding * canvasRect.localScale.y;
                float scaledLeftPadding = leftPadding * canvasRect.localScale.x;
                float scaledBottomPadding = bottomPadding * canvasRect.localScale.y;
                float scaledRightPadding = rightPadding * canvasRect.localScale.x;
                float scaledSizeDeltaX = rectSize.x * canvasRect.localScale.x;
                float scaledSizeDeltaY = rectSize.y * canvasRect.localScale.y;

                // Screen border properties
                if (useAsPercents)
                {
                    scaledTopPadding = scaledTopPadding / 100 * Screen.height;
                    scaledLeftPadding = scaledLeftPadding / 100 * Screen.width;
                    scaledBottomPadding = scaledBottomPadding / 100 * Screen.height;
                    scaledRightPadding = scaledRightPadding / 100 * Screen.width;
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
                
                // Set the new position of the tooltip
                rect.position = finalPosition;
            }
        }

        /// <summary>
        /// Hides the tooltip and invokes the hide action.
        /// <para>
        /// This function should only be called when the tooltip should be hidden. It should likely only be called from Hoverable.
        /// </para>
        /// </summary>
        public void Hide()
        {
            HideAction?.Invoke();
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows the tooltip, invokes the show action, and starts updating the position of the tooltip.
        /// <para>
        /// This function should only be called when the tooltip should be shown. It should likely only be called from Hoverable.
        /// </para>
        /// </summary>
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
