using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;
using DISUnity.PDU;
using DISUnity.Attributes;
using System.Collections.Generic;
using DISUnity.Resources;
using System.Net.NetworkInformation;
using DISUnity.DataType;
using DISUnity.Events;

namespace DISUnity.Network
{    
    [AddComponentMenu( "DISUnity/Network/Connection" )]
    public class Connection : MonoBehaviour
    {
        #region Properties

        #region Private

        [SerializeField]
        private int port = 3000;

        [SerializeField]
        private bool useBroadcast = true;

        [BroadcastAddress( "Broadcast Address" )]
        [SerializeField]
        private string broadcastAddress = IPAddress.Broadcast.ToString();

        [MulticastAddressList( "Multicast Groups" )]
        [SerializeField]
        private List<string> multicastGroups = new List<string>();

        [SerializeField]
        private DecodeFactory pduFactory;

        [Tooltip( Tooltips.ReceivePollFrequency )]
        [SerializeField]
        private float receivePollFrequency = 0.1f;

        [Tooltip( Tooltips.StartConnectionOnPlay )]
        [SerializeField]
        private bool startOnPlay = true;

        [Tooltip( Tooltips.UsePDUBundles )]
        [SerializeField]
        private bool usePDUBundles = false;

        [SerializeField]
        private float bundleSendFrequency = 0.1f;

        private List<Header> pduBundle = new List<Header>();
        
        private UdpClient client;

        private static List<byte[]> subnetsMasks;

        // For sending data
        private BinaryWriter writer;
        private MemoryStream stream;

        #endregion

        /// <summary>
        /// Port to use for DIS data, by default 3000.
        /// </summary>
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }

        /// <summary>
        /// Use broadcasting?
        /// </summary>
        public bool UseBroadcast
        {
            get
            {
                return useBroadcast;
            }
            set
            {
                useBroadcast = value;
            }
        }

        /// <summary>
        /// Address to broadcast on.
        /// </summary>
        public string BroadcastAddress
        {
            get
            {
                return broadcastAddress;
            }
            set
            {
                broadcastAddress = value;
            }
        }

        /// <summary>
        /// The multicast addresses that are part of this connection.    
        /// During runtime use AddMulticastAddress & RemoveMulticastAddress or Reset the connection after making changes.
        /// </summary>
        public List<string> MulticastGroups
        {
            get
            {
                return multicastGroups;
            }
            set
            {
                multicastGroups = value;
            }
        }

        /// <summary>
        /// Decodes the received data and converts to the correct PDU's
        /// </summary>
        public DecodeFactory Factory
        {
            get
            {
                return pduFactory;
            }
            set
            {
                pduFactory = value;
            }
        }

        /// <summary>
        /// How often to check for new data on the socket?
        /// </summary>
        public float ReceivePollFrequency
        {
            get
            {
                return receivePollFrequency;
            }
            set
            {
                receivePollFrequency = value;

                if( Application.isPlaying && IsActive )
                {
                    ResetConnection();
                }
            }
        }

        /// <summary>
        /// Is the connection currently active?
        /// </summary>
        public bool IsActive
        {
            get
            {
                return client != null;
            }
        }

        /// <summary>
        /// Network efficiency may be enhanced with PDU bundling. This is the process of concatenating two or more 
        /// PDUs into a single network datagram so that they may be transmitted and relayed through the network in a 
        /// single operation. In DISUnity the Send function will concatenate PDU's into a bundle which will then be sent
        /// periodically(set by the bundle send freq variable). If not enabled PDU will be sent instantly when the Send 
        /// function is called. 
        /// Note: This feature may not be supported by some DIS applications, if you are unsure then it is safer not to use it.
        /// </summary>
        public bool UsePDUBundles
        {
            get
            {
                return usePDUBundles;
            }
            set
            {
                if( usePDUBundles == value ) return;

                usePDUBundles = value;

                if( Application.isPlaying && IsActive )
                {
                    if( value )
                    {
                        InvokeRepeating( "SendBundle", bundleSendFrequency, bundleSendFrequency );
                    }
                    else
                    {
                        CancelInvoke( "SendBundle" );
                    }
                }
            }
        }

        /// <summary>
        /// How often to send a PDU bundle if bundling is enabled.
        /// </summary>
        public float BundleSendFrequency
        {
            get
            {
                return bundleSendFrequency;
            }
            set
            {
                if( Mathf.Approximately( bundleSendFrequency, value ) ) return;

                bundleSendFrequency = value;

                if( Application.isPlaying && IsActive && usePDUBundles )
                {
                    CancelInvoke( "SendBundle" );
                    InvokeRepeating( "SendBundle", bundleSendFrequency, bundleSendFrequency );                    
                }
            }
        }

        #endregion
        
        #region Events

        /// <summary>
        /// Called when the connection is about to start.
        /// </summary>
        public ConnectionEvent connectionStarted;        

        /// <summary>
        /// Called when the connection is stopped.
        /// </summary>
        public ConnectionEvent connectionStopped;        

        #endregion Events

        /// <summary>
        /// Init
        /// </summary>
        private void Awake()
        {
            if( startOnPlay )
            {
                StartConnection(); 
            }
            
            // Used for sending data
            stream = new MemoryStream();
            writer = BitConverter.IsLittleEndian ? new BinaryWriterBigEndian( stream ) : new BinaryWriter( stream );
        }

        /// <summary>
        /// Start the socket sending and receiving on the DIS network
        /// </summary>
        public virtual void StartConnection()
        {
            connectionStarted.Invoke( this );
            gameObject.name = "Connection(Started)";

            CreateSocket();
            
            if( pduFactory == null )
            {
                Debug.LogWarning( "No DecoderFactory assigned, I can not decode DIS data without one. I will create a default factory." );
                pduFactory = gameObject.AddComponent<DecodeFactory>();                
            }

            InvokeRepeating( "CheckForData", 0, receivePollFrequency );

            // Start sending bundles?
            if( usePDUBundles )
            {
                InvokeRepeating( "SendBundle", bundleSendFrequency, bundleSendFrequency );
            } 
        }

        /// <summary>
        /// Stops the connection from sending and receing data.        
        /// </summary>
        public virtual void StopConnection()
        {            
            if( IsActive )
            {
                connectionStopped.Invoke( this );            
                gameObject.name = "Connection(Stopped)";
                CancelInvoke( "CheckForData" );
                client.Close();
                client = null;

                if( usePDUBundles )
                {
                    CancelInvoke( "SendBundle" );                    
                } 
            }
        }

        /// <summary>
        /// Stop & Start the connection
        /// </summary>
        public virtual void ResetConnection()
        {
            StopConnection();
            StartConnection();
        }

        /// <summary>
        /// Creates the UDP socket for broadcast and/or multicast.
        /// </summary>
        private void CreateSocket()
        {
            // Check we have no current connection, if so close it.
            if( IsActive )
            {
                StopConnection();
            }
            
            if( useBroadcast )
            {
                client = new UdpClient( port, IPAddress.Parse( broadcastAddress ).AddressFamily );
            }
            else
            {
                client = new UdpClient( port );
            }

            // Add multicast if any
            if( multicastGroups != null )
            {
                foreach( string mcg in multicastGroups )
                {
                    client.JoinMulticastGroup( IPAddress.Parse( mcg ) );                    
                }
            }
        }

        /// <summary>
        /// Poll the socket for new data.
        /// </summary>
        public void CheckForData()
        {
            // Check if any data is available to receive.
            while( client.Available > 0 )
            {
                IPEndPoint fromHost = new IPEndPoint( IPAddress.Any, port );
                byte[] data = client.Receive( ref fromHost );
                if( data != null )
                {
                    pduFactory.Decode( data );
                }
            }            
        }

        /// <summary>
        /// Sends the current PDU bundle if one exists. This is called periodically if use pdu
        /// bundle is enabled however you can call it manually to force a bundle to be sent.
        /// </summary>
        public void SendBundle()
        {
            // TODO: Padding??
            if( pduBundle.Count > 0 )
            {
                foreach( Header h in pduBundle )
                {
                    // Space for more data?
                    if( stream.Length + h.Length > SymbolicValues.MAX_PDU_SIZE_OCTETS )
                    {
                        // Send the bundle, we have no space left
                        client.Send( stream.GetBuffer(), ( int )stream.Length );

                        // Reset stream for an other bundle
                        stream.Seek( 0, SeekOrigin.Begin );
                        stream.SetLength( 0 );
                    }
                    else
                    {
                        // Append onto bundle
                        h.Encode( writer );
                    }
                }

                // Send
                if( stream.Length > 0 )
                {
                    client.Send( stream.GetBuffer(), ( int )stream.Length );
                }

                // Reset stream for re-use.
                stream.Seek( 0, SeekOrigin.Begin );
                stream.SetLength( 0 );

                pduBundle.Clear();
            }
        }

        /// <summary>
        /// Sends a PDU over the DIS network. If PDU bundles are enabled then the PDU will be added to the bundle and
        /// sent later. If you wish to bypass the bundles use SendImmediate.
        /// </summary>
        /// <param name="pdu"></param>        
        public void Send( Header pdu )
        {
            if( usePDUBundles )
            {
                pduBundle.Add( pdu );
            }
            else
            {
                SendImmediate( pdu );
            }
        }

        /// <summary>
        /// Sends a PDU over the DIS network regardless of PDU bundle settings.
        /// </summary>
        /// <param name="pdu"></param>
        public void SendImmediate( Header pdu )
        {                        
            pdu.Encode( writer );

            client.Send( stream.GetBuffer(), ( int )stream.Length );

            // Reset stream for re-use.
            stream.Seek( 0, SeekOrigin.Begin );
            stream.SetLength( 0 );
        }

        /// <summary>
        /// Adds a multicast address to the socket.
        /// </summary>
        /// <param name="address"></param>
        public void AddMulticastAddress( string address )
        {
            multicastGroups.Add( address );
            if( client != null )
            {
                client.JoinMulticastGroup( IPAddress.Parse( address ) );
            }
        }

        /// <summary>
        /// Leaves a multicast group.
        /// </summary>
        /// <param name="address"></param>
        public void RemoveMulticastAddress( string address )
        {
            multicastGroups.Remove( address );
            if( client != null )
            {
                client.DropMulticastGroup( IPAddress.Parse( address ) );
            }
        }

        /// <summary>
        /// Returns a list of all available multicast addresses.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailableMulticastAddresses()
        {
            List<string> mcAddresses = new List<string>();            
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            if( interfaces != null && interfaces.Length > 0 )
            {
                foreach( NetworkInterface adapter in interfaces )
                {                
                    MulticastIPAddressInformationCollection ipInfos = adapter.GetIPProperties().MulticastAddresses;
                    if( ipInfos != null && ipInfos.Count > 0 )
                    {
                        foreach( MulticastIPAddressInformation mctIPAddressInformation in ipInfos )
                        {
                            if( mctIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork )
                            {
                                string add = mctIPAddressInformation.Address.ToString();

                                // Dont add duplicates
                                if( !mcAddresses.Contains( add ) )
                                {
                                    mcAddresses.Add( add );
                                }
                            }
                        }
                    }
                }
            }
            return mcAddresses;
        }

        /// <summary>
        /// Returns all multicast addresses that are not currently part of the connection.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAvailableUnusedMulticastAddresses()
        {
            // Get all
            List<string> allMCAdd = GetAvailableMulticastAddresses();

            // Remove if it is in our list
            allMCAdd.RemoveAll( o => multicastGroups.Contains( o ) );

            return allMCAdd;
        }

        /// <summary>
        /// Checks a broadcast address and returns true if it is valid else false.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsValidBroadcastAddress( string address )
        {
            if( subnetsMasks == null )
            {
                subnetsMasks = new List<byte[]>();
                try
                {
                    NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                    if( interfaces != null && interfaces.Length > 0 )
                    {
                        foreach( NetworkInterface adapter in interfaces )
                        {
                            UnicastIPAddressInformationCollection ipInfos = adapter.GetIPProperties().UnicastAddresses;
                            if( ipInfos != null && ipInfos.Count > 0 )
                            {
                                foreach( UnicastIPAddressInformation uIPAddressInformation in ipInfos )
                                {
                                    if( uIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork )
                                    {
                                        //subnetsMasks.Add( uIPAddressInformation.IPv4Mask.GetAddressBytes() );
                                    }
                                }
                            }
                        }
                    }
                }
                catch( NotImplementedException )
                {
                    // TODO: Validate a different way, this is thrown by some versions of mono
                    return true;
                }
            }

            if( subnetsMasks.Count > 0 )
            {
                try
                {
                    IPAddress ipadd = IPAddress.Parse( address );

                    // Is this a valid broadcast address?
                    uint addressBits = BitConverter.ToUInt32( ipadd.GetAddressBytes(), 0 );
                    foreach( byte[] mask in subnetsMasks )
                    {
                        uint invertedSubnetBits = ~BitConverter.ToUInt32( mask, 0 );
                        bool broadcastValid = ( ( addressBits & invertedSubnetBits ) == invertedSubnetBits );
                        if( broadcastValid ) return true;
                    }
                }
                catch( Exception )
                {
                    return false;
                }
            }
            return false;
        }
    }
}

