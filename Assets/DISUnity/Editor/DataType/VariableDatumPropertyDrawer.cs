using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using DISUnity.Resources;
using DISUnity.Editor.Attributes;
using System;
using System.Linq;

namespace DISUnity.Editor.DataType
{
    [CustomPropertyDrawer( typeof( VariableDatum ) )]
    public class VariableDatumPropertyDrawer : PropertyDrawer
    {
        #region Properties

        private SerializedProperty datumID;

        private SerializedProperty internalDataType;
        private GUIContent internalDataTypeLabel;

        private SerializedProperty datumLengthInBits;

        private SerializedProperty data;
        
        private VariableDatum tempVariableDatum = new VariableDatum(); // Using for internal data conversions.

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
            datumLengthInBits = property.FindPropertyRelative( "datumLengthInBits" );
            data = property.FindPropertyRelative( "data" );  
            
            // Copy data over to temp            
            if( data.arraySize > 0 )
            {
                byte[] buffer = new byte[data.arraySize];
                for( int i = 0; i < data.arraySize; ++i )
                {
                    buffer[i] = ( byte )data.GetArrayElementAtIndex( i ).intValue;
                }
                tempVariableDatum.SetData( buffer, ( uint )datumLengthInBits.intValue );
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
            float h = EditorGUIUtility.singleLineHeight; // Label
            if( property.isExpanded )
            {
                h += 2 * EditorGUIUtility.singleLineHeight; // Datum id & Internal data type
                SerializedProperty internalType = property.FindPropertyRelative( "internalDataType" );
                if( internalDataType != null )
                {
                    SerializedProperty internalData = property.FindPropertyRelative( "data" );
                    if( internalData != null )
                    {                       
                        VariableDatum.DatumDataType dataType = ( VariableDatum.DatumDataType )internalType.intValue;
                        switch( dataType )
                        {
                            case VariableDatum.DatumDataType.Doubles:
                            case VariableDatum.DatumDataType.Longs:
                            case VariableDatum.DatumDataType.ULongs:                            
                            h += ( ( internalData.arraySize / 8 ) + 1 ) * EditorGUIUtility.singleLineHeight;
                            break;

                            case VariableDatum.DatumDataType.String:
                            h += EditorGUIUtility.singleLineHeight;                    
                            for( int i = 0; i < internalData.arraySize; ++i )
                                if( internalData.GetArrayElementAtIndex( i ).intValue == '\n' )
                                    h += EditorGUIUtility.singleLineHeight;
                            break;
                        }
                    }
                }
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

                VariableDatum.DatumDataType dataType = ( VariableDatum.DatumDataType )EditorGUI.EnumPopup( position, internalDataTypeLabel, ( VariableDatum.DatumDataType )internalDataType.intValue );
                internalDataType.intValue = ( int )dataType;
                position.y += EditorGUIUtility.singleLineHeight;
                
                switch( dataType )
                {
                    case VariableDatum.DatumDataType.Doubles: DrawDoublesField( position ); break;
                    case VariableDatum.DatumDataType.Longs: DrawLongsField( position ); break;
                    case VariableDatum.DatumDataType.ULongs: DrawULongsField( position ); break;
                    case VariableDatum.DatumDataType.String: DrawStringField( position ); break;
                }                
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Draw for editing a doubles array.
        /// </summary>
        /// <param name="position"></param>
        void DrawDoublesField( Rect position )
        {            
            Rect itemPos = new Rect( position.x, position.y, position.width - 30, position.height );
            Rect buttonPos = new Rect( itemPos.x + itemPos.width, itemPos.y, 30, itemPos.height );

            EditorGUI.BeginChangeCheck();
                               
            var list = tempVariableDatum.GetAsDoubleList();

            if( list != null )
            {
                for( int i = 0; i < list.Length; ++i )
                {
                    list[i] = EditorGUI.FloatField( itemPos, "Item " + i, ( float )list[i] );

                    if( GUI.Button( buttonPos, "-" ) )
                    {
                        Array.Copy( list, i + 1, list, i, list.Length - i - 1 );
                        Array.Resize( ref list, list.Length - 1 );
                    }
      
                    buttonPos.y = itemPos.y += EditorGUIUtility.singleLineHeight;
                }                
            }

            if( GUI.Button( buttonPos, "+" ) )
            {
                if( list != null )
                    Array.Resize( ref list, list.Length + 1 );
                else 
                    list = new double[1];
            }

            if( EditorGUI.EndChangeCheck() )
            {
                tempVariableDatum.SetData( list );
                if( data.arraySize != tempVariableDatum.Data.Length )
                    data.arraySize = tempVariableDatum.Data.Length;

                // Copy back to property
                for( int i = 0; i < tempVariableDatum.Data.Length; ++i )
                {
                    data.GetArrayElementAtIndex( i ).intValue = tempVariableDatum.Data[i];
                }
                datumLengthInBits.intValue = ( int )tempVariableDatum.DatumLength;
            }
        }

        /// <summary>
        /// Draw for editing a longs array.
        /// </summary>
        /// <param name="position"></param>
        void DrawLongsField( Rect position )
        {
            Rect itemPos = new Rect( position.x, position.y, position.width - 30, position.height );
            Rect buttonPos = new Rect( itemPos.x + itemPos.width, itemPos.y, 30, itemPos.height );

            EditorGUI.BeginChangeCheck();

            var list = tempVariableDatum.GetAsLongList();

            if( list != null )
            {
                for( int i = 0; i < list.Length; ++i )
                {
                    list[i] = EditorGUI.IntField( itemPos, "Item " + i, ( int )list[i] );

                    if( GUI.Button( buttonPos, "-" ) )
                    {
                        Array.Copy( list, i + 1, list, i, list.Length - i - 1 );
                        Array.Resize( ref list, list.Length - 1 );
                    }

                    buttonPos.y = itemPos.y += EditorGUIUtility.singleLineHeight;
                }
            }

            if( GUI.Button( buttonPos, "+" ) )
            {
                if( list != null )
                    Array.Resize( ref list, list.Length + 1 );
                else
                    list = new long[1];
            }

            if( EditorGUI.EndChangeCheck() )
            {
                tempVariableDatum.SetData( list );
                if( data.arraySize != tempVariableDatum.Data.Length )
                    data.arraySize = tempVariableDatum.Data.Length;

                // Copy back to property
                for( int i = 0; i < tempVariableDatum.Data.Length; ++i )
                {
                    data.GetArrayElementAtIndex( i ).intValue = tempVariableDatum.Data[i];
                }
                datumLengthInBits.intValue = ( int )tempVariableDatum.DatumLength;
            }
        }


        /// <summary>
        /// Draw for editing a ulongs array.
        /// </summary>
        /// <param name="position"></param>
        void DrawULongsField( Rect position )
        {
            Rect itemPos = new Rect( position.x, position.y, position.width - 30, position.height );
            Rect buttonPos = new Rect( itemPos.x + itemPos.width, itemPos.y, 30, itemPos.height );

            EditorGUI.BeginChangeCheck();

            var list = tempVariableDatum.GetAsULongList();

            if( list != null )
            {
                for( int i = 0; i < list.Length; ++i )
                {
                    list[i] = ( uint ) Mathf.Clamp( EditorGUI.IntField( itemPos, "Item " + i, ( int )list[i] ), 0, ulong.MaxValue );

                    if( GUI.Button( buttonPos, "-" ) )
                    {
                        Array.Copy( list, i + 1, list, i, list.Length - i - 1 );
                        Array.Resize( ref list, list.Length - 1 );
                    }

                    buttonPos.y = itemPos.y += EditorGUIUtility.singleLineHeight;
                }
            }

            if( GUI.Button( buttonPos, "+" ) )
            {
                if( list != null )
                    Array.Resize( ref list, list.Length + 1 );
                else
                    list = new ulong[1];
            }

            if( EditorGUI.EndChangeCheck() )
            {
                tempVariableDatum.SetData( list );
                if( data.arraySize != tempVariableDatum.Data.Length )
                    data.arraySize = tempVariableDatum.Data.Length;

                // Copy back to property
                for( int i = 0; i < tempVariableDatum.Data.Length; ++i )
                {
                    data.GetArrayElementAtIndex( i ).intValue = tempVariableDatum.Data[i];
                }
                datumLengthInBits.intValue = ( int )tempVariableDatum.DatumLength;
            }
        }

        /// <summary>
        /// Draw for editing a string.
        /// </summary>
        /// <param name="position"></param>
        void DrawStringField( Rect position )
        {           
            EditorGUI.BeginChangeCheck();
            
            var text = tempVariableDatum.GetAsString();

            // Calculate height
            position.height = EditorGUIUtility.singleLineHeight * ( 1 + text.Count( o => o == '\n' ) );

            text = EditorGUI.TextArea( position, text );
            
            if( EditorGUI.EndChangeCheck() )
            {
                tempVariableDatum.SetData( text );
                if( tempVariableDatum.Data == null )
                {
                    data.arraySize = 0;
                }
                else if( tempVariableDatum.Data.Length != data.arraySize )
                {
                    data.arraySize = tempVariableDatum.Data.Length;

                    // Copy back to property
                    for( int i = 0; i < tempVariableDatum.Data.Length; ++i )
                    {
                        data.GetArrayElementAtIndex( i ).intValue = tempVariableDatum.Data[i];
                    }
                    datumLengthInBits.intValue = ( int )tempVariableDatum.DatumLength;
                }
            }
        }
    }
}