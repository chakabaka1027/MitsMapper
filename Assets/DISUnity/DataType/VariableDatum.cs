using System;
using System.IO;
using UnityEngine;
using DISUnity.DataType;
using DISUnity.DataType.Enums;
using DISUnity.Resources;
using System.Text;

namespace DISUnity.DataType
{
    /// <summary>
    /// Stores variable datum values.
    /// </summary>
    /// <size>8 bytes</size>
    [Serializable]
	public class VariableDatum : DataTypeBaseComplex<VariableDatum>
	{
        #region Properties
        
        #region Private
        
        [SerializeField]
        protected DatumID datumID;

		[SerializeField]
		protected int datumLengthInBits;

        [SerializeField]
		protected byte[] data;
		
		#endregion Private

        /// <summary>
        /// Size of this data type in bytes. Includes the padding that may have been added to the data.
        /// </summary>
        /// <returns></returns>
        public override int Length
        {
            get
            {
                return 8 + ( int )DatumLengthIncludingPadding / 8; 
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
        /// The type of variable datum to be communicated.
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
		/// Length of datum in bits not including padding.
		/// </summary>
		public uint DatumLength
		{
			get
			{
				return ( uint )datumLengthInBits;
			}			
		}

        /// <summary>
        /// Length of datum in bits including padding.
        /// </summary>        
        public uint DatumLengthIncludingPadding
        {
            get
            {
                return ( uint )Mathf.Ceil( ( float )datumLengthInBits / 64.0f ) * 64;
            }
        }

        public enum DatumDataType
        {
            Unknown,
            Longs,
            ULongs,
            Doubles,
            String
        }

        [SerializeField]
        [Tooltip( Tooltips.InternalDataType )]
        protected DatumDataType internalDataType;

        /// <summary>
        /// Raw data for the record including padding.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
        }
        
        #endregion Properties

		public VariableDatum()
		{
		}

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public VariableDatum( BinaryReader br )
        {
            Decode( br );
        }

        /// <summary>
        /// Treats the internal data as a list of 64 bit integers.
        /// </summary>
        /// <returns>List of converted longs or null if no data exists or something went wrong.</returns>
        public long[] GetAsLongList()
        {
            internalDataType = DatumDataType.Longs;

            if( data == null )
                return null;

            if( datumLengthInBits % 64 != 0 )            
                Debug.LogWarning( "The data is not the correct size (must be multiple of 64 bits)." +
                                  "This data is " + datumLengthInBits + " bits. The last " + datumLengthInBits % 64 +
                                  " bits will be ignored." );

            // Validate that we actually have the data
            if( data.Length != DatumLengthIncludingPadding / 8 )
            {
                Debug.LogError( "Data length is incorrect, it should be " + DatumLengthIncludingPadding +
                                " but it is " + data.Length * 8 );
                return null;
            }                        

            int valuesToConvert = Mathf.FloorToInt( datumLengthInBits / 64.0f );
            long[] values = new long[valuesToConvert];
            byte[] tmpBuffer = new byte[8];
            for( int i = 0; i < valuesToConvert; ++i )
            {
                // Do we need to endian swap?
                if( BitConverter.IsLittleEndian )
                {
                    Buffer.BlockCopy( data, i * 8, tmpBuffer, 0, 8 );
                    Array.Reverse( tmpBuffer );
                    values[i] = BitConverter.ToInt64( tmpBuffer, 0 );
                }
                else
                    values[i] = BitConverter.ToInt64( data, i * 8 );
            }

            return values;
        }

        /// <summary>
        /// Treats the internal data as a list of 64 bit unsigned integers.
        /// </summary>
        /// <returns>List of converted ulongs or null if no data exists or something went wrong.</returns>
        public ulong[] GetAsULongList()
        {
            internalDataType = DatumDataType.ULongs;

            if( data == null )
                return null;

            if( datumLengthInBits % 64 != 0 )
                Debug.LogWarning( "The data is not the correct size (must be multiple of 64 bits)." +
                                  "This data is " + datumLengthInBits + " bits. The last " + datumLengthInBits % 64 +
                                  " bits will be ignored." );

            // Validate that we actually have the data
            if( data.Length != DatumLengthIncludingPadding / 8 )
            {
                Debug.LogError( "Data length is incorrect, it should be " + DatumLengthIncludingPadding +
                                " but it is " + data.Length * 8 );
                return null;
            }

            int valuesToConvert = Mathf.FloorToInt( datumLengthInBits / 64.0f );
            ulong[] values = new ulong[valuesToConvert];
            byte[] tmpBuffer = new byte[8];
            for( int i = 0; i < valuesToConvert; ++i )
            {
                // Do we need to endian swap?
                if( BitConverter.IsLittleEndian )
                {
                    Buffer.BlockCopy( data, i * 8, tmpBuffer, 0, 8 );
                    Array.Reverse( tmpBuffer );
                    values[i] = BitConverter.ToUInt64( tmpBuffer, 0 );
                }
                else
                    values[i] = BitConverter.ToUInt64( data, i * 8 );
            }

            return values;
        }

        /// <summary>
        /// Treats the internal data as a list of doubles.
        /// </summary>
        /// <returns>List of converted doubles or null if no data exists or something went wrong.</returns>
        public double[] GetAsDoubleList()
        {
            internalDataType = DatumDataType.Doubles;

            if( data == null )
                return null;

            if( datumLengthInBits % 64 != 0 )
                Debug.LogWarning( "The data is not the correct size (must be multiple of 64 bits)." +
                                  "This data is " + datumLengthInBits + " bits. The last " + datumLengthInBits % 64 +
                                  " bits will be ignored." );

            // Validate that we actually have the data
            if( data.Length != DatumLengthIncludingPadding / 8 )
            {
                Debug.LogError( "Data length is incorrect, it should be " + DatumLengthIncludingPadding +
                                " but it is " + data.Length * 8 );
                return null;
            }

            int valuesToConvert = Mathf.FloorToInt( datumLengthInBits / 64.0f );
            double[] values = new double[valuesToConvert];
            byte[] tmpBuffer = new byte[8];
            for( int i = 0; i < valuesToConvert; ++i )
            {
                // Do we need to endian swap?
                if( BitConverter.IsLittleEndian )
                {
                    Buffer.BlockCopy( data, i * 8, tmpBuffer, 0, 8 );
                    Array.Reverse( tmpBuffer );
                    values[i] = BitConverter.ToDouble( tmpBuffer, 0 );
                }
                else
                    values[i] = BitConverter.ToDouble( data, i * 8 );
            }

            return values;
        }

         /// <summary>
        /// Treats the internal data as a string.
        /// </summary>
        /// <returns>Converted string.</returns>
        public string GetAsString()
        {
            internalDataType = DatumDataType.String;

            if( data == null )
                return string.Empty;

            if( datumLengthInBits % 8 != 0 )
                Debug.LogWarning( "The data is not the correct size (must be multiple of 8 bits)." +
                                  "This data is " + datumLengthInBits + " bits. The last " + datumLengthInBits % 8 +
                                  " bits will be ignored." );

            return Encoding.ASCII.GetString( data, 0, Mathf.FloorToInt( datumLengthInBits / 8.0f ) );            
        }

        /// <summary>
        /// Sets the data from a list of longs.
        /// </summary>
        /// <param name="values"></param>
        public void SetData( long[] values )
        {            
            int datumLengthInBytes = values.Length * 8; 
            datumLengthInBits = datumLengthInBytes * 8;

            if( data == null )
                data = new byte[datumLengthInBytes];
            else if( data.Length != datumLengthInBits )
                Array.Resize( ref data, datumLengthInBytes );

            for( int i = 0; i < values.Length; ++i )
            {
                byte[] convertedLongBytes = BitConverter.GetBytes( values[i] );

                // Do we need to endian swap?
                if( BitConverter.IsLittleEndian )                
                    Array.Reverse( convertedLongBytes );

                // Copy into the data buffer
                Buffer.BlockCopy( convertedLongBytes, 0, data, i * 8, 8 );                
            }                               
        }

        /// <summary>
        /// Sets the data from a list of ulongs.
        /// </summary>
        /// <param name="values"></param>
        public void SetData( ulong[] values )
        {
            int datumLengthInBytes = values.Length * 8;
            datumLengthInBits = datumLengthInBytes * 8;

            if( data == null )
                data = new byte[datumLengthInBytes];
            else if( data.Length != datumLengthInBits )
                Array.Resize( ref data, datumLengthInBytes );

            for( int i = 0; i < values.Length; ++i )
            {
                byte[] convertedLongBytes = BitConverter.GetBytes( values[i] );

                // Do we need to endian swap?
                if( BitConverter.IsLittleEndian )
                    Array.Reverse( convertedLongBytes );

                // Copy into the data buffer
                Buffer.BlockCopy( convertedLongBytes, 0, data, i * 8, 8 );
            }
        }

        /// <summary>
        /// Sets the data from a list of doubles.
        /// </summary>
        /// <param name="values"></param>
        public void SetData( double[] values )
        {
            int datumLengthInBytes = values.Length * 8;
            datumLengthInBits = datumLengthInBytes * 8;

            if( data == null )
                data = new byte[datumLengthInBytes];
            else if( data.Length != datumLengthInBits )
                Array.Resize( ref data, datumLengthInBytes );

            for( int i = 0; i < values.Length; ++i )
            {
                byte[] convertedLongBytes = BitConverter.GetBytes( values[i] );

                // Do we need to endian swap?
                if( BitConverter.IsLittleEndian )
                    Array.Reverse( convertedLongBytes );

                // Copy into the data buffer
                Buffer.BlockCopy( convertedLongBytes, 0, data, i * 8, 8 );
            }
        }

        /// <summary>
        /// Sets the data from a string.
        /// </summary>
        /// <param name="values"></param>
        public void SetData( string text )
        {
            internalDataType = DatumDataType.String;
            datumLengthInBits = text.Length * 8;

            if( string.IsNullOrEmpty( text ) )
            {
                data = null;
            }
            else
            {
                // Add padding directly to the string.
                text = text.PadRight( text.Length + ( 8 - ( text.Length % 8 ) ) );
                data = Encoding.ASCII.GetBytes( text );
            }
        }     

		/// <summary>
		/// Sets the datum value. Calculates and adds any padding required to align on a 64 bit boundary.
		/// </summary>
		/// <param name="value">Byte array containing value to be set.</param>
		/// <param name="sizeInBits">Size of data contained in value in bits.</param>
        public virtual void SetData( byte[] values, uint sizeInBits ) 
		{
            datumLengthInBits = ( int )sizeInBits;            
			int dataSizeWithPaddingInBytes = ( int )DatumLengthIncludingPadding / 8;
			
            // Size the array to include space for padding
			if( data == null )
                data = new byte[dataSizeWithPaddingInBytes];
            else if( data.Length != dataSizeWithPaddingInBytes )
                Array.Resize( ref data, dataSizeWithPaddingInBytes );
            
            // Copy the data into our buffer
			Buffer.BlockCopy( values, 0, data, 0, ( int )sizeInBits / 8 );
            
			isDirty = true;
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
			datumLengthInBits = ( int )br.ReadUInt32();
            int datumLengthWithPaddingInBytes = ( int )DatumLengthIncludingPadding / 8;            
            data = br.ReadBytes( ( int )datumLengthWithPaddingInBytes );   
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {          
            bw.Write( ( uint )datumID );
			bw.Write( ( uint )datumLengthInBits );      
            bw.Write( data );
            isDirty = false;
        }

        /// <summary>
        /// Default factory decoder. When no specific version of VariableDatum can be
        /// found the base VariableDatum class is used.        
        /// </summary>
        /// <param name="type">The DatumID type enum.</param>
        /// <param name="br">The rest of the vp record that has not been decoded.</param>
        /// <returns></returns>
        public static VariableDatum FactoryDecode( int type, BinaryReader br )
        {
            return new VariableDatum( br );            
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Variable Datum ID: " + DatumID;
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( VariableDatum b )
        {
            if( datumID != b.datumID                     ) return false;
			if( datumLengthInBits != b.datumLengthInBits ) return false;
            if( !Array.Equals( data, b.data )            ) return false;            
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
		public static bool Equals( VariableDatum a, VariableDatum b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}