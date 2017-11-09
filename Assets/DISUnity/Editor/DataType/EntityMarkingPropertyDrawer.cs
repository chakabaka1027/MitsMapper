using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using DISUnity.Resources;
using DISUnity.Editor.Attributes;
using DISUnity.DataType.Enums.EntityMarking;

namespace DISUnity.Editor.DataType
{
    [CustomPropertyDrawer( typeof( EntityMarking ) )]
    public class EntityMarkingPropertyDrawer : LabelPropertyDrawer
    {
        #region Properties

        private SerializedProperty characters;

        private EntityMarking src;

        #endregion Properties

        /// <summary>
        /// Setup SerializedProperty's and any labels used, tools tips etc.
        /// </summary>
        /// <param name="property"></param>
        private void Init( SerializedProperty property )
        {
            characters = property.FindPropertyRelative( "characters" );
            // Get the array and store it in a temp version of EntityMarking, so it can do all the convertion between strings etc.
            byte[] charBytes = new byte[characters.arraySize];
            for( int i = 0; i < characters.arraySize; ++i )
            {
                charBytes[i] = ( byte )characters.GetArrayElementAtIndex( i ).intValue;
            }
            src = new EntityMarking( CharacterSet.ASCII, charBytes );            
        }

        /// <summary>
        /// Current height of this property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            return EditorGUIUtility.singleLineHeight;
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

            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            src.ASCII = EditorGUI.TextField( position, new GUIContent( "Entity Marking", Tooltips.EntityMarking ), src.ASCII );
            if( EditorGUI.EndChangeCheck() )
            {
                // Apply changes, copy over.
                byte[] srcBytes = src.Characters;
                for( int i = 0; i < srcBytes.Length; ++i )
                {
                    characters.GetArrayElementAtIndex( i ).intValue = srcBytes[i];
                }
            }

            EditorGUI.EndProperty();
        }
    }
}