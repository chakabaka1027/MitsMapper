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
	/// The Remove Entity PDU shall communicate the removal of an entity from a DIS exercise. 
	/// This PDU indicates to the receiving entity that it is being removed from the exercise.
	/// NOTE—An entity may affect the removal of itself from an exercise by setting the State 
	/// bit in the Appearance field equal to Deactivated in the last Entity State PDU that it issues
	/// 
	/// Information contained in the Create Entity PDU:
	/// The Originating Entity ID shall identify the SM issuing the Remove Entity PDU. 
	/// The Receiving Entity ID shall identify the entity that is being removed from the simulation exercise.
	/// 
	/// Issuance of Create Entity PDU:
	/// The Remove Entity PDU shall be issued by an SM when a particular entity is to be removed from the simulation.
	/// </summary>
	/// <DIS_Version>5</DIS_Version>
	/// <size>28 bytes</size>
	[Serializable]
    public class RemoveEntity : SimulationManagementHeader, IPduBodyDecoder
    {		
		#region Properties
		
		#region Private
		
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
				return 28;
			}
		}
		
		/// <summary>
		/// Identifies the specific and unique entity removal request being made by the SM.
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

		public RemoveEntity()
		{
			pDUType = PDUType.RemoveEntity;
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="br"></param>
		public RemoveEntity( Header h, BinaryReader br )
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
			requestID = ( int )br.ReadUInt32();
		}
		
		/// <summary>
		/// Encode data for network transmission.
		/// </summary>
		/// <param name="bw"></param>
		public override void Encode( BinaryWriter bw )
		{
			base.Encode( bw );
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
		public bool Equals( RemoveEntity b )
		{
            if( !SimulationManagementHeader.Equals( this, b ) ) return false;			
			if( requestID != b.requestID                      ) return false;
			return true;
		}
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Equals( RemoveEntity a, RemoveEntity b )
		{
			return a.Equals( b );
		}
		
		#endregion Operators
	}	
}