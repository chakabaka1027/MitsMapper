#if DIS_VERSION_7

using System;
using System.IO;
using System.Runtime.InteropServices;
using DISUnity.DataType.Enums;
using UnityEngine;
using DISUnity.Attributes;
using DISUnity.Resources;
using System.Text;

namespace DISUnity.DataType
{
    /// <summary>
    /// Represents the explosion of a non-munition.
	/// The exploding object may be an entity for which an Entity State PDU has been issued or may be
    /// an articulated or attached part of an entity. If it is necessary to provide more details of 
    /// the characteristics ofthe explosion, one or more VP records may be developed and included
    /// in the VP record section of	the Detonation PDU to convey such information. 
    /// NOTE: Any munition that explodes, whether for its intended purpose or not, is represented by
    /// the Munition Descriptor record and not an Explosion Description record.
	/// This descriptor is applicable to the Detonation PDU.
    /// </summary>
    /// <size>16 bytes</size>
    [Serializable]
    public class ExplosionDescriptor : Descriptor
    {
        #region Properties

        #region Private
        
        [SerializeField]
        private ExplosiveMaterial explosiveMaterial;

        [Tooltip( Tooltips.ExplosiveForce )]
        [SerializeField]
        private float explosiveForce;

        #endregion Private

        /// <summary>
        /// Indicates the material that exploded.
        /// </summary>
        public ExplosiveMaterial ExplosiveMaterial
        {
            get
            {
                return explosiveMaterial;
            }
            set
            {
                isDirty = true;
                explosiveMaterial = value;
            }
        }

        /// <summary>
        /// The explosive force expressed as the equivalent kilograms of TNT (4.184 x 106 Joules per kilogram).
        /// </summary>
        public float ExplosiveForce
        {
            get
            {
                return explosiveForce;
            }
            set
            {
                isDirty = true;
                explosiveForce = value;
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
            explosiveMaterial = ( ExplosiveMaterial )br.ReadUInt16();
            br.BaseStream.Seek( 2, SeekOrigin.Current ); // Skip padding
            explosiveForce = br.ReadSingle();
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {                  
            isDirty = false;
            base.Encode( bw );
            bw.Write( ( ushort )explosiveMaterial );
            bw.Write( ( ushort )0 ); // Padding 16 bits
            bw.Write( explosiveForce );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( "Explosion Descriptor\n\t" );
            sb.AppendLine( base.ToString() );
            sb.AppendFormat( "\tMaterial: {0}\n", explosiveMaterial );
            sb.AppendFormat( "\tForce: {0}\n", explosiveForce );                        
            return sb.ToString();
        }        
        
        #endregion DataTypeBase    
   
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( ExplosionDescriptor b )
        {
            if( !base.Equals( b )                                ) return false;
            if( !explosiveMaterial.Equals( b.explosiveMaterial ) ) return false;
            if( !explosiveForce.Equals( b.explosiveForce )       ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( ExplosionDescriptor a, ExplosionDescriptor b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}
#endif