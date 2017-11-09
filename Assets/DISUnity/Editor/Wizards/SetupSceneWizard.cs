using UnityEngine;
using UnityEditor;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.Network;
using DISUnity.Network.Filters;
using DISUnity.DataType;
using DISUnity.Resources;
using System.Net;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System;
using DISUnity.Simulation.Managers;
using DISUnity.Simulation;

namespace DISUnity.Editor.Wizards
{
    /// <summary>
    /// Wizard to guide a user through adding DIS to a scene
    /// </summary>
    public class SetupSceneWizard : ScriptableWizard
    {
        #region Properties

        #region GUI

        private const string WizardTitle = "Add DISUnity To Scene";

        private GUIContent rootNameLabel;
        private GUIContent simAddLabel;
        private GUIContent appIdLabel;
        private GUIContent siteIdLabel;
        private GUIContent broadcastLabel;

        private List<string> mcAddresses;
        private int selectedMcAddIndex;

        #endregion

        // General
        private bool showGeneral = false;
        public string rootName = "DISUnity";
                
        // Exercise
        public bool showExercise = false;
        public byte exerciseID = 1;
        public SimulationAddress address = new SimulationAddress( 1, 1 );

        // Connection
        private bool showConnection = false;
        public bool useBroadcast = true;
        public int port = 3000;
        public float refreshFreq = 0.1f;
        public string broadcastAddress = IPAddress.Broadcast.ToString();
        private bool showMcAddresses = false;
        public List<string> multicastAddresses = new List<string>();

        #endregion

        /// <summary>
        /// Menu command to show wizard.
        /// </summary>
        [MenuItem( "DISUnity/Setup Scene For DIS" )]
        public static void ShowWizard()
        {
            ScriptableWizard.DisplayWizard<SetupSceneWizard>( WizardTitle );
        }

        /// <summary>
        /// Init.
        /// </summary>
        private void OnEnable()
        {
            minSize = new Vector2( 300, 400 );
            rootNameLabel = new GUIContent( "Name", "What name would you like the DISUnity GameObject to have?" );
            simAddLabel = new GUIContent( "Simulation Addresss", "This address represents our application, it will be used for all entities etc." );
            appIdLabel = new GUIContent( "Application", Tooltips.ApplicationID );
            siteIdLabel = new GUIContent( "Site", Tooltips.SiteID );
            broadcastLabel = new GUIContent( "Broadcast", "Use a broadcast connection, the most common type of DIS connection." );
            mcAddresses = Connection.GetAvailableMulticastAddresses();
        }

        /// <summary>
        /// Draw GUI
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Label( "Setup a scene to use DISUnity.", EditorStyles.boldLabel );

            GUILayout.Space( 20 );

            EditorGUILayout.BeginVertical( GUI.skin.box );

            // General
            showGeneral = EditorGUILayout.Foldout( showGeneral, "General" );
            if( showGeneral )
            {
                EditorGUI.indentLevel += 2;                
                OnGUIGeneral();
                EditorGUI.indentLevel -= 2;
            }

            // Exercise
            showExercise = EditorGUILayout.Foldout( showExercise, "Exercise" );
            if( showExercise )
            {
                EditorGUI.indentLevel += 2;
                OnGUIExercise();
                EditorGUI.indentLevel -= 2;
            }

            // Connection
            showConnection = EditorGUILayout.Foldout( showConnection, "Connection" );
            if( showConnection )
            {
                EditorGUI.indentLevel += 2;
                OnGUIConnection();
                EditorGUI.indentLevel -= 2;
            }

            GUILayout.Space( 10 );
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space( 10 );
            if( GUILayout.Button( "Create", EditorStyles.toolbarButton, GUILayout.Width( 100 ) ) )
            {                
                Create();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space( 10 );

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draw general options.
        /// </summary>
        private void OnGUIGeneral()
        {               
            EditorGUI.BeginChangeCheck();
            
            // Name 
            string rn = EditorGUILayout.TextField( rootNameLabel, rootName );

            if( EditorGUI.EndChangeCheck() )
            {
                Undo.RecordObject( this, WizardTitle );
                rootName = rn;
            }
        }

        /// <summary>
        /// Draw exercise options
        /// </summary>
        private void OnGUIExercise()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField( simAddLabel );
            EditorGUI.indentLevel++;
            ushort site = ( ushort )Mathf.Clamp( EditorGUILayout.IntField( siteIdLabel, ( int )address.Site, GUILayout.Width( 210 ) ), ushort.MinValue, ushort.MaxValue );
            ushort app = ( ushort )Mathf.Clamp( EditorGUILayout.IntField( appIdLabel, ( int )address.Application, GUILayout.Width( 210 ) ), ushort.MinValue, ushort.MaxValue );
            EditorGUI.indentLevel--;

            if( EditorGUI.EndChangeCheck() )
            {
                Undo.RecordObject( this, WizardTitle );
                address.Site = site;
                address.Application = app;
            }
        }

        /// <summary>
        /// Draws connection options
        /// </summary>
        private void OnGUIConnection()
        {
            port = EditorGUILayout.IntField( new GUIContent( "Port", "DIS uses 3000 by default." ), port );
            useBroadcast = EditorGUILayout.Toggle( broadcastLabel, useBroadcast );
            GUI.enabled = useBroadcast;            
                       
            // Text field
            broadcastAddress = EditorGUILayout.TextField( "Broadcast Address", broadcastAddress );            
            if( !Connection.IsValidBroadcastAddress( broadcastAddress ) )
            {
                EditorGUILayout.HelpBox( "Invalid Broadcast Address", MessageType.Warning );
            }
                         
            GUI.enabled = true;

            // Multicast
            EditorGUI.indentLevel--;
            showMcAddresses = EditorGUILayout.Foldout( showMcAddresses, string.Format( "Multicast Addresses({0})", multicastAddresses.Count ) );
            EditorGUI.indentLevel++;

            if( showMcAddresses )
            {
                EditorGUI.indentLevel += 2;

                GUILayout.Space( 5 );
                EditorGUILayout.BeginHorizontal();                
                if( mcAddresses.Count > 0 )
                {
                    selectedMcAddIndex = EditorGUILayout.Popup( selectedMcAddIndex, mcAddresses.ToArray(), GUILayout.Width( 175 ) );
                    if( GUILayout.Button( "Add", EditorStyles.toolbarButton, GUILayout.Width( 50 ) ) )
                    {
                        multicastAddresses.Add( mcAddresses[selectedMcAddIndex] );
                        mcAddresses.RemoveAt( selectedMcAddIndex );
                        selectedMcAddIndex = 0;
                    }
                }
                else
                {
                    EditorGUILayout.LabelField( "No Multicast Addresses Remain" );
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space( 5 );

                for( int i = 0; i < multicastAddresses.Count; ++i )
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField( multicastAddresses[i], GUILayout.Width( 175 ) );
                    if( GUILayout.Button( "Del", EditorStyles.toolbarButton, GUILayout.Width( 50 ) ) )
                    {
                        mcAddresses.Add( multicastAddresses[i] );
                        multicastAddresses.RemoveAt( i );
                        break;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel -= 2;
            }
        }

        /// <summary>
        /// Creates the required objects for a DIS exercise using the provided params.
        /// </summary>
        public void Create()
        {
            // Create the root GameObject
            GameObject rootGO = new GameObject( rootName );            

            // Exercise info
            GameObject exerciseGO = new GameObject( "Exercise Manager" );
            exerciseGO.transform.parent = rootGO.transform;
            ExerciseManager exMan = exerciseGO.AddComponent<ExerciseManager>();
            exMan.Address = address;
            
            // Entity Manager
            GameObject entGO = new GameObject( "Entity Manager" );
            entGO.transform.parent = rootGO.transform;
            EntityManager entMan = entGO.AddComponent<EntityManager>();    
       
            // Entity type
            GameObject mapGo = new GameObject( "Entity Map" );
            mapGo.transform.parent = entGO.transform;
            entMan.EntityMap = mapGo.AddComponent( typeof( EntityTypeMap ) ) as EntityTypeMap;
            entMan.EntityMap.Load();

            // Create a gameobject to be parent of all remote entities.
            GameObject remoteEntsParent = new GameObject( "Remote Entities" );
            remoteEntsParent.transform.parent = entGO.transform;
            entMan.RemoteEntityParent = remoteEntsParent;

            // Connection
            GameObject conGO = new GameObject( "Connection" );
            conGO.transform.parent = rootGO.transform;
            Connection connection = conGO.AddComponent<Connection>();
            connection.Port = port;
            connection.ReceivePollFrequency = refreshFreq;
            connection.UseBroadcast = useBroadcast;
            connection.BroadcastAddress = broadcastAddress;
            connection.MulticastGroups = multicastAddresses;
            DecodeFactory facory = conGO.AddComponent<DecodeFactory>();
            connection.Factory = facory;                       

            // Filters
            GameObject filtersGO = new GameObject( "Filters" );
            filtersGO.transform.parent = conGO.transform;

            // Exercise ID Filter
            ExerciseIDFilter exid = filtersGO.AddComponent<ExerciseIDFilter>();
            exid.AllowedExerciseID = exerciseID;
            facory.Filters.Add( exid );

            Undo.RegisterCreatedObjectUndo( rootGO, "Setup Scene For DIS" );
            Close();
        }
    }
}