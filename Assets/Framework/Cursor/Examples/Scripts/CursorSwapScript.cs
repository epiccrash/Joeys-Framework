using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Cursor.Examples
{
    /// <summary>
    /// Simple script used to swap the cursor's image, change its lock state, and change its visibility.
    /// </summary>
    public class CursorSwapScript : MonoBehaviour
    {
        /// <summary>
        /// Changes the cursor's lock state and/or visibility based on the key pressed.
        /// </summary>
        private void Update()
        {
            // Free the cursor by pressing A
            if (Input.GetKeyDown(KeyCode.A))
            {
                CursorControl.Instance.FreeCursor();
            }
            // Lock the cursor to the center of the game window by pressing B
            else if (Input.GetKeyDown(KeyCode.B))
            {
                CursorControl.Instance.LockCursorToCenterOfGameWindow();
            }
            // Lock the cursor to the boundaries of the game window by pressing C
            else if (Input.GetKeyDown(KeyCode.C))
            {
                CursorControl.Instance.LockCursorToBoundariesOfGameWindow();
            }

            // Make the cursor visible
            if (Input.GetKeyDown(KeyCode.V))
            {
                CursorControl.Instance.ShowCursor();
            }
            // Hide the cursor
            else if (Input.GetKeyDown(KeyCode.H))
            {
                CursorControl.Instance.HideCursor();
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
