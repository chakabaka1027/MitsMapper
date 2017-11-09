using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using DISUnity.DataType;
using System.Text;
using DISUnity.DataType.Enums;
using DISUnity.Resources;
using System.Net;

namespace DISUnity.DataType
{
    /// <summary>
    /// Stores fixed datum values.
    /// </summary>
    /// <size>8 bytes</size>
    [Serializable]
    public class FixedDatum : DataTypeBaseComplex<FixedDatum>
    {
        #region Properties
        
        #region Private
        
        [SerializeField]
        protected DatumID datumID;

        [SerializeField]
        protected byte[] data = new byte[4];

        public enum DatumDataType
        {
            Unknown,
            Int,
            UInt,
            Float,
        }

        [SerializeField]
        [Tooltip( Tooltips.InternalDataType )]
        protected DatumDataType internalDataType;
      
        #endregion Private

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        public override int Length
        {
            get
            {
                return 8; 
            }
        }

        /// <summary>
        /// A value of null means this is the default
        /// </summary>
        public new static int[] ConcreteTypeEnums
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// The type of fixed datum to be communicated.
        /// </summary>
        public DatumID DatumID
        {
            get
            {
                return datumID;
            }
            set
            {
                datumID = value;
                isDirty = true;
            }
        }

        /// <summary>
        /// Raw data for the record. This will be null for derived types however if a record is found
        /// on the network that does not have a specific implementation then the base FixedDatum 
        /// class will be used and the data field populated.
        /// Field should be 4 bytes in length, data will be chopped or padded to fit this size.
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

                if( data == null )
                    data = new byte[4];
                else if( data.Length != 4 )
                    Array.Resize( ref data, 4 );                                
            }
        }
        
        /// <summary>
        /// The fixed datum supports some simple data types so that users do not need to create a derrived version for every DatumID.
        /// </summary>
        public DatumDataType InternalDataType
        {
            get
            {
                return internalDataType;
            }
            set
            {
                internalDataType = value;
            }
        }

        #endregion Properties

		public FixedDatum()
		{
		}

		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="br"></param>
        public FixedDatum( BinaryReader br )
		{
			Decode( br );
		}

        /// <summary>
        /// Returns the internal data as an integer.
        /// </summary>
        public int GetAsInt()
        {
            if( data == null )
                data = new byte[4];

            internalDataType = DatumDataType.Int;

            // Do we need to endian swap?
            if( BitConverter.IsLittleEndian )
            {
                byte[] dataCpy = new byte[4];
                Array.Copy( data, dataCpy, 4 );
                Array.Reverse( dataCpy );
                return BitConverter.ToInt32( dataCpy, 0 );
            }
            else
                return BitConverter.ToInt32( data, 0 );
        }

        /// <summary>
        /// Treats the internal data as an unsigned integer.
        /// </summary>
        /// <param name="value"></param>
        public uint GetAsUint()
        {
            if( data == null )
                data = new byte[4];

            internalDataType = DatumDataType.UInt;

            // Do we need to endian swap?
            if( BitConverter.IsLittleEndian )
            {
                byte[] dataCpy = new byte[4];
                Array.Copy( data, dataCpy, 4 );
                Array.Reverse( dataCpy );
                return BitConverter.ToUInt32( dataCpy, 0 );
            }
            else
                return BitConverter.ToUInt32( data, 0 );
        }

        /// <summary>
        /// Treats the internal data as a float.
        /// </summary>
        /// <param name="value"></param>
        public float GetAsFloat()
        {
            if( data == null )
                data = new byte[4];

            internalDataType = DatumDataType.Float;

            // Do we need to endian swap?
            if( BitConverter.IsLittleEndian )
            {
                byte[] dataCpy = new byte[4];
                Array.Copy( data, dataCpy, 4 );
                Array.Reverse( dataCpy );
                return BitConverter.ToSingle( dataCpy, 0 );
            }
            else
                return BitConverter.ToSingle( data, 0 );
        }

        /// <summary>
        /// Treats the internal data as an integer.
        /// </summary>
        /// <param name="value"></param>
        public void SetData( int value )
        {
            internalDataType = DatumDataType.Int;
            isDirty = true;

            data = BitConverter.GetBytes( value );

            // Do we need to endian swap?
            if( BitConverter.IsLittleEndian )
                Array.Reverse( data );
        }

        /// <summary>
        /// Treats the internal data as an unsigned integer.
        /// </summary>
        /// <param name="value"></param>
        public void SetData( uint value )
        {
            internalDataType = DatumDataType.UInt;
            isDirty = true;

            data = BitConverter.GetBytes( value );

            // Do we need to endian swap?
            if( BitConverter.IsLittleEndian )
                Array.Reverse( data );
        }

        /// <summary>
        /// Treats the internal data as an integer.
        /// </summary>
        /// <param name="value"></param>
        public void SetData( float value )
        {
            internalDataType = DatumDataType.Float;
            isDirty = true;

            data = BitConverter.GetBytes( value );

            // Do we need to endian swap?
            if( BitConverter.IsLittleEndian )
                Array.Reverse( data );
        }

        #region DataTypeBase
        
        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        public override void Decode( BinaryReader br )
        {
            isDirty = true;
            datumID = ( DatumID )br.ReadUInt32();
            data = br.ReadBytes( 4 );    
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( ( uint )datumID );

            if( data == null )
                data = new byte[4];               
            
            bw.Write( data );
            isDirty = false;
        }

        /// <summary>
        /// Default factory decoder. When no specific version of FixedDatum can be
        /// found the base FixedDatum class is used.        
        /// </summary>
        /// <param name="type">The DatumID type enum.</param>
        /// <param name="br">The rest of the vp record that has not been decoded.</param>
        /// <returns></returns>
        public static FixedDatum FactoryDecode( int type, BinaryReader br )
        {
            return new FixedDatum( br );            
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Fixed Datum ID: " + DatumID;
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( FixedDatum b )
        {
            if( datumID != b.datumID          ) return false;
            if( !Array.Equals( data, b.data ) ) return false;            
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( FixedDatum a, FixedDatum b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}