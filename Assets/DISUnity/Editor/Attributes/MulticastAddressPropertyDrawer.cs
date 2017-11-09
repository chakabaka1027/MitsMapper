using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using System.Linq;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using System;
using DISUnity.Attributes;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using DISUnity.Network;

namespace DISUnity.Editor.Attributes
{
    [CustomPropertyDrawer( typeof( MulticastAddressListAttribute ) )]
    public class MulticastAddressPropertyDrawer : LabelPropertyDrawer
    {
        #region Properties

        private List<string> availableMcAddresses = new List<string>();
        private int selectedMcAddIndex;

        #endregion Properties

        /// <summary>
        /// Current height of this property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            float h = EditorGUIUtility.singleLineHeight;
            if( property.isExpanded )
            {
                h += EditorGUIUtility.singleLineHeight * 2;
                h += EditorGUIUtility.singleLineHeight * property.arraySize;
            }
            return h;
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

            EditorGUI.BeginProperty( position, label, property );

            position.height = EditorGUIUtility.singleLineHeight;

            // Collect info on  valid multicast addresses          
            availableMcAddresses = Connection.GetAvailableMulticastAddresses();

            // Remove used address
            for( int i = 0; i < property.arraySize; ++i )
            {
                availableMcAddresses.Remove( property.GetArrayElementAtIndex( i ).stringValue );                
            }

            EditorGUI.indentLevel--;
            property.isExpanded = EditorGUI.Foldout( position, property.isExpanded, label );
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.indentLevel++;

            if( property.isExpanded )
            {
                EditorGUI.indentLevel += 2;
                // Text field
                EditorGUI.BeginChangeCheck();

                EditorGUIUtility.LookLikeControls();

                if( availableMcAddresses.Count > 0 )
                {
                    Rect listRect = new Rect( position.x, position.y, 275, position.height );
                    Rect addRect = new Rect( position.x + listRect.width + 5, position.y, 50, position.height );
                    selectedMcAddIndex = EditorGUI.Popup( listRect, "Available", selectedMcAddIndex, availableMcAddresses.ToArray() );
                    if( GUI.Button( addRect, "Add", EditorStyles.miniButton ) )
                    {
                        property.InsertArrayElementAtIndex( property.arraySize );
                        property.GetArrayElementAtIndex( property.arraySize - 1 ).stringValue = availableMcAddresses[selectedMcAddIndex];
                        selectedMcAddIndex = 0;
                    }
                }
                else
                {
                    EditorGUI.LabelField( position, "No More Available Multicast Addresses." );
                }
                position.y += EditorGUIUtility.singleLineHeight;

                EditorGUI.LabelField( position, "Items" );
                position.y += EditorGUIUtility.singleLineHeight;

                for( int i = 0; i < property.arraySize; ++i )
                {
                    Rect itemRect = new Rect( position.x, position.y, 160, position.height );
                    Rect delRect = new Rect( position.x + itemRect.width, position.y, 50, position.height );
                    EditorGUI.LabelField( itemRect, property.GetArrayElementAtIndex( i ).stringValue );
                    if( GUI.Button( delRect, "Del", EditorStyles.miniButton ) )
                    {
                        property.DeleteArrayElementAtIndex( i );
                    }

                    position.y += EditorGUIUtility.singleLineHeight;
                }
            }

            EditorGUI.EndProperty();
        }
    }
}