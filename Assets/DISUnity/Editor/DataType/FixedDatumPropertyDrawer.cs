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
    [CustomPropertyDrawer( typeof( FixedDatum ) )]
    public class FixedDatumPropertyDrawer : PropertyDrawer
    {
        #region Properties

        private SerializedProperty datumID;

        private SerializedProperty internalDataType;
        private GUIContent internalDataTypeLabel;

        private SerializedProperty data;

        private FixedDatum tempFixedDatum = new FixedDatum(); // Using for internal data conversions.

        #endregion Properties

        /// <summary>
        /// Setup SerializedProperty's and any labels used, tools tips etc.
        /// </summary>
        /// <param name="property"></param>
        private void Init( SerializedProperty property )
        {
            // Get the private vars and create labels with tooltips

            datumID = property.FindPropertyRelative( "datumID" );
            internalDataType = property.FindPropertyRelative( "internalDataType" );
            internalDataTypeLabel = new GUIContent( "Internal Data Type", Tooltips.InternalDataType );
            data = property.FindPropertyRelative( "data" );  
            
            // Copy data over to temp            
            tempFixedDatum.Data = new byte[4];
            
            if( data.arraySize != 4 )
                data.arraySize = 4;

            if( data.arraySize == 4 )
                for( int i = 0; i < 4; ++i )
                    tempFixedDatum.Data[i] = ( byte )data.GetArrayElementAtIndex( i ).intValue;             
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
                   ( property.isExpanded ? EditorGUIUtility.singleLineHeight * 3 : 0 );                   
        }

        /// <summary>
        /// Draw property
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {            
            Init( property );

            EditorGUI.BeginProperty( position, label, property );
            position.height = EditorGUIUtility.singleLineHeight;

            // Label        
            property.isExpanded = EditorGUI.Foldout( position, property.isExpanded, ObjectNames.NicifyVariableName( ( ( DatumID )datumID.intValue ).ToString() ) );
            position.y += EditorGUIUtility.singleLineHeight;

            if( property.isExpanded )
            {
                EditorGUI.indentLevel++;                

                datumID.intValue = ( int )( DatumID )EditorGUI.EnumPopup( position, "Datum ID", ( DatumID )datumID.intValue );
                position.y += EditorGUIUtility.singleLineHeight;

                FixedDatum.DatumDataType dataType = ( FixedDatum.DatumDataType )EditorGUI.EnumPopup( position, internalDataTypeLabel, ( FixedDatum.DatumDataType )internalDataType.intValue );
                internalDataType.intValue = ( int )dataType;
                position.y += EditorGUIUtility.singleLineHeight;

                EditorGUI.BeginChangeCheck();
                switch( dataType )
                {
                    case FixedDatum.DatumDataType.Int:
                        tempFixedDatum.SetData( EditorGUI.IntField( position, "Value", tempFixedDatum.GetAsInt() ) );
                        break;

                    case FixedDatum.DatumDataType.UInt:
                        tempFixedDatum.SetData( ( uint )EditorGUI.IntField( position, "Value", ( int )tempFixedDatum.GetAsUint() ) );
                        break;

                    case FixedDatum.DatumDataType.Float:
                        tempFixedDatum.SetData( EditorGUI.FloatField( position, "Value", tempFixedDatum.GetAsFloat() ) );
                        break;               
                        
                    default:
                        string dataS = "Data: ";
                        for( int i = 0; i < data.arraySize; i++ )                        
                            dataS += data.GetArrayElementAtIndex( i ).intValue.ToString( "X" ) + " ";                            
                        
                        EditorGUI.LabelField( position, dataS );                        
                        break;
                }                              

                if( EditorGUI.EndChangeCheck() )
                {
                    if( data.arraySize != 4 )
                        data.arraySize = 4;

                    // Copy back to property
                    for( int i = 0; i < 4; ++i )
                    {
                        data.GetArrayElementAtIndex( i ).intValue = tempFixedDatum.Data[i];
                    }                                            
                }
                
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }
    }
}