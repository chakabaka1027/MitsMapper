using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using DISUnity.DataType;
using System.Text;
using DISUnity.DataType.Enums;
using DISUnity.Attributes;
using DISUnity.Resources;

namespace DISUnity.DataType
{
    /// <summary>
    /// Specification of removable parts that may be attached to an entity.
    /// E.G. A missile attached to an aircraft wing.
    /// </summary>
    [Serializable]
    public class AttachedPart : VariableParameter
    {
        #region Properties
        
        #region Private

        [SerializeField]
        private byte detachedIndicator;

        [SerializeField]
        [Range( ushort.MinValue, ushort.MaxValue )]
        private int partAttachedToID;

        [SerializeField]
        private AttachedPartParameterType aPPT;

        [SerializeField]
        private EntityType attachedPartType = new EntityType( EntityKind.Expendable, 0, 0, 0, 0, 0, 0 ); // Most common type is Expendable.
 
        #endregion Private

        /// <summary>
        /// The type/s this class supports. 
        /// </summary>
        public new static int[] ConcreteTypeEnums
        {
            get
            {
                return new int[]{ ( int )VariableParameterType.AttachedPart };
            }
        }

        /// <summary>
        /// Indicates whether an attached part is attached or detached. 
        /// Should be set to Attached(0) to indicate the part is attached 
        /// and to Detached(1) if the part becomes detached.
        /// </summary>
        public byte DetachedIndicator
        {
            get
            {
                return detachedIndicator;
            }
            set
            {
                isDirty = true;
                detachedIndicator = value;
            }
        }
                
        /// <summary>
        /// The identification of the articulated or attached part to which this 
        /// attached part is attached.
        /// Should contain the value zero if the attached part is attached 
        /// directly to the entity.
        /// </summary>
        public ushort PartAttachedToID
        {
            get
            {
                return ( ushort )partAttachedToID;
            }
            set
            {
                isDirty = true;
                partAttachedToID = value;
            }
        }

        /// <summary>
        /// The location (or station) to which the part is attached. 
        /// <see cref="AttachedPartParameterType"/> for further details.
        /// </summary>
        public AttachedPartParameterType APPT
        {
            get
            {
                return aPPT;
            }
            set
            {
                isDirty = true;
                aPPT = value;
            }
        }

        /// <summary>
        /// Entity Type record enumeration of the attached part.
        /// E.G This could be the type of missile that is attached to an entity wing.
        /// </summary>
        public EntityType AttachedPartType
        {
            get
            {
                return attachedPartType;
            }
            set
            {
                isDirty = true;
                attachedPartType = value;
            }
        }

        #endregion Properties

        public AttachedPart()
        {
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public AttachedPart( BinaryReader br )
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
            detachedIndicator = br.ReadByte();
            partAttachedToID = br.ReadUInt16();
            aPPT = ( AttachedPartParameterType )br.ReadUInt32();
            attachedPartType.Decode( br );
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( ( byte )variableParameterType );
            bw.Write( detachedIndicator );
            bw.Write( ( ushort )partAttachedToID );
            bw.Write( ( uint )aPPT );
            attachedPartType.Encode( bw );
            isDirty = false;
        }

        /// <summary>
        /// Decoder used for VariableParameters with the type field of AttachedPart(0).
        /// </summary>
        /// <param name="type"></param>
        /// <param name="br"></param>
        public new static VariableParameter FactoryDecode( int type, BinaryReader br )
        {
            return new AttachedPart( br );                      
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format( "Attached Part( Detached Indicator-{0}, Part:-{1}, APPT-{2}, Type-{3})", detachedIndicator, partAttachedToID, aPPT, attachedPartType );
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( AttachedPart b )
        {
            if( variableParameterType != b.variableParameterType ) return false;
            if( detachedIndicator     != b.detachedIndicator     ) return false;
            if( partAttachedToID      != b.partAttachedToID      ) return false;
            if( aPPT                  != b.aPPT                  ) return false;
            if( attachedPartType      != b.attachedPartType      ) return false;            
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( AttachedPart a, AttachedPart b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}