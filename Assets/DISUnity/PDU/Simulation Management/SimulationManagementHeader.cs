using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Text;
using DISUnity.DataType;
using DISUnity.DataType.Enums;

namespace DISUnity.PDU.SimulationManagement
{
    /// <summary>
    /// The first part of each Simulation Management PDU.
    /// </summary>
    /// <DIS_Version>TODO</DIS_Version>
    /// <size>24 bytes</size>
    [Serializable]
    public class SimulationManagementHeader : Header
    {
        //TODO [Tooltip( Tooltips.EntityID )]
        [SerializeField]
        protected EntityIdentifier originatingEntityID = new EntityIdentifier();

        //TODO [Tooltip( Tooltips.EntityID )]
        [SerializeField]
        protected EntityIdentifier receivingEntityID = new EntityIdentifier();

        /// <summary>
        /// Total size of header in bytes.
        /// </summary>        
        public override int Length
        {
            get
            {
                length = 24;
                return length;
            }
        }

        /// <summary>
        /// This field shall identify the entity issuing the Simulation Management PDU.
        /// </summary>
        public EntityIdentifier OriginatingEntityID
        {
            get
            {
                return originatingEntityID;
            }
            set
            {
                isDirty = true;
                originatingEntityID = value;
            }
        }

        /// <summary>
        /// This field shall identify the entity to which the Simulation Management PDU is intended.
        /// </summary>
        public EntityIdentifier ReceivingEntityID
        {
            get
            {
                return receivingEntityID;
            }
            set
            {
                isDirty = true;
                receivingEntityID = value;
            }
        }

        public SimulationManagementHeader()
        {
            protocolVersion = ProtocolVersion.IEEE_1278_1_1995;
            protocolFamily = ProtocolFamily.SimulationManagement;
        }

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public virtual void Decode( Header h, BinaryReader br )
        {
            Clone( h );
            originatingEntityID.Decode( br );
            receivingEntityID.Decode( br );
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            base.Encode( bw );
            originatingEntityID.Encode( bw );
            receivingEntityID.Encode( bw );
        }

        /// <summary>
        /// Returns string interpretation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.AppendLine( "Originating Entity Identifier: " + originatingEntityID.ToString() );
            sb.AppendLine( "Receiving Entity Identifier: " + receivingEntityID.ToString() );
            return sb.ToString();
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( SimulationManagementHeader b )
        {
            if( !Header.Equals( this, b )                    ) return false;        
            if( originatingEntityID != b.originatingEntityID ) return false;
            if( receivingEntityID != b.receivingEntityID     ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( SimulationManagementHeader a, SimulationManagementHeader b )
        {
            return a.Equals( b );
        }
    }

}