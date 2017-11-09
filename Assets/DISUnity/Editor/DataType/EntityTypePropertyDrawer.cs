using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using Node = DISUnity.Simulation.EntityTypeMap.Node;
using DISUnity.Resources;
using DISUnity.Editor.Attributes;
using DISUnity.Simulation;

namespace DISUnity.Editor.DataType
{
    [CustomPropertyDrawer( typeof( EntityType ) )]
    public class EntityTypePropertyDrawer : LabelPropertyDrawer
    {
        #region Properties

        private EntityTypeMap map;
        private EntityTypeMap Map
        {
            get
            {
                if( map == null )
                {
                    // Find a mapper class
                    Object[] objs = UnityEngine.Resources.FindObjectsOfTypeAll( typeof( EntityTypeMap ) );
                    if( objs != null && objs.Length > 0 )
                    {
                        map = objs[0] as EntityTypeMap;
                    }
                }

                return map;
            }
            set
            {
                map = value;
            }
        }

        private SerializedProperty[] properties;
        private GUIContent[] labels;
        private int[][] ints;
        private GUIContent[][] descriptions;

        #endregion Properties

        /// <summary>
        /// Updates the popup choices for the 7 fields.
        /// </summary>
        private void UpdateChoices()
        {
            if( Map != null )
            {
                Node n = GeneratePopupOptions( Map.Root, out ints[0], out descriptions[0], byte.MinValue, byte.MaxValue, properties[0].intValue );
                for( int i = 1; i < properties.Length; ++i )
                {
                    if( properties[i].hasMultipleDifferentValues )
                    {
                        // Set to no data for remaining
                        for( int j = i; j < 7; ++j )
                        {
                            descriptions[j] = new GUIContent[0];
                            ints[j] = new int[0];                            
                        }
                        return;
                    }

                    // Country is a ushort
                    if( i == 2 )
                    {
                        n = GeneratePopupOptions( n, out ints[i], out descriptions[i], ushort.MinValue, ushort.MaxValue, properties[i].intValue );
                    }
                    else
                    {
                        n = GeneratePopupOptions( n, out ints[i], out descriptions[i], byte.MinValue, byte.MaxValue, properties[i].intValue );
                    }                
                }
            }
        }

        /// <summary>
        /// Generates int and GUIContent arrays for use in IntPopup. Returns Node with the same value as <paramref name="findVal"/> if one is found.
        /// </summary>
        /// <param name="n">Node whos children will be processed</param>
        /// <param name="intVals">Value array to populate</param>
        /// <param name="labelVals">Name/Description array to populate</param>
        /// <param name="min">Min value allowed. Allows us to ignore wildcards used in the map(-1)</param>
        /// <param name="max">Max value allowed.</param>
        /// <param name="findVal">Value to search for and return found node.</param>
        /// <returns>Found node with the value <paramref name="findVal"/></returns>
        private Node GeneratePopupOptions( Node n, out int[] intVals, out GUIContent[] labelVals, int min, int max, int findVal )
        {
            Node FoundNd = null;
            if( n != null )
            {
                intVals = new int[n.children.Count];
                labelVals = new GUIContent[n.children.Count];
                for( int i = 0; i < n.children.Count; ++i )
                {
                    Node currentNd = n.children[i];
                    if( currentNd.value < min || currentNd.value > max ) continue; // Ignore invalid values

                    intVals[i] = currentNd.value;
                    labelVals[i] = new GUIContent( currentNd.name );

                    // Is this value currently selected?
                    if( findVal == currentNd.value )
                    {
                        FoundNd = currentNd;
                    }
                }
            }
            else
            {
                intVals = new int[0];
                labelVals = new GUIContent[0];
            }
            return FoundNd;
        }

        /// <summary>
        /// Setup SerializedProperty's and any labels used, tools tips etc.
        /// </summary>
        /// <param name="property"></param>
        private void Init( SerializedProperty property )
        {
            properties = new SerializedProperty[7];
            labels = new GUIContent[7];
            ints = new int[7][];
            descriptions = new GUIContent[7][];

            properties[0] = property.FindPropertyRelative( "kind" );
            labels[0] = new GUIContent( "Kind", Tooltips.EntityKind );

            properties[1] = property.FindPropertyRelative( "domain" );
            labels[1] = new GUIContent( "Domain", Tooltips.EntityDomain );

            properties[2] = property.FindPropertyRelative( "country" );
            labels[2] = new GUIContent( "Country", Tooltips.EntityCountry );

            properties[3] = property.FindPropertyRelative( "category" );
            labels[3] = new GUIContent( "Category", Tooltips.EntityCategory );

            properties[4] = property.FindPropertyRelative( "subCategory" );
            labels[4] = new GUIContent( "Sub Category", Tooltips.EntitySubCategory );

            properties[5] = property.FindPropertyRelative( "specific" );
            labels[5] = new GUIContent( "Specific", Tooltips.EntitySpecific );

            properties[6] = property.FindPropertyRelative( "extra" );
            labels[6] = new GUIContent( "Extra", Tooltips.EntityExtra );

            if( Map != null )
            {
                UpdateChoices();
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
                   ( ( property.isExpanded && Map != null )? EditorGUIUtility.singleLineHeight * 7 : 0 ) + // Fields                   
                   ( ( property.isExpanded && ( Map == null || EditorSettings.AdvancedMode ) ) ? EditorGUIUtility.singleLineHeight + 2 : 0 ); // Entity Map
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

            if( property.isExpanded && ( Map == null || EditorSettings.AdvancedMode ) )
            {
                EditorGUIUtility.LookLikeControls();
                EditorGUI.indentLevel++;
                Rect objFieldRect = new Rect( position.x, position.y, 300, EditorGUIUtility.singleLineHeight );
                Rect btnRect = new Rect( position.x + 300, position.y, 200, EditorGUIUtility.singleLineHeight );
                position.y += EditorGUIUtility.singleLineHeight + 2; // 2 Button padding

                Map = EditorGUI.ObjectField( objFieldRect, "Entity Map", Map, typeof( EntityTypeMap ), true ) as EntityTypeMap;
                if( GUI.Button( btnRect, "Create New Entity Map" ) )
                {
                    GameObject mapGo = new GameObject( "Entity Map" );
                    Map = mapGo.AddComponent( typeof( EntityTypeMap ) ) as EntityTypeMap;
                    Map.Load();
                    UpdateChoices();
                }
                EditorGUI.indentLevel--;                
            }

            if( property.isExpanded && Map != null )
            {
                EditorGUI.indentLevel++;

                Rect popupField = new Rect( position.x, position.y, position.width - 100, EditorGUIUtility.singleLineHeight );
                Rect intField = new Rect( position.x + popupField.width, position.y, 100, EditorGUIUtility.singleLineHeight );
                int tmp;

                // Draw the 7 fields
                for( int i = 0; i < properties.Length; ++i )
                {
                    EditorGUI.showMixedValue = properties[i].hasMultipleDifferentValues;
                    EditorGUI.BeginChangeCheck();
                    tmp = EditorGUI.IntField( intField, properties[i].intValue );                    
                    tmp = EditorGUI.IntPopup( popupField, labels[i], tmp, descriptions[i], ints[i] );
                    if( EditorGUI.EndChangeCheck() )
                    {
                        properties[i].intValue = tmp;
                        UpdateChoices();
                    }
                    popupField.y = intField.y += EditorGUIUtility.singleLineHeight; 
                }               
            }
            EditorGUI.EndProperty();
        }
    }
}