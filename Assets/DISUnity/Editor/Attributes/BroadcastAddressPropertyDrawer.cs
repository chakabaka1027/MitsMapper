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
    [CustomPropertyDrawer( typeof( BroadcastAddressAttribute ) )]
    public class BroadcastAddressPropertyDrawer : LabelPropertyDrawer
    {
        /// <summary>
        /// Current height of this property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            return EditorGUIUtility.singleLineHeight * ( Connection.IsValidBroadcastAddress( property.stringValue ) ? 1 : 2.5f );
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

            // Text field
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string sAddress = EditorGUI.TextField( position, label, property.stringValue );
            position.y += EditorGUIUtility.singleLineHeight;
            position.height = EditorGUIUtility.singleLineHeight * 1.5f; // Help box height
            
            if( !Connection.IsValidBroadcastAddress( sAddress ) )
            {
                EditorGUI.HelpBox( position, "Invalid Broadcast Address", MessageType.Warning );
            }                
 
            if( EditorGUI.EndChangeCheck() )
            {
                property.stringValue = sAddress;
            }
            
            EditorGUI.EndProperty();
        }
    }
}