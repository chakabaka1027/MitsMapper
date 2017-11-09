using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using DISUnity.Resources;
using DISUnity.Editor.Attributes;

namespace DISUnity.Editor.DataType
{
    [CustomPropertyDrawer( typeof( WorldCoordinates ) )]
    public class WorldCoordinatesPropertyDrawer : LabelPropertyDrawer
    {
        #region Properties

        private SerializedProperty[] properties;
        private GUIContent[] labels;

        private const float InfoBoxHeight = 25;

        #endregion Properties

        /// <summary>
        /// Setup SerializedProperty's and any labels used, tools tips etc.
        /// </summary>
        /// <param name="property"></param>
        private void Init( SerializedProperty property )
        {
            properties = new SerializedProperty[3];
            labels = new GUIContent[3];

            properties[0] = property.FindPropertyRelative( "x" );
            labels[0] = new GUIContent( "X" );
            
            properties[1] = property.FindPropertyRelative( "y" );
            labels[1] = new GUIContent( "Y" );
            
            properties[2] = property.FindPropertyRelative( "z" );
            labels[2] = new GUIContent( "Z" );
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
                   ( property.isExpanded ? EditorGUIUtility.singleLineHeight * 3 : 0 ) + // Fields                   
                   ( property.isExpanded ? InfoBoxHeight : 0 ); // Help box                  
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

                // Help box - Inform user that floats are only supported
                Rect helpRect = new Rect( EditorGUIUtility.singleLineHeight * EditorGUI.indentLevel, position.y, 280, InfoBoxHeight );
                EditorGUI.HelpBox( helpRect, "Note: The editor only supports float values", MessageType.Info );
                position.y += InfoBoxHeight;

                float tmp;
                for( int i = 0; i < properties.Length; ++i )
                {
                    EditorGUI.showMixedValue = properties[i].hasMultipleDifferentValues;
                    EditorGUI.BeginChangeCheck();
                    tmp = EditorGUI.FloatField( position, labels[i], properties[i].floatValue );
                    if( EditorGUI.EndChangeCheck() )
                    {
                        properties[i].floatValue = tmp;
                    }
                    position.y += EditorGUIUtility.singleLineHeight;
                }
            }
            EditorGUI.EndProperty();
        }
    }
}