using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Cursor
{
    /// <summary>
    /// A ScriptableObject containing images which can be used for the mouse cursor and the IDs needed to retrieve them.
    /// </summary>
    [CreateAssetMenu(fileName = "Cursor Options", menuName = "Framework/Cursor")]
    public class CursorOptions : ScriptableObject
    {
        /// <summary>
        /// The list of different key-value pairs used to store and retrieve different cursor images.
        /// </summary>
        [SerializeField]
        private List<CursorOption> cursorOptions;

        /// <summary>
        /// A key-value pair of an ID and image which can be used to switch the cursor image.
        /// </summary>
        [Serializable]
        private struct CursorOption
        {
            /// <summary>
            /// The ID of the cursor image. This is used to retrieve the Texture2D to display on the cursor.
            /// </summary>
            public string cursorID;
            /// <summary>
            /// The image which can be used for the cursor.
            /// </summary>
            public Texture2D cursorImage;
        }

        /// <summary>
        /// Gets the texture with the given ID, if one exists in the list.
        /// </summary>
        /// <param name="cursorID">The ID of the texture to retrieve.</param>
        /// <returns>The Texture2D which corresponds to the provided ID. If the ID has no Texture2D associated with it, this returns null.</returns>
        public Texture2D GetTexture(string cursorID)
        {
            return cursorOptions.Find(option => option.cursorID.Equals(cursorID)).cursorImage;
        }
    }
}
