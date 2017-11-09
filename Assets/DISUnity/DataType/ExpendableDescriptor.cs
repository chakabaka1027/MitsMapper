#if DIS_VERSION_7
using System;
using System.IO;
using System.Runtime.InteropServices;
using DISUnity.DataType.Enums;
using UnityEngine;

namespace DISUnity.DataType
{
    /// <summary>
    /// The burst of a chaff expendable or ignition of a flare shall be represented by an Expendable Descriptor.
    /// The Expendable Descriptor record is applicable to the Fire and Detonation PDU. 
    /// </summary>
    /// <size>16 bytes</size>
    [Serializable]
    public class ExpendableDescriptor : Descriptor
    {
        #region DataTypeBase

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        public override void Decode( BinaryReader br )
        {
            isDirty = true;
            base.Decode( br );
            br.BaseStream.Seek( 8, SeekOrigin.Current ); // Skip padding; 
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {                  
            isDirty = false;
            base.Encode( bw );
            bw.Write( 0L ); // Padding 64 bits
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format( "Expendable Descriptor\n\t{0}", base.ToString() );
        }
        
        #endregion DataTypeBase       
    }
}
#endif