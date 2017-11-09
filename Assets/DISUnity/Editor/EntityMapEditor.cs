using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using DISUnity.Simulation;
using System.IO;
using UnityEngine;
using Node = DISUnity.Simulation.EntityTypeMap.Node;

namespace DISUnity.Editor
{
    /// <summary>
    /// Custom editor view 
    /// </summary>
    [CustomEditor( typeof( EntityTypeMap ) )]
    public class EntityMapEditor : UnityEditor.Editor
    {
        #region Properties

        private EntityTypeMap monoMap;

        #endregion Properties

        /// <summary>
        /// Init
        /// </summary>
        private void OnEnable()
        {
            monoMap = target as EntityTypeMap;
        }

        /// <summary>
        /// Draw inspector view
        /// </summary>
        public override void OnInspectorGUI()
        {
            if( monoMap.Root == null )
            {
                monoMap.Load();                
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                if( GUILayout.Button( "Reset", GUILayout.Width( 300 ) ) )
                {
                    monoMap.Load();
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
				
				if( GUILayout.Button( "+", EditorStyles.toolbarButton ) )
				{
					monoMap.Root.children.Insert( 0, new Node( -1, "Any" ) );
					monoMap.Root.isExpanded = true;                            
				}
				
                ShowNode( monoMap.Root );

                EditorGUILayout.Space();

                // Save undo.
                if( GUI.changed )
                {                    
                    Undo.RecordObject( monoMap, "Entity Map" );
                }
            }
        }

        /// <summary>
        /// Show a nodes contents and its child nodes if expanded.
        /// </summary>
        /// <param name="n"></param>
        private void ShowNode( Node n )
        {
            if( n.children != null )
            {
                for( int i = 0; i < n.children.Count; ++i )
                {
                    Node ch = n.children[i];
                    float indent = EditorGUI.indentLevel * 15;

                    EditorGUILayout.BeginHorizontal();    
                    
                    EditorGUILayout.BeginHorizontal( GUILayout.Width( 30 + indent ) );                    
                    Color defaultBgCol = GUI.backgroundColor;
                    GUI.backgroundColor = ch.children.Count > 0 ? defaultBgCol : new Color( defaultBgCol.r, defaultBgCol.g, defaultBgCol.b, 0.1f ); // Use colour to indicate if a node has child nodes. 
                    ch.isExpanded = EditorGUILayout.Foldout( ch.isExpanded, string.Empty );
                    GUI.backgroundColor = defaultBgCol;
                    EditorGUILayout.EndHorizontal();

                    //GUI.contentColor = colors[EditorGUI.indentLevel % colors.Length];
                    
                    ch.value = EditorGUILayout.IntField( ch.value, GUI.skin.label, GUILayout.Width( 30 + indent ) );
                    ch.name = EditorGUILayout.TextField( ch.name, EditorStyles.label );

                    // Add/Remove node?         
                    if( EditorGUI.indentLevel < 7 )
                    {
                        if( GUILayout.Button( "+", EditorStyles.toolbarButton ) )
                        {
                            ch.children.Insert( 0, new Node( -1, "Any" ) );
                            ch.isExpanded = true;                            
                        }
                    }

                    if( GUILayout.Button( "-", EditorStyles.toolbarButton ) )
                    {
                        n.children.Remove( ch );
                        break;
                    }
                    
                    // Object field
                    ch.obj = EditorGUILayout.ObjectField( ch.obj, typeof( GameObject ), false ) as GameObject;

                    EditorGUILayout.EndHorizontal();                    

                    // Draw children?
                    if( ch.isExpanded )
                    {
                        EditorGUI.indentLevel++;                                                                                           
                        ShowNode( ch );
                        EditorGUI.indentLevel--;                        
                    }                    
                }
            }
        }
    }
}
