using UnityEngine;
using System.Collections;
using System;
using DISUnity.DataType;

namespace DISUnity.Attributes
{
    /// <summary>
    /// Attribute for custom label and tooltip.    
    /// </summary>
    public class LabelAttribute : PropertyAttribute
    {
        // The label
        public GUIContent label;

        // Type of object the label belongs to, can be used to find the property drawer.
        public Type genericType;

        /// <summary>
        /// Default Ctor
        /// </summary>
        protected LabelAttribute()
        {
        }
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name">Name in inspector</param>
        /// <param name="tooltip">Tooltip for label</param>
        /// <param name="genericType">If the property is a class then this is the class to use based on the CustomPropertyDrawer attribute type.</param>
        public LabelAttribute( string name, string tooltip = "", Type genericType = null )
        {
            label = new GUIContent( name, tooltip );
            this.genericType = genericType;            
        }
    }
}