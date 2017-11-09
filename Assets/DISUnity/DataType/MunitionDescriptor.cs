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
    /// Muntion / Burst MunitionDescriptor.
    /// The MunitionDescriptor record is applicable to the Fire PDU and Detonation PDU.
    /// It provides additional characteristics related to something that is fired or launched, or that detonates, bursts, or ignites. 
    /// If DIS_VERSION_5 or DIS_VERSION_6 is defined then this record is the only available Descriptor, it represents the BurstDescriptor from pre DIS 7.    
    /// </summary>
    /// <size>16 bytes</size>
    [Serializable]
    public class MunitionDescriptor : Descriptor
    {
        #region Properties

        #region Private
        
        [SerializeField]       
        private Warhead warhead;

        [SerializeField]
        private Fuse fuse;

        [SerializeField]
        private int quantity;

        [SerializeField]
        private int rate;
        
        #endregion Private

        /// <summary>
        /// The type of warhead.
        /// </summary>
        public Warhead Warhead
        {
            get
            {
                return warhead;
            }
            set
            {
                isDirty = true;
                warhead = value;
            }
        }

        /// <summary>
        /// The type of fusing.
        /// </summary>
        public Fuse Fuse
        {
            get
            {
                return fuse;
            }
            set
            {
                isDirty = true;
                fuse = value;
            }
        }

        /// <summary>
        /// Number of rounds fired in the burst or the number of munitions simultaneously launched.
        /// </summary>
        public ushort Quantity
        {
            get
            {
                return ( ushort )quantity;
            }
            set
            {
                isDirty = true;
                quantity = value;
            }
        }

        /// <summary>
        /// Rate of fire such as the rounds per minute for a munition. 
        /// If the Quantity field is equal to one, the Rate field should be set to 0.
        /// </summary>
        public ushort Rate
        {
            get
            {
                return ( ushort )rate;
            }
            set
            {
                isDirty = true;
                rate = value;
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
            base.Decode( br );
            warhead = ( Warhead )br.ReadUInt16();
            fuse = ( Fuse )br.ReadUInt16();
            quantity = br.ReadUInt16();
            rate = br.ReadUInt16();
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {                  
            isDirty = false;
            base.Encode( bw );
            bw.Write( ( ushort )warhead );
            bw.Write( ( ushort )fuse );
            bw.Write( ( ushort )quantity );
            bw.Write( ( ushort )rate );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( "Munition Descriptor\n\t" );
            sb.AppendLine( base.ToString() );
            sb.AppendFormat( "\tWarhead({0}): {1}\n", ( int )warhead, warhead );
            sb.AppendFormat( "\tFuse({0}): {1}\n", ( int )fuse, fuse );
            sb.AppendFormat( "\tQuantity: {0}\n", quantity );
            sb.AppendFormat( "\tRate: {0}\n", rate );
            return sb.ToString();
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( MunitionDescriptor b )
        {
            if( !base.Equals( b )              ) return false;
            if( !warhead.Equals( b.warhead )   ) return false;
            if( !fuse.Equals( b.fuse )         ) return false;
            if( !quantity.Equals( b.quantity ) ) return false;
            if( !rate.Equals( b.rate )         ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( MunitionDescriptor a, MunitionDescriptor b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}