using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using DISUnity.DataType;
using System.Text;
using DISUnity.DataType.Enums;

namespace DISUnity.DataType
{
    // TODO: 
    /*
    Separation (see 6.2.94.6)
    Entity Type (see 6.2.94.5)
    Entity Association (see 6.2.94.4)
    Attached Part (See 6.2.94.3)
    -Articulated Part (see 6.2.94.2)
    */

    /// <summary>
    /// Variable Parameter base class.
    /// </summary>
    /// <size>16 bytes</size>
    [Serializable]
    public class VariableParameter : DataTypeBaseComplex<VariableParameter>
    {
        #region Properties
        
        #region Private

        [HideInInspector]
        [SerializeField]        
        protected VariableParameterType variableParameterType;

        [HideInInspector]
        [SerializeField]
        protected byte[] data;

        #endregion Private

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        public override int Length
        {
            get
            {
                return 16; 
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
        /// The type of VariableParameter record
        /// </summary>
        public VariableParameterType RecordType
        {
            get
            {
                return variableParameterType;
            }
            set
            {
                variableParameterType = value;
                isDirty = true;
            }
        }

        /// <summary>
        /// Raw data for the record. This will be null for derived types however if a record is found
        /// on the network that does not have a specific implementation then the base VariableParameter 
        /// class will be used and the data field populated.
        /// Field should be 15 bytes in length, data will be chopped or padded to fit this size.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;

                if( data == null )
                    data = new byte[15];
                else if( data.Length != 15 )
                    Array.Resize( ref data, 4 );                                                        

                isDirty = true;
            }
        }

        #endregion Properties

        public VariableParameter()
        {
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public VariableParameter( BinaryReader br )
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
            isDirty = true;
            variableParameterType = ( VariableParameterType )br.ReadByte();
            data = br.ReadBytes( 15 );    
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( ( byte )variableParameterType );

            if( data == null )                
                data = new byte[15];            

            bw.Write( data );
            isDirty = false;
        }

        /// <summary>
        /// Default factory decoder. When no specific version of VariableParameter can be
        /// found the base VariableParameter class is used.        
        /// </summary>
        /// <param name="type">The vp type enum.</param>
        /// <param name="br">The entire vp record that has not been decoded.</param>
        /// <returns></returns>
        public static VariableParameter FactoryDecode( int type, BinaryReader br )
        {
            return new VariableParameter( br );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Variable Parameter Type: " + ( VariableParameterType )variableParameterType;
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( VariableParameter b )
        {
            if( variableParameterType != b.variableParameterType ) return false;
            if( !Array.Equals( data, b.data ) ) return false;            
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( VariableParameter a, VariableParameter b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}