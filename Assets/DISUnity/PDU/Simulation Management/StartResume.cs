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
	/// Issuance of Start / Resume PDU:
	/// The Start/Resume PDU shall be issued by an SM to an entity to instruct that entity to
	/// proceed from a Stopped/Frozen state to a Simulated state (see 4.5.5.5.3 for more on entity states).
	/// The Start/Resume PDU should be sent far enough in advance that any receiving simulation application 
	/// will have time to comply. The maximum expected time between transmission and reception of PDUs under 
	/// various conditions is described in IEEE Std 1278.2-1995. 
	/// An application should consider a Start/Resume PDU that arrives after the designated start time an 
	/// error condition that may be reported with an Event Report PDU.
	/// </summary>
	/// <DIS_Version>5</DIS_Version>
	/// <size>44 bytes</size>
	[Serializable]
    public class StartResume : SimulationManagementHeader, IPduBodyDecoder
    {
		#region Properties

		#region Private

		[SerializeField]
        private ClockTime realWorldTime = new ClockTime();

        [SerializeField]
        private ClockTime simTime = new ClockTime();

		[SerializeField]
		private int requestID;

		#endregion Private

		/// <summary>
		/// Size of this data type in bytes
		/// </summary>
		public override int Length 
		{
			get 
			{
				return 44;
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
		/// Specifies the simulation time at which the entity is to start/resume in the exercise.
		/// </summary>
		public ClockTime SimTime
		{
			get
			{
				return simTime;
			}
			set
			{
				isDirty = true;
				simTime = value;
			}
		}

		/// <summary>
		/// Identifies the specific and unique start/resume request being made by the SM.
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

		public StartResume()
		{
			pDUType = PDUType.StartResume;
		}

		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="br"></param>
		public StartResume( Header h, BinaryReader br )
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
			simTime.Decode( br );
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
			simTime.Encode( bw );
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
			sb.AppendLine( "Sim time: " + simTime.ToString() );
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
		public bool Equals( StartResume b )
		{
            if( !SimulationManagementHeader.Equals( this, b ) ) return false;		
			if( !realWorldTime.Equals( b.realWorldTime )      ) return false;
			if( !simTime.Equals( b.simTime )                  ) return false;
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