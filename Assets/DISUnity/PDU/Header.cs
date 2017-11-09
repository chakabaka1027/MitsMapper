using System;
using UnityEngine;
using DISUnity.DataType.Enums;
using DISUnity.DataType;
using System.Text;
using System.IO;
using DISUnity.Attributes;
using DISUnity.Resources;
using System.Net;

namespace DISUnity.PDU
{
    /// <summary>
    /// PDU Header. Contains information used to identify the PDU type that is represented by the datagram.
	/// This header contains all features up to DIS version 6(1278.1a).
    /// </summary>
    /// <DIS_Version>5</DIS_Version>
    /// <size>12 bytes</size>
    [Serializable]
    public class Header : DataTypeBaseSimple
    {
        #region Properties

        #region Private        

        [HideInInspector]
        [SerializeField]         
        protected ProtocolVersion protocolVersion;

        [Tooltip( Tooltips.ExerciseID )]
        [SerializeField]           
        protected byte exerciseID;

        [HideInInspector]
        [SerializeField]
        protected PDUType pDUType;

        [HideInInspector]        
        [SerializeField]
        protected ProtocolFamily protocolFamily;

        [SerializeField]        
        protected TimeStamp timeStamp = new TimeStamp();

        [HideInInspector]
        [SerializeField]        
        protected int length;

        #if DIS_VERSION_7
        [HideInInspector]
        [SerializeField]
        protected byte ownershipStatus;

        [HideInInspector]
        [SerializeField]
        protected byte pduStatus;
        #endif

        #endregion Private
                
        #region PDU Status
        #if DIS_VERSION_7

        /// <summary>
        /// Coupled Extension Indicator.
        /// Indicates whether the PDU is Coupled(true) or Not Coupled(false) with an Attribute PDU.
        /// Applies to all PDU's(1-71) except the Attribute PDU.
        /// </summary>
        /// <remarks>
        /// DIS 7 feature.
        /// Bit 3 of PDU status field.
        /// </remarks>
        public bool CEI
        {
            get
            {
                return ( pduStatus & 0x08 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                pduStatus = ( byte )( value ? ( pduStatus | 0x08 ) : ( pduStatus & ~0x08 ) );
            }
        }

        #endif
        #endregion PDU Status
        
        /// <summary>
        /// DIS protocol version.
        /// </summary>
        /// <remarks>
        /// In DISUnity the version is automatically set by each PDU
        /// to the minimum version of DIS required to support the current PDU.
        /// </remarks>   
        public ProtocolVersion ProtocolVersion
        {
            get
            {
                return protocolVersion;
            }

            set
            {
                isDirty = true;
                protocolVersion = value;
            }
        }

        /// <summary>
        /// Unique to each exercise being conducted simultaneously. 0-255
        /// </summary>        
        public byte ExerciseID
        {
            get
            {
                return exerciseID;
            }

            set
            {
                isDirty = true;
                exerciseID = value;
            }
        }

        /// <summary>
        /// The type of PDU. Set by PDU automatically.
        /// </summary>
        public PDUType PDUType
        {
            get
            {
                return pDUType;
            }

            set
            {
                isDirty = true;
                pDUType = value;
            }
        }
        
        /// <summary>
        /// Family of protocols to which the PDU belongs. Set by PDU automatically.
        /// Only change if you know what you are doing.
        /// </summary>
        public ProtocolFamily ProtocolFamily
        {
            get
            {
                return protocolFamily;
            }

            set
            {
                isDirty = true;
                protocolFamily = value;
            }
        }

        /// <summary>
        /// Specifies the time which the data in the PDU is valid.
        /// </summary>
        public TimeStamp TimeStamp
        {
            get
            {            
                return timeStamp;
            }
            set
            {                
                timeStamp = value;
            }
        }

        /// <summary>
        /// Total size of PDU in bytes.
        /// </summary>        
        public override int Length
        {
            get
            {
                return length;
            }
        }
 
        #endregion Properties

        public Header()
        {
        }

        public Header( Header h )
        {
            Clone( h );
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public Header( BinaryReader br )
        {
            Decode( br );
        }
        
        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        public override void Decode( BinaryReader br )
        {            
            isDirty = true;
            protocolVersion = ( ProtocolVersion ) br.ReadByte();
            exerciseID = br.ReadByte();
            pDUType = ( PDUType )br.ReadByte();
            protocolFamily = ( ProtocolFamily )br.ReadByte();
            timeStamp = new TimeStamp( br );
            length = br.ReadUInt16();

            #if DIS_VERSION_7
            ownershipStatus = br.ReadByte();
            pduStatus = br.ReadByte();            
            #else            
            br.BaseStream.Seek( 2, SeekOrigin.Current ); // Skip padding
            #endif
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( ( byte )protocolVersion );
			bw.Write( ( byte )exerciseID );
            bw.Write( ( byte )pDUType );
            bw.Write( ( byte )protocolFamily );
            timeStamp.Encode( bw );
            bw.Write( ( ushort )Length ); // Use the property so it can be overriden and calculate the length on demand.

            #if DIS_VERSION_7
            bw.Write( ownershipStatus );
            bw.Write( pduStatus );
            #else
            bw.Write( ( ushort )0 ); // Padding
            #endif

            isDirty = false;
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine( pDUType.ToString() );
            sb.AppendLine( string.Format( "{0, -20} : {1, -30}", "Protocol Version", protocolVersion ) );
            sb.AppendLine( string.Format( "{0, -20} : {1, -30}", "Exercise ID", exerciseID ) );
            sb.AppendLine( string.Format( "{0, -20} : {1, -30}", "Protocol Family" , protocolFamily ) );
            sb.Append( timeStamp.ToString() );
            sb.AppendLine( string.Format( "{0, -20} : {1, -30}", "PDU Length", length ) );
            return sb.ToString();
        }

        #region Operators

        /// <summary>
        /// Performs shallow copy.
        /// </summary>
        /// <param name="h"></param>
        public void Clone( Header h )
        {
            pDUType = h.pDUType;
            protocolVersion = h.protocolVersion;
            exerciseID = h.exerciseID;
            protocolFamily = h.protocolFamily;
            timeStamp = h.timeStamp;
            length = h.length;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( Header b )
        {
            if( pDUType         != b.pDUType         ) return false;
            if( protocolVersion != b.protocolVersion ) return false;
            if( exerciseID      != b.exerciseID      ) return false;
            if( protocolFamily  != b.protocolFamily  ) return false;
            if( !timeStamp.Equals(b.timeStamp)       ) return false;
            if( length          != b.length          ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( Header a, Header b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}
