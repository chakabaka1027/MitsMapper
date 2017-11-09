using System;
using System.IO;
using System.Runtime.InteropServices;
using DISUnity.DataType.Enums;
using UnityEngine;
using DISUnity.DataType;
using System.Text;
using DISUnity.Attributes;
using DISUnity.Resources;

namespace DISUnity.DataType
{
    /// <summary>
    /// Base class for Descriptor records.  
    /// Muntion / Burst Descriptor:
    /// The Munition Descriptor record is applicable to the Fire PDU and Detonation PDU.
    /// The Descriptor field is found in the Fire and Detonation PDU. It provides additional characteristics related
    /// to something that is fired or launched, or that detonates, bursts, or ignites. 
    /// Before DIS 7 only one record existed known as the BurstDescriptor, now known as the Munition Descriptor in DIS 7. DIS 7 
    /// introduced 2 new descriptors, Explosion & Expendable.
    /// If DIS_VERSION_5 or DIS_VERSION_6 is defined then the only Descriptor record available will be the Munition Descriptor. 
    /// DIS_VERSION_7 will allow you to use Explosion & Expendable.   
    /// </summary>
    /// <size>16 bytes</size>
    [Serializable]
    public abstract class Descriptor : DataTypeBaseComplex<Descriptor>
    {
        #region Properties

        #region Private
        
        [SerializeField]
        protected EntityType type;

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
        /// Munition, Explosion or Expendable Type.
        /// </summary>
        public EntityType Type
        {
            get
            {
                return type;
            }
            set
            {
                isDirty = true;
                type = value;
            }
        }        

        #endregion Properties

        #region DataTypeBase

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        public override void Decode( BinaryReader br )
        {
            isDirty = true;
            type.Decode( br );                        
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            type.Encode( bw );            
            isDirty = false;
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return type.ToString();
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( Descriptor b )
        {
            if( !type.Equals( b.type ) ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( Descriptor a, Descriptor b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}