using UnityEngine;
using System.Collections;
using DISUnity.Network;
using System.Net;
using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Net.Sockets;

namespace DISUnity.UI
{
    /// <summary>
    /// GUI for editing the DIS connection during runtime.
    /// </summary>
    /// 
    [AddComponentMenu( "DISUnity/UI/Connection Editor" )]
    public class ConnectionEditor : UIComponent
    {
        #region Properties

        #region Private

        [SerializeField]
        private string windowLabel = "DIS Connection Settings";

        [SerializeField]
        [HideInInspector]
        private int windowID = "ConnectionSettings".GetHashCode();

        [SerializeField]
        private Rect windowRect = new Rect( 0, 0, 350, 400 );

        [SerializeField]
        private Connection connection;

        // Used at runtime        
        private string broadcastAddress;
        private int selectedUnusedMultiCastAddress = 0;
        private int selectedUsedMultiCastAddress = 0;

        #endregion

        /// <summary>
        /// The label/header of the window.
        /// </summary>
        public string WindowLabel
        {
            get
            {
                return windowLabel;
            }
            set
            {
                windowLabel = value;
            }
        }

        /// <summary>
        /// Unique ID of window
        /// </summary>
        public int WindowID
        {
            get
            {
                return windowID;
            }
            set
            {
                windowID = value;
            }
        }

        /// <summary>
        /// Position and size of window
        /// </summary>
        public Rect WindowRect
        {
            get
            {
                return windowRect;
            }
            set
            {
                windowRect = value;
            }
        }

        /// <summary>
        /// The connection being edited.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value;
            }
        }

        #endregion

        /// <summary>
        /// Init.
        /// </summary>
        private void OnEnable()
        {
            if( connection != null )
            {
                broadcastAddress = connection.BroadcastAddress;                
            }
        }

        /// <summary>
        /// Draw GUI window
        /// </summary>
        protected override void DrawGUI()
        {            
            windowRect = GUI.Window( windowID, windowRect, DrawWindowContents, windowLabel );
        }         
                
        /// <summary>
        /// Draws the contents of the dragable window.
        /// </summary>
        /// <param name="id"></param>
        private void DrawWindowContents( int id )
        {            
            if( connection == null )
            {
                GUILayout.Label( "No connection to edit, it is null." );                
            }
            else
            {                                
                // Port
                GUILayout.BeginHorizontal();
                GUILayout.Label( "Port" );
                int port;
                if( int.TryParse( GUILayout.TextField( connection.Port.ToString() ), out port ) )
                {
                    Connection.Port = port;
                }
                GUILayout.EndHorizontal();

                // Broadcast
                GUI.enabled = connection.UseBroadcast = GUILayout.Toggle( connection.UseBroadcast, "Use Broadcast?" );                                                                             
                GUILayout.BeginHorizontal();
                Color bgCol = GUI.color;
                GUILayout.Label( "Broadcast Address" );
                GUI.color = Connection.IsValidBroadcastAddress( broadcastAddress ) ? bgCol : Color.red;
                broadcastAddress = GUILayout.TextField( broadcastAddress );
                GUI.color = bgCol;
                GUILayout.EndHorizontal();                 
                GUI.enabled = true;

                // Multicast
                GUILayout.BeginVertical( GUI.skin.box );
                GUILayout.Label( "Multicast" );
                GUILayout.BeginHorizontal();                

                // Available
                GUILayout.BeginVertical( GUI.skin.box );
                GUILayout.Label( "Available" );
                string[] unusedMC = connection.GetAvailableUnusedMulticastAddresses().ToArray();
                string[] usedMC = connection.MulticastGroups.ToArray();
                selectedUnusedMultiCastAddress = GUILayout.SelectionGrid( selectedUnusedMultiCastAddress, unusedMC, 1 );
                GUILayout.EndVertical();

                // Add/Remove
                GUILayout.BeginVertical();
                if( GUILayout.Button( ">" ) )
                {
                    if( unusedMC.Length > 0 )
                    {
                        connection.MulticastGroups.Add( unusedMC[selectedUnusedMultiCastAddress] );
                        selectedUnusedMultiCastAddress = 0;
                    }
                }
                if( GUILayout.Button( "<" ) )
                {
                    if( usedMC.Length > 0 )
                    {
                        connection.MulticastGroups.Remove( usedMC[selectedUsedMultiCastAddress] );
                        usedMC = connection.MulticastGroups.ToArray();
                        selectedUsedMultiCastAddress = 0;
                    }
                }
                GUILayout.EndVertical();
               
                // Subscribed
                GUILayout.BeginVertical( GUI.skin.box );
                GUILayout.Label( "Subscribed" );
                selectedUsedMultiCastAddress = GUILayout.SelectionGrid( selectedUsedMultiCastAddress, usedMC, 1 );
                GUILayout.EndVertical();
                
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                
                // Start/Stop
                GUILayout.BeginHorizontal();
                GUI.enabled = !connection.IsActive;
                if( GUILayout.Button( "Start" ) )
                {
                    connection.StartConnection();
                }

                GUI.enabled = connection.IsActive;
                if( GUILayout.Button( "Stop" ) )
                {
                    connection.StopConnection();
                }
                GUI.enabled = true;
                GUILayout.EndHorizontal();                
            }

            GUI.DragWindow();
        }     
    }
}