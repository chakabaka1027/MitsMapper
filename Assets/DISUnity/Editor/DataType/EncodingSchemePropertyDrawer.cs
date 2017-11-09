using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using DISUnity.Resources;
using DISUnity.Editor.Attributes;
using System;

namespace DISUnity.Editor.DataType
{
    [CustomPropertyDrawer( typeof( EncodingScheme ) )]
    public class EncodingSchemeDrawer : LabelPropertyDrawer
    {
        #region Properties

        private SerializedProperty typeAndClass;
        private SerializedProperty tdlType;
       
        private GUIContent[] names;

        private EncodingScheme src;

        #endregion Properties

        /// <summary>
        /// Setup SerializedProperty's and any labels used, tools tips etc.
        /// </summary>
        /// <param name="property"></param>
        private void Init( SerializedProperty property )
        {
            typeAndClass = property.FindPropertyRelative( "typeAndClass" );
            tdlType = property.FindPropertyRelative( "tdlType" );
            src = new EncodingScheme( typeAndClass.intValue, ( TacticalDataLinkType )tdlType.intValue );

            // Generate enum arrays
            Array n = Enum.GetNames( typeof( SignalEncodingType ) );
            names = new GUIContent[n.Length];
            for( int i = 0; i < n.Length; ++i )
            {
                // Remove underscore and nicify the name
                names[i] = new GUIContent( ObjectNames.NicifyVariableName( ( ( string )n.GetValue( i ) ).Replace( '_', ' ' ) ) );
            }
        }

        /// <summary>
        /// Current height of this property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            return EditorGUIUtility.singleLineHeight + // Label
                   ( property.isExpanded ? EditorGUIUtility.singleLineHeight * 3 : 0 ) +
                   ( EditorSettings.AdvancedMode ? EditorGUIUtility.singleLineHeight : 0 ); // Bit field        
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

            Init( property );

            EditorGUI.BeginProperty( position, label, property );
            position.height = EditorGUIUtility.singleLineHeight;

            // Label        
            property.isExpanded = EditorGUI.Foldout( position, property.isExpanded, label );
            position.y += EditorGUIUtility.singleLineHeight;

            if( property.isExpanded )
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
                
                if( EditorSettings.AdvancedMode )
                {
                    src.TypeAndClassBitField = EditorGUI.IntField( position, "Type & Class Bit Field", src.TypeAndClassBitField );
                    position.y += EditorGUIUtility.singleLineHeight;
                }

                src.EncodingClass = ( SignalEncodingClass )EditorGUI.EnumPopup( position, "Encoding Class", src.EncodingClass );
                position.y += EditorGUIUtility.singleLineHeight;

                if( src.EncodingClass == SignalEncodingClass.EncodedAudio )
                {
                    src.EncodingType = ( SignalEncodingType )EditorGUI.EnumPopup( position, "Audio Type", src.EncodingType );                    
                }
                else
                {
                    src.NumberTDLMessages = ( ushort )EditorGUI.IntField( position, "Number of TDL Messages", src.NumberTDLMessages );
                }

                position.y += EditorGUIUtility.singleLineHeight;
                
                if( EditorGUI.EndChangeCheck() )
                {
                    typeAndClass.intValue = src.TypeAndClassBitField;
                }

                tdlType.intValue = ( int )( TacticalDataLinkType )EditorGUI.EnumPopup( position, "Tactical Data Link Type:", ( TacticalDataLinkType )tdlType.intValue );
            }
            EditorGUI.EndProperty();
        }
    }
}