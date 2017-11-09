using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using System.Linq;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using System;
using DISUnity.Attributes;

namespace DISUnity.Editor.Attributes
{
    [CustomPropertyDrawer( typeof( NicifyEnumAttribute ) )]
    public class NicifyEnumPropertyDrawer : LabelPropertyDrawer
    {
        /// <summary>
        /// Draw property
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            label = GetLabel( label );

            EditorGUI.BeginProperty( position, label, property );

            // Generate enum arrays
            NicifyEnumAttribute att = attribute as NicifyEnumAttribute;                        
            Array n = Enum.GetNames( att.type );
            GUIContent[] names = new GUIContent[n.Length];            
            for( int i = 0; i < n.Length; ++i )
            {
                // Remove underscore and nicify the name
                names[i] = new GUIContent( ObjectNames.NicifyVariableName( ( ( string )n.GetValue( i ) ).Replace( '_', ' ' ) ) );
            }

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

            Rect enumRect = new Rect( position.x, position.y, position.width - 100, position.height );
            Rect numRect = new Rect( position.x + enumRect.width, position.y, 100, position.height );
                        
            EditorGUI.BeginChangeCheck();            
            int tmpVal = EditorGUI.IntField( numRect, property.intValue );
            if( EditorGUI.EndChangeCheck() )
            {
                property.intValue = tmpVal;
            }

            EditorGUI.BeginChangeCheck();                        
            int tmpI = EditorGUI.Popup( enumRect, label, property.enumValueIndex, names );
            if( EditorGUI.EndChangeCheck() )
            {            
                property.enumValueIndex = tmpI;
            }
            
            EditorGUI.EndProperty();
        }
    }
}