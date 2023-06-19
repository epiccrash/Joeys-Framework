using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Tooltip
{
    /// <summary>
    /// Class for any 2D or 3D object which should trigger a tooltip when hovered over. The tooltip can be disabled.
    /// </summary>
    public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Whether to display the tooltip when this object is hovered over.
        /// </summary>
        [Tooltip("Whether to display the tooltip when this object is hovered over.")]
        [SerializeField]
        private bool useTooltip = true;

        /// <summary>
        /// The RectTransform of this object, if there is one. Obtained in Awake.
        /// </summary>
        private RectTransform rect;
        /// <summary>
        /// Whether this object is a UI object. This is automatically set.
        /// </summary>
        private bool isUI = false;

        /// <summary>
        /// Gets the RectTransform of this object if UI and sets whether this is a UI object.
        /// </summary>
        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            isUI = rect != null;
        }

        /// <summary>
        /// Shows the tooltip if this is not a UI object and the tooltip is enabled when the mouse enters this object.
        /// </summary>
        public void OnMouseEnter()
        {
            if (!isUI && useTooltip) TooltipControl.Instance.Show();
        }

        /// <summary>
        /// Hides the tooltip if this is not a UI object and the tooltip is enabled when the mouse leaves this object.
        /// </summary>
        public void OnMouseExit()
        {
            if (!isUI && useTooltip) TooltipControl.Instance.Hide();
        }

        /// <summary>
        /// Shows the tooltip if this is a UI object when the mouse enters this object.
        /// </summary>
        /// <param name="eventData">The mouse entry data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipControl.Instance.Show();
        }

        /// <summary>
        /// Hides the tooltip if this is a UI object when the mouse leaves this object.
        /// </summary>
        /// <param name="eventData">The mouse exit data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipControl.Instance.Hide();
        }
    }
}
