using System;
using System.IO;
using System.Runtime.InteropServices;
using DISUnity.DataType.Enums;
using UnityEngine;
using DISUnity.DataType;
using System.Text;
using DISUnity.Resources;
using DISUnity.Attributes;

namespace DISUnity.DataType
{
    /// <summary>
    /// Scheme used to encode a set of data.
    /// 
    /// Encoding Type or Number of TDL Messages:
    /// i)The 14 least significant bits of the Encoding Scheme record shall represent Encoding
    /// Type when the Encoding Class is Encoded Audio (0).
    /// ii)The 14 least significant bits of the Encoding Scheme record shall be zero when the
    /// encoding class is not Encoded Audio (0) and the TDL Type is Other (0).
    /// iii)Otherwise, the 14 least significant bits of the Encoding Scheme record shall represent the
    /// number of tactical data link messages contained in the data section of the Signal PDU.
    /// </summary>
    /// <size>4 bytes</size>
    [Serializable]
    public class EncodingScheme : DataTypeBaseSimple
    {
        #region Properties

        #region Private

        [SerializeField]
        private int typeAndClass; // Bits 0-13 type or number TDL and bits 14-15 class.
        
        [SerializeField]
        private TacticalDataLinkType tdlType;

        #endregion Private

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        public override int Length
        {
            get
            {
                return 4;
            }
        }

        /// <summary>
        /// The number of Tactical Data Links if the TDL Type is not 0(Other).        
        /// </summary>
        public ushort NumberTDLMessages
        {
            get
            {
                return ( ushort )( ( typeAndClass & 0x3FFF ) );
            }
            set
            {
                isDirty = true;
                typeAndClass = ( ( int )value ) | ( typeAndClass & ~0x3FFF );
            }
        }

        /// <summary>
        /// Encoding type when encoding class is audio.
        /// Note: Setting this value will cause the type to be set to audio.
        /// Bits 14-15
        /// </summary>
        public SignalEncodingType EncodingType
        {
            get
            {
                return ( SignalEncodingType )( ( typeAndClass & 0x3FFF ) );
            }
            set
            {
                //isDirty = true; // Done in EncodingClass.Set                
                typeAndClass = ( ( int )value ) | ( typeAndClass & ~0x3FFF );
                EncodingClass = SignalEncodingClass.EncodedAudio;
            }
        }
        
        /// <summary>
        /// Encoding Class.
        /// Bits 14-15
        /// </summary>
        public SignalEncodingClass EncodingClass
        {
            get
            {
                return ( SignalEncodingClass )( ( typeAndClass & 0xC000 ) >> 14 );
            }
            set
            {
                isDirty = true;
                typeAndClass = ( ( int )value << 14 ) | ( typeAndClass & ~0xC000 );
            }
        }

        /// <summary>
        /// The type of TDL or 0(Other) if the type is not a TDL.
        /// </summary>
        public TacticalDataLinkType TacticalDataLinkType
        {
            get
            {
                return tdlType;
            }
            set
            {
                isDirty = true;
                tdlType = value;
            }
        }

        /// <summary>
        /// The raw type and class bit field.
        /// </summary>
        public int TypeAndClassBitField
        {
            get
            {
                return typeAndClass;
            }
            set
            {
                isDirty = true;
                typeAndClass = value;
            }
        }

        #endregion Properties

        public EncodingScheme()
        {
        }

        public EncodingScheme( int typeAndClassBits, TacticalDataLinkType tdl )
        {
            typeAndClass = typeAndClassBits;
            tdlType = tdl;
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public EncodingScheme( BinaryReader br )
        {            
            Decode( br );
        }

        #region DataTypeBase

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        public override void Decode( BinaryReader br )
        {
            typeAndClass = br.ReadUInt16();
            tdlType = ( TacticalDataLinkType )br.ReadUInt16();
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( ( ushort )typeAndClass );
            bw.Write( ( ushort )tdlType );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format( "Encoding Scheme: {0}, TDL Type {1}\n ", typeAndClass, tdlType );
        }

        #endregion DataTypeBase

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( EncodingScheme b )
        {
            if( typeAndClass != b.typeAndClass ) return false;
            if( tdlType      != b.tdlType )      return false;                       
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( EncodingScheme a, EncodingScheme b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}