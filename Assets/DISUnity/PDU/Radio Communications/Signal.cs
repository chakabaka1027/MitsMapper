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
    /// Contains actual transmission of voice, audio, or other data such as an an index 
	/// into a database that defines the signal.    
    /// </summary>
    /// <DIS_Version>5</DIS_Version>
    /// <size>32 bytes min size</size>
    [Serializable]
    public class Signal : RadioCommunicationsHeader, IPduBodyDecoder
    {
        #region Properties

        #region Private
        
        [SerializeField]
        private EncodingScheme encodingScheme = new EncodingScheme();

        [Tooltip( Tooltips.SampleRate )]
        [SerializeField]
        private int sampleRate;
        
        [SerializeField]
        private int samples;

        [SerializeField]
        private byte[] data;

        #endregion Private

        /// <summary>
        /// Total size of PDU including padding in bytes.
        /// </summary>        
        public override int Length
        {
            get
            {
                int l = 32; // Min size
                int dl = Mathf.CeilToInt( DataLength / 8.0f ); // Add data length
                int padding = dl % 4 == 0 ? 0 : ( 4 - dl % 4 ); // Add padding
                return l + dl + padding;
            }
        }

        /// <summary>
        /// Encoding scheme used for the data.
        /// </summary>
        public EncodingScheme EncodingScheme
        {
            get
            {
                return encodingScheme;
            }
            set
            {
                isDirty = true;
                encodingScheme = value;
            }
        }

        /// <summary>
        /// Sample rate in samples per second if the encoding class is encoded 
        /// audio or the data rate in bits per second for data transmissions.
        /// </summary>
        public uint SampleRate
        {
            get
            {
                return ( uint )sampleRate;
            }

            set
            {
                isDirty = true;
                sampleRate = ( int )value;
            }
        }

        /// <summary>
        /// The number of bits of digital voice audio or digital data being sent in the PDU.
        /// </summary>
        public ushort DataLength
        {
            get
            {
                return ( ushort )( data.Length * 8 );
            }
        }

        /// <summary>
        /// The number of samples in this PDU.
        /// </summary>
        public ushort Samples
        {
            get
            {
                return ( ushort )samples;
            }
            set
            {
                isDirty = true;
                samples = value;
            }
        }

        /// <summary>
        /// The audio or digital data conveyed by the radio transmission.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                isDirty = true;
                data = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// Ctor
        /// </summary>
        public Signal()
        {
            pDUType = PDUType.Signal;
        }

        /// <summary>
        /// Creates a new instance from a binary stream only decoding the body.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public Signal( Header h, BinaryReader br )
        {
            Decode( h, br );
        }

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public override void Decode( Header h, BinaryReader br )
        {
            base.Decode( h, br );

            encodingScheme.Decode( br );
            sampleRate = ( int )br.ReadUInt32();
            int dataLength = Mathf.CeilToInt( br.ReadUInt16() / 8.0f ) ; // Convert to bytes
            samples = br.ReadUInt16();
            data = br.ReadBytes( dataLength );
            
            // Skip any padding
            int padding = dataLength % 4 == 0 ? 0 : ( 4 - dataLength % 4 );
            br.BaseStream.Seek( padding, SeekOrigin.Current );
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            base.Encode( bw ); // Header

            encodingScheme.Encode( bw );
            bw.Write( SampleRate );
            bw.Write( DataLength );
            bw.Write( Samples );
            bw.Write( data );

            // Add padding
            int dl = Mathf.CeilToInt( DataLength / 8.0f ); // Convert to bytes
            int padding = dl % 4 == 0 ? 0 : ( 4 - dl % 4 );
            for( int i = 0; i < padding; ++i )
            {
                bw.Write( ( byte )0 );                    
            }            
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.Append( encodingScheme.ToString() );
            sb.AppendLine( "Sample Rate: " + sampleRate );
            sb.AppendLine( "Data Length: " + DataLength );
            sb.AppendLine( "Samples: " + samples );                        
            return sb.ToString();
        }

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( Signal b )
        {
            if( !RadioCommunicationsHeader.Equals( this, b ) ) return false;            
            if( !encodingScheme.Equals( b.encodingScheme )   ) return false;
            if( sampleRate != b.sampleRate                   ) return false;
            if( DataLength != b.DataLength                   ) return false;
            if( samples    != b.samples                      ) return false;
            if( !data.Equals( b.data )                       ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( Signal a, Signal b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}