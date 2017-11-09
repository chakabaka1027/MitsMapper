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
    /// The Articulated Part VP record is used to represent the state of the movable parts of an entity. 
    /// Examples of movable parts include the turret on a tank and the periscope on a submarine.
    /// An Articulated Part VP record shall represent the value of only one parameter of a movable, 
    /// or articulated, part. Thus, it may require multiple Articulated Part VP records to describe  
    /// the state of a single articulated part. 
    /// </summary>
    [Serializable]
    public class ArticulatedPart : VariableParameter
    {
        #region Properties
        
        #region Private

        [SerializeField]
        private byte changeIndicator;

        [SerializeField]
        [Range( ushort.MinValue, ushort.MaxValue )]
        private int partID;

        [Tooltip( Tooltips.ArticulatedPartsMetric )]
        [SerializeField]
        private ArticulatedPartsMetric paramMetric = ArticulatedPartsMetric.Position; // Default

        [Tooltip( Tooltips.ArticulatedPartsClass )]
        [SerializeField]
        private ArticulatedPartsClass paramClass = ArticulatedPartsClass.Rudder; // Default

        [SerializeField]
        private float paramValue;
        
        #endregion Private

        /// <summary>
        /// The type/s this class supports. 
        /// </summary>
        public new static int[] ConcreteTypeEnums
        {
            get
            {
                return new int[]{ ( int )VariableParameterType.ArticulatedPart };
            }
        }

        /// <summary>
        /// Stores change to any articulated part. Set to 0 at sim start and incremented
        /// each time a change occurs, if value is greater than 255 it should be set back to 0.
        /// </summary>
        public byte ChangeIndicator
        {
            get
            {
                return changeIndicator;
            }
            set
            {
                isDirty = true;
                changeIndicator = value;
            }
        }

        /// <summary>
        /// The identification of the articulated part to which this articulation parameter is attached. 
        /// </summary>
        public ushort PartID
        {
            get
            {
                return ( ushort )partID;
            }
            set
            {
                isDirty = true;
                partID = value;
            }
        }

        /// <summary>
        /// Identifies a particular articulated part on a given entity type.
        /// </summary>
        public ArticulatedPartsClass Class
        {
            get
            {
                return paramClass;
            }
            set
            {
                isDirty = true;
                paramClass = value;
            }
        }

        /// <summary>
        /// Identifies the transformation to be applied to the articulated part.
        /// </summary>
        public ArticulatedPartsMetric Metric
        {
            get
            {
                return paramMetric;
            }
            set
            {
                isDirty = true;
                paramMetric = value;  
            }
        }

        /// <summary>
        /// The value for the articulated part.
        /// </summary>
        public float Value
        {
            get
            {
                return Value;
            }
            set
            {
                isDirty = true;
                paramValue = value;
            }
        }

        #endregion Properties

        public ArticulatedPart()
        {
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public ArticulatedPart( BinaryReader br )
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
            changeIndicator = br.ReadByte();
            partID = br.ReadUInt16();
            
            int typeVariant = br.ReadInt32();
            paramMetric = ( ArticulatedPartsMetric )( typeVariant % 32 );
            paramClass = ( ArticulatedPartsClass )( typeVariant - ( int )paramMetric );

            paramValue = br.ReadSingle();
            br.BaseStream.Seek( 1, SeekOrigin.Current ); // Skip padding
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( ( byte )variableParameterType );
            bw.Write( changeIndicator );
            bw.Write( ( ushort )partID );
            bw.Write( ( int )paramClass + ( int )paramMetric );
            bw.Write( paramValue );            
            bw.Write( ( byte )0 ); // Padding
            isDirty = false;
        }

        /// <summary>
        /// Decoder used for VariableParameters with the type field of ArticulatedPart(0).
        /// </summary>
        /// <param name="type"></param>
        /// <param name="br"></param>
        public new static VariableParameter FactoryDecode( int type, BinaryReader br )
        {
            return new ArticulatedPart( br );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format( "Articulated Part({0}: Change {1}: {2}({3} = {4}", partID, changeIndicator, Class, Metric, paramValue );
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( ArticulatedPart b )
        {
            if( variableParameterType != b.variableParameterType ) return false;
            if( changeIndicator       != b.changeIndicator       ) return false;
            if( partID                != b.partID                ) return false;
            if( paramMetric           != b.paramMetric           ) return false;
            if( paramClass            != b.paramClass            ) return false;
            if( paramValue            != b.paramValue            ) return false;           
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( ArticulatedPart a, ArticulatedPart b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}