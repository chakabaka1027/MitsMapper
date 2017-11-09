using UnityEngine;
using UnityEditor;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.Network;
using DISUnity.Network.Filters;

namespace DISUnity.Editor.Wizards
{
    /// <summary>
    /// Wizard to guide a user through adding a DIS connection to their scene/sim.
    /// </summary>
    public class ConnectionWizard : ScriptableWizard
    {
        #region Properties

        public string gameObjectName = "DISUnity - Connection";        

        public bool dontFilterExerciseID = false;
        public int exerciseID = 1;

        #endregion

        /// <summary>
        /// Menu command to show wizard
        /// </summary>
        [MenuItem( "DISUnity/Create DIS Connection" )]
        public static void ShowWizard()
        {
            ScriptableWizard.DisplayWizard<ConnectionWizard>( "Create Connection" );
        }

        /// <summary>
        /// Creates a new Connection GameObject and related components.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filterExerciseID"></param>
        /// <param name="exerciseID"></param>
        /// <returns></returns>
        public static GameObject CreateConnection( string name, bool filterExerciseID, int exerciseID )
        {
            GameObject connectionGO = new GameObject( name );
            Connection connection = connectionGO.AddComponent<Connection>();

            // PDU Factory
            GameObject factoryGO = new GameObject( "PDU Factory" );
            factoryGO.transform.parent = connectionGO.transform;

            DecodeFactory facory = factoryGO.AddComponent<DecodeFactory>();
            connection.Factory = facory;

            // Filters
            GameObject filtersGO = new GameObject( "Filters" );
            filtersGO.transform.parent = connectionGO.transform;

            // Exercise ID Filter
            if( filterExerciseID )
            {
                ExerciseIDFilter exid = filtersGO.AddComponent<ExerciseIDFilter>();
                exid.AllowedExerciseID = exerciseID;
                facory.Filters.Add( exid );
            }

            return connectionGO;
        }

        /// <summary>
        /// Draw GUI
        /// </summary>
        private void OnGUI()
        {
            minSize  = new Vector2( 700, 300 );

            GUILayout.Label( "This wizard will guide you through creating a DIS connection.\n" +
                             "Any values you set here can later be changed through the created Connection GameObject", EditorStyles.boldLabel );

            GUILayout.Space( 20 );

            EditorGUILayout.BeginVertical( GUI.skin.box );

            // GO Name
            GUILayout.Label( "What name would you like your Connection to have?\nThis is the name of the created GameObject.", EditorStyles.boldLabel );
            gameObjectName = EditorGUILayout.TextField( "Connection Name", gameObjectName );

            GUILayout.Space( 10 );

            // Filter exercise ID
            GUILayout.Label( "What Exercise ID would you like your Connection to be part of? In DIS many exercises can be run\n" +
                             "on the same network/port/address, in order to separate them we can specify an exercise ID, all\n" +
                             "DIS messages that do not have this exercise ID will be ignored.", EditorStyles.boldLabel );

            dontFilterExerciseID = EditorGUILayout.Toggle( "Allow All Exercise ID's", dontFilterExerciseID );
            GUI.enabled = !dontFilterExerciseID;
            exerciseID = EditorGUILayout.IntField( "Exercise ID", exerciseID );
            GUI.enabled = true;      

            GUILayout.Space( 10 );

            GUILayout.BeginHorizontal();
            GUIContent gc = new GUIContent( "Now create the GameObject:" );
            GUILayout.Label( gc, EditorStyles.boldLabel, GUILayout.Width( EditorStyles.boldLabel.CalcSize( gc ).x ) );

            // Create
            if( GUILayout.Button( "Create", GUILayout.Width( 100 ) ) )
            {
                GameObject connectionGO = CreateConnection( gameObjectName, !dontFilterExerciseID, exerciseID );
                Selection.activeObject = connectionGO;
                Close();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space( 10 );

            EditorGUILayout.EndVertical();
        }
    }
}