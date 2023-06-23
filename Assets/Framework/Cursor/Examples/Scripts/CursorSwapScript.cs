using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Framework.Cursor.Examples
{
    /// <summary>
    /// Simple script used to swap the cursor's image, change its lock state, and change its visibility.
    /// </summary>
    public class CursorSwapScript : MonoBehaviour
    {
        /// <summary>
        /// The KeyCode used to show the cursor.
        /// </summary>
        [Header("Cursor Visibility KeyCodes")]
        [SerializeField]
        private KeyCode showCursorKeyCode = KeyCode.S;
        /// <summary>
        /// The KeyCode used to hide the cursor.
        /// </summary>
        [SerializeField]
        private KeyCode hideCursorKeyCode = KeyCode.H;

        /// <summary>
        /// The KeyCode used to free the cursor from a lock state.
        /// </summary>
        [Header("Cursor Lock KeyCodes")]
        [SerializeField]
        private KeyCode freeCursorKeyCode = KeyCode.F;
        /// <summary>
        /// The KeyCode used to lock the cursor to the center of the game window.
        /// </summary>
        [SerializeField]
        private KeyCode lockCursorToCenterKeyCode = KeyCode.C;
        /// <summary>
        /// The KeyCode used to lock the cursor to the boundaries of the game window.
        /// </summary>
        [SerializeField]
        private KeyCode lockCursorToBoundariesKeyCode = KeyCode.B;

        /// <summary>
        /// The TextMeshPro displaying the KeyCode used to show the cursor.
        /// </summary>
        [Header("UI References")]
        [SerializeField]
        private TextMeshProUGUI showCursorTMP;
        /// <summary>
        /// The TextMeshPro displaying the KeyCode used to hide the cursor.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI hideCursorTMP;
        /// <summary>
        /// The TextMeshPro displaying the KeyCode used to free the cursor from a lock state.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI freeCursorTMP;
        /// <summary>
        /// The TextMeshPro displaying the KeyCode used to lock the cursor to the center of the game window.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI lockCursorToCenterTMP;
        /// <summary>
        /// The TextMeshPro displaying the KeyCode used to lock the cursor to the boundaries of the game window.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI lockCursorToBoundariesTMP;

        /// <summary>
        /// Sets the TextMeshPro text based on the KeyCodes set above.
        /// </summary>
        private void Awake()
        {
            showCursorTMP.text = $"Show cursor - {showCursorKeyCode}";
            hideCursorTMP.text = $"Hide cursor - {hideCursorKeyCode}";
            freeCursorTMP.text = $"Free cursor from lock state - {freeCursorKeyCode}";
            lockCursorToCenterTMP.text = $"Lock cursor to center - {lockCursorToCenterKeyCode}";
            lockCursorToBoundariesTMP.text = $"Lock cursor to boundaries - {lockCursorToBoundariesKeyCode}";
        }

        /// <summary>
        /// Changes the cursor's lock state and/or visibility based on the key pressed.
        /// </summary>
        private void Update()
        {
            // Make the cursor visible
            if (Input.GetKeyDown(showCursorKeyCode))
            {
                CursorControl.Instance.ShowCursor();
            }
            // Hide the cursor
            else if (Input.GetKeyDown(hideCursorKeyCode))
            {
                CursorControl.Instance.HideCursor();
            }

            // Free the cursor
            if (Input.GetKeyDown(freeCursorKeyCode))
            {
                CursorControl.Instance.FreeCursor();
            }
            // Lock the cursor to the center of the game window
            else if (Input.GetKeyDown(lockCursorToCenterKeyCode))
            {
                CursorControl.Instance.LockCursorToCenterOfGameWindow();
            }
            // Lock the cursor to the boundaries of the game window
            else if (Input.GetKeyDown(lockCursorToBoundariesKeyCode))
            {
                CursorControl.Instance.LockCursorToBoundariesOfGameWindow();
            }
        }

        /// <summary>
        /// Swaps the cursor image to the ID specified. This is called in the scene.
        /// </summary>
        /// <param name="cursorId">The ID of the cursor image to use.</param>
        public void SwapCursor(string cursorId)
        {
            CursorControl.Instance.SwapCursor(cursorId);
        }
    }
}
