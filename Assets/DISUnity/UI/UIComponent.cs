using UnityEngine;
using System.Collections;

namespace DISUnity.UI
{
    /// <summary>
    /// Base class for all DISUnity runtime GUI components.
    /// </summary>
    public abstract class UIComponent : MonoBehaviour
    {
        #region Properties

        #region Private

        [SerializeField]
        private bool isVisible = true;

        [SerializeField]
        private GUISkin skin;

        #endregion

        /// <summary>
        /// Is the window currently visible?
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
            }
        }
        
        /// <summary>
        /// The GUI skin to use.
        /// </summary>
        public GUISkin Skin
        {
            get
            {
                return skin;
            }
            set
            {
                skin = value;
            }
        }

        #endregion

        /// <summary>
        /// Draw the GUI component.
        /// </summary>
        private void OnGUI()
        {
            if( isVisible )
            {
                if( skin != null )
                {
                    GUI.skin = skin;
                }
                DrawGUI();
            }
        }

        /// <summary>
        /// Do the GUI drawing
        /// </summary>
        protected abstract void DrawGUI();
    }
}