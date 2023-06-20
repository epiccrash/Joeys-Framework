using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Cursor
{
    /// <summary>
    /// Master class for controlling the cursor. This can be placed on an object in a scene and then referenced by other scripts throughout the app to manipulate the cursor.
    /// </summary>
    public class CursorControl : Singleton<CursorControl>
    {
        /// <summary>
        /// The ScriptableObject holding key-value pairs for the cursor options.
        /// </summary>
        [Header("Cursor Options ScriptableObject Reference")]
        [SerializeField]
        private CursorOptions cursorOptions;

        /// <summary>
        /// An action called when the cursor is swapped.
        /// </summary>
        public static Action OnSwapCursor;
        /// <summary>
        /// An action called when the cursor is hidden.
        /// </summary>
        public static Action OnHideCursor;
        /// <summary>
        /// An action called when the cursor is shown.
        /// </summary>
        public static Action OnShowCursor;
        /// <summary>
        /// An action called when the cursor is marked free.
        /// </summary>
        public static Action OnFreeCursor;
        /// <summary>
        /// An action called when the cursor is locked to the center of the game window.
        /// </summary>
        public static Action OnCenterLockCursor;
        /// <summary>
        /// An action called when the cursor is locked to the boundaries of the game window.
        /// </summary>
        public static Action OnBoundaryLockCursor;

        /// <summary>
        /// Swaps the cursor from one image to another based on the ID specified. If an invalid ID is specified, the cursor becomes its system default.
        /// </summary>
        /// <param name="cursorID">The identifier for the image to use as a cursor.</param>
        /// <param name="hotspotX">The x coordinate of the hotspot used for the cursor. In most cases this can just be 0, and so does not need to be specified.</param>
        /// <param name="hotspotY">The y coordinate of the hotspot used for the cursor. In most cases this can just be 0, and so does not need to be specified.</param>
        /// <param name="cursorMode">The mode to use for the cursor. This is set to CursorMode.Auto by default, rather than forcing software cursor rendering.</param>
        public void SwapCursor(string cursorID, float hotspotX = 0.0f, float hotspotY = 0.0f, CursorMode cursorMode = CursorMode.ForceSoftware)
        {
            UnityEngine.Cursor.SetCursor(cursorOptions.GetTexture(cursorID), new Vector2(hotspotX, hotspotY), cursorMode);
            OnSwapCursor?.Invoke();
        }

        /// <summary>
        /// Hides the cursor.
        /// </summary>
        public void HideCursor()
        {
            UnityEngine.Cursor.visible = false;
            OnHideCursor?.Invoke();
        }

        /// <summary>
        /// Shows the cursor.
        /// </summary>
        public void ShowCursor()
        {
            UnityEngine.Cursor.visible = true;
            OnShowCursor?.Invoke();
        }

        /// <summary>
        /// Frees the cursor from any lock states.
        /// </summary>
        public void FreeCursor()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            OnFreeCursor?.Invoke();
        }

        /// <summary>
        /// Locks the cursor to the center of the game window.
        /// </summary>
        public void LockCursorToCenterOfGameWindow()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            OnCenterLockCursor?.Invoke();
        }

        /// <summary>
        /// Locks the cursor to the boundaries of the game window.
        /// </summary>
        public void LockCursorToBoundariesOfGameWindow()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            OnBoundaryLockCursor?.Invoke();
        }
    }
}
