using UnityEngine;
using System;
using System.Collections;
using DISUnity.DataType;
using System.IO;
using System.Text;
using DISUnity.DataType.Enums;

namespace DISUnity.PDU.SimulationManagement
{
    /// <summary>
    /// Used to communicate to a simulation entity that it is to leave a Stopped/ Frozen state 
    /// and begin participating in a simulation exercise.
    /// 
    /// Issuance of Stop / Freeze PDU:
    /// The Stop/Freeze PDU shall be issued by an SM to an entity when the SM requests an entity
    /// to stop simulating.
    /// </summary>
    /// <DIS_Version>5</DIS_Version>
    /// <size>40 bytes</size>
    [Serializable]
    public class StopFreeze : SimulationManagementHeader, IPduBodyDecoder
    {
        #region Properties

        #region Private

        [SerializeField]
        private ClockTime realWorldTime = new ClockTime();

        [SerializeField]
        private byte reason;

        [SerializeField]
        private byte frozenBehavior;

        [SerializeField]
        private int requestID;

        #endregion Private

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        /// <value>The length.</value>
        public override int Length
        {
            get
            {
                return 40;
            }
        }

        /// <summary>
        /// Specifies the real-world time at which the entity is to start/resume in the exercise.
        /// </summary>
        public ClockTime RealWorldTime
        {
            get
            {
                return realWorldTime;
            }
            set
            {
                isDirty = true;
                realWorldTime = value;
            }
        }

        /// <summary>
        /// Specifies the reason that an entity or exercise was stopped/frozen.
        /// </summary>
        /// <value>The sim time.</value>
        public byte Reason
        {
            get
            {
                return reason;
            }
            set
            {
                isDirty = true;
                reason = value;
            }
        }

        /// <summary>
        /// Specifies the internal behavior of the simulation and its appearance while frozen to the 
        /// other participants of the exercise.
        /// </summary>
        /// <value>The sim time.</value>
        public byte FrozenBehavior
        {
            get
            {
                return frozenBehavior;
            }
            set
            {
                isDirty = true;
                frozenBehavior = value;
            }
        }

        /// <summary>
        /// Identifies the specific and unique stop/freeze request being made by the SM.
        /// </summary>
        public uint RequestID
        {
            get
            {
                return ( uint )requestID;
            }
            set
            {
                isDirty = true;
                requestID = ( int )value;
            }
        }

        #endregion Properties

        public StopFreeze()
        {
            pDUType = PDUType.StopFreeze;
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public StopFreeze( Header h, BinaryReader br )
        {
            Decode( h, br );
        }

        #region DataTypeBase

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public override void Decode( Header h, BinaryReader br )
        {
            base.Decode( h, br );
            realWorldTime.Decode( br );
            reason = br.ReadByte();
            frozenBehavior = br.ReadByte();
            br.ReadUInt16();
            requestID = ( int )br.ReadUInt32();
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            base.Encode( bw );
            realWorldTime.Encode( bw );
            bw.Write( reason );
            bw.Write( frozenBehavior );
            bw.Write( ( ushort )0 ); // padding
            bw.Write( requestID );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.AppendLine( "Real world time: " + realWorldTime.ToString() );
            sb.AppendLine( "Reason: " + reason.ToString() );
            sb.AppendLine( "Frozen behavior: " + frozenBehavior.ToString() );
            sb.AppendLine( "Request ID: " + requestID.ToString() );
            return sb.ToString();
        }

        #endregion DataTypeBase

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( StopFreeze b )
        {
            if( !SimulationManagementHeader.Equals( this, b ) ) return false;	
            if( !realWorldTime.Equals( b.realWorldTime )      ) return false;
            if( reason != b.reason                            ) return false;
            if( frozenBehavior != b.frozenBehavior            ) return false;
            if( requestID != b.requestID                      ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( StartResume a, StartResume b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}