using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using System.Linq;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using System;
using DISUnity.Attributes;
using System.Reflection;
using System.Collections.Generic;

namespace DISUnity.Editor.Attributes
{
    // TODO: Remove this class now Unity has added the ToolTip attribute.

    /// <summary>
    /// Draws primitive editors with a custom label that can contain both a name and tooltip.
    /// If the type is generic then the PropertyDrawer for that type will be found and invoked with the custom label.
    /// </summary>
    [CustomPropertyDrawer( typeof( LabelAttribute ) )]
    public class LabelPropertyDrawer : PropertyDrawer
    {
        #region Properties

        /// <summary>
        /// The PropertyDrawer for this type
        /// </summary>
        private PropertyDrawer genericDrawer;
        protected PropertyDrawer GenericDrawer
        {
            get
            {
                if( genericDrawer == null )
                {
                    genericDrawer = GetGenericDrawer();
                }
                
                return genericDrawer;
            }
        }

        /// <summary>
        /// The label attribute.
        /// </summary>
        private LabelAttribute labelAtt;
        protected LabelAttribute LabelAttribute
        {
            get
            {
                if( labelAtt == null )
                {
                    labelAtt = attribute as LabelAttribute;
                }              

                return labelAtt;
            }
        }

        #endregion Properties

        /// <summary>
        /// Current height of this property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            if( property.propertyType == SerializedPropertyType.Generic && GenericDrawer != null )
            {
                return GenericDrawer.GetPropertyHeight( property, label );
            }

            //Get the base height when not expanded
            float height = base.GetPropertyHeight( property, label );

            // if the property is expanded go through all its children and get their height
            if( property.isExpanded )
            {
                IEnumerator propEnum = property.GetEnumerator();
                while( propEnum.MoveNext() )
                {
                    height += EditorGUI.GetPropertyHeight( ( SerializedProperty )propEnum.Current, GUIContent.none, true );
                }
            }

            return height;                  
        }

        /// <summary>
        /// Finds the property drawer for this generic type using reflection
        /// </summary>
        /// <returns></returns>
        private PropertyDrawer GetGenericDrawer()
        {
            // Examine every type and look for:
            //  -inherit from PropertyDrawer
            //  -have attribute CustomPropertyDrawer with type that matches generic type
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach( Type typ in types )
            {
                // Register each of the found classes
                if( typ.IsSubclassOf( typeof( PropertyDrawer ) ) )
                {
                    Attribute a = Attribute.GetCustomAttribute( typ, typeof( CustomPropertyDrawer ) );
                    if( a != null )
                    {
                        // Get the private variable type
                        CustomPropertyDrawer c = a as CustomPropertyDrawer;
                        FieldInfo fi = c.GetType().GetField( "type", BindingFlags.Instance | BindingFlags.NonPublic );                     
                        Type found = ( Type )fi.GetValue( c );
                        if( LabelAttribute.genericType == found )
                        {
                            // We have found the drawer, create an instance and return it.
                            return ( PropertyDrawer )Activator.CreateInstance( typ );
                        }
                    }                
                }
            }

            if( LabelAttribute != null && LabelAttribute.genericType  != null )
            {
                Debug.LogWarning( "Could not find PropertyDrawer for type: " + LabelAttribute.genericType.ToString() );
            }
            return null;
        }

        /// <summary>
        /// Returns custom label if one exists, if not returns current.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public GUIContent GetLabel( GUIContent current )
        {
            if( attribute != null && attribute is LabelAttribute )
            {
                if( LabelAttribute.label != null )
                {
                    return LabelAttribute.label;
                }
            }

            return current;
        }

        /// <summary>
        /// Draw property
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            label = GetLabel( label );

            if( property.propertyType == SerializedPropertyType.Generic && GenericDrawer != null )
            {
                GenericDrawer.OnGUI( position, property, label );
                return;             
            }
         
            // Use default property drawer.
            Dictionary<string, PropertyDrawer> s_dictionary = typeof( PropertyDrawer ).GetField( "s_PropertyDrawers", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic ).GetValue( null ) as Dictionary<string, PropertyDrawer>;                
            foreach( KeyValuePair<string, PropertyDrawer> entry in s_dictionary )
            {                    
                if( entry.Value == this )
                {
                    // Remove this property drawer.
                    s_dictionary[entry.Key] = null;
                    EditorGUI.PropertyField( position, property, label, true );
                    s_dictionary[entry.Key] = this;
                    return;
                }
            }            
        }
    }
}
