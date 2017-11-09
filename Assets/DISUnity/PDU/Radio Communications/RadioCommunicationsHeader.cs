using UnityEngine;
using System;
using DISUnity.DataType;
using DISUnity.Attributes;
using DISUnity.Resources;
using DISUnity.DataType.Enums;
using System.IO;
using System.Text;

namespace DISUnity.PDU.RadioCommunications
{
    /// <summary>
    /// Header for all PDU from the radio communications family. 
    /// </summary>
    /// <DIS_Version>5</DIS_Version>
    /// <size>20 bytes</size>
    [Serializable]
    public class RadioCommunicationsHeader : Header
    {
        #region Properties

        #region Private        

        [Tooltip( Tooltips.RadioReferenceID )]
        [SerializeField]
        private EntityIdentifier radioRefID = new EntityIdentifier();

        [Tooltip( Tooltips.RadioNumber )]
        [SerializeField]
        private int radioNumber = 1;

        #endregion Private

        /// <summary>
        /// Total size of PDU in bytes.
        /// </summary>        
        public override int Length
        {
            get
            {
                return 20;
            }
        }

        /// <summary>
        /// For attached radios, this field should identify the Entity Identifier record or Object Identifier 
        /// record to which the radio is attached. For unattached radios, this field should contain the 
        /// Unattached Identifier record. Note: The combination of the Radio Reference ID and the Radio Number 
        /// field uniquely identifies a particular radio within a simulation exercise. This combination is 
        /// referred to as the Radio Identifier. The Radio Identifier is used to associate Transmitter, 
        /// Signal, and Receiver PDUs with the same radio.
        /// </summary>
        public EntityIdentifier RadioReferenceID
        {
            get
            {
                return radioRefID;
            }
            set
            {
                isDirty = true;
                radioRefID = value;
            }
        }

        /// <summary>
        /// Identifies a radio/communications device belonging to the entity. IDs should be assigned sequentially to
        /// entities, starting with 1. Also known as Radio Number in DIS 7. Note: The combination of the Radio Reference 
        /// ID and the Radio Number field uniquely identifies a particular radio within a simulation exercise. This 
        /// combination is referred to as the Radio Identifier. The Radio Identifier is used to associate Transmitter,
        /// Signal, and Receiver PDUs with the same radio.
        /// </summary>
        public ushort RadioNumber
        {
            get
            {
                return ( ushort )radioNumber;
            }
            set
            {
                isDirty = true;
                radioNumber = value;
            }
        }

        #endregion Properties

        public RadioCommunicationsHeader()
        {
            protocolVersion = ProtocolVersion.IEEE_1278_1_1995; // Min version required to support this PDU            
            protocolFamily = ProtocolFamily.RadioCommunications;
        }

        /// <summary>
        /// Creates a new instance from a binary stream only decoding the body.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public RadioCommunicationsHeader( Header h, BinaryReader br )
        {
            Decode( h, br );
        }

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public virtual void Decode( Header h, BinaryReader br )
        {
            // Copy header values.
            Clone( h );

            radioRefID.Decode( br );
            radioNumber = br.ReadUInt16();
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            base.Encode( bw ); // Header

            radioRefID.Encode( bw );
            bw.Write( ( ushort )radioNumber );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.Append( "Radio Reference ID: " + RadioReferenceID.ToString() );
            sb.AppendLine( "Radio Number: " + radioNumber );            
            return sb.ToString();
        }

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( RadioCommunicationsHeader b )
        {
            if( !Header.Equals( this, b )                      ) return false;
            if( !RadioReferenceID.Equals( b.RadioReferenceID ) ) return false;
            if( radioNumber != b.radioNumber                   ) return false; 
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( RadioCommunicationsHeader a, RadioCommunicationsHeader b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}