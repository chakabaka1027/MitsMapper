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
	/// The Create Entity PDU shall communicate information about the creation of a new entity for a 
	/// DIS exercise. This PDU simply establishes the identity of the new entity.
	/// 
	/// Information contained in the Create Entity PDU:
	/// The Originating Entity ID shall represent the identification number for the SM that is responsible 
	/// for creating the new entity. The Receiving Entity ID shall represent the entity identification 
	/// number of the newly created entity if it is known. Identification numbers shall be unique to each entity.
	/// The SM may request that the application creating the entity assign the entity number. This is 
	/// accomplished by setting the Entity Identifier field to RQST_ASSIGN_ID
	/// 
	/// Issuance of Create Entity PDU:
	/// The Create Entity PDU may be issued by a SM to a simulation application when a new entity is to be created
	/// for a DIS exercise. This PDU is not necessary for the creation of munition entities that require tracking 
	/// information. Entities may enter an exercise without the use of the Simulation Management protocol.
	/// </summary>
	/// <DIS_Version>5</DIS_Version>
	/// <size>28 bytes</size>
	[Serializable]
    public class CreateEntity : SimulationManagementHeader, IPduBodyDecoder
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
		/// Identifies the entity creation request being made by the SM.
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

		public CreateEntity()
		{
			pDUType = PDUType.CreateEntity;
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="br"></param>
		public CreateEntity( Header h, BinaryReader br )
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
		public bool Equals( CreateEntity b )
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
		public static bool Equals( CreateEntity a, CreateEntity b )
		{
			return a.Equals( b );
		}
		
		#endregion Operators
	}	
}