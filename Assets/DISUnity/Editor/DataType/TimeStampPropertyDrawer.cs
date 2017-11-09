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
    [CustomPropertyDrawer( typeof( TimeStamp ) )]
    public class TimeStampPropertyDrawer : LabelPropertyDrawer
    {
        #region Properties

        private TimeStamp src;

        private GUIContent TypeEnumLabel;
        private GUIContent TimeLabel;

        private int[] typeInts;
        private GUIContent[] typeDescriptions;

        private SerializedProperty allFields;

        #endregion Properties

        /// <summary>
        /// Setup SerializedProperty's and any labels used, tools tips etc.
        /// </summary>
        /// <param name="property"></param>
        private void Init( SerializedProperty property )
        {
            // Get the private vars and create labels with tooltips
            TypeEnumLabel = new GUIContent( "Type", Tooltips.TimeStampType );

            TimeLabel = new GUIContent( "Time", Tooltips.TimeStampTime );

            allFields = property.FindPropertyRelative( "allFields" );

            // Create a temp version so we can use the get/set properties to do the bit operations.
            src = new TimeStamp( allFields.intValue );

            // Cleanup the type enum values
            Array n = Enum.GetNames( typeof( SignalEncodingType ) );
            typeDescriptions = new GUIContent[n.Length];
            for( int i = 0; i < n.Length; ++i )
            {
                // Remove underscore and nicify the name
                typeDescriptions[i] = new GUIContent( ObjectNames.NicifyVariableName( ( ( string )n.GetValue( i ) ).Replace( '_', ' ' ) ) );
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
                   ( property.isExpanded ? EditorGUIUtility.singleLineHeight * 2 : 0 ) + // Fields
                   ( EditorSettings.AdvancedMode && property.isExpanded ? EditorGUIUtility.singleLineHeight : 0 ); // Advanced Fields
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

                EditorGUI.showMixedValue = allFields.hasMultipleDifferentValues;

                EditorGUI.BeginChangeCheck();

                if( EditorSettings.AdvancedMode )
                {
                    src.AllFields = EditorGUI.IntField( position, "All Fields", src.AllFields );
                    position.y += EditorGUIUtility.singleLineHeight;
                }
                
                src.Type = ( TimeStampType )EditorGUI.EnumPopup( position, TypeEnumLabel, src.Type );
                position.y += EditorGUIUtility.singleLineHeight;

                src.Time = ( uint )EditorGUI.IntField( position, TimeLabel, ( int )src.Time );

                if( EditorGUI.EndChangeCheck() )
                {
                    // Update the property field. Swap the values so the new value goes into the SerializedProperty.
                    int tmp = allFields.intValue;
                    allFields.intValue = src.AllFields;
                    src.AllFields = tmp;
                }           
            }
            EditorGUI.EndProperty();
        }
    }
}