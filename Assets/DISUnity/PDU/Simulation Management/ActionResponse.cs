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
	/// The Action Response PDU shall be used by an entity to acknowledge the receipt of an Action Request PDU. 
	/// This PDU shall provide information on the status of the request and may also be used to provide 
	/// additional information depending upon the type of action requested.
	/// </summary>
	/// <DIS_Version>5</DIS_Version>
	/// <size>40 bytes + size of fixed & variable datums</size>
	[Serializable]
	public class ActionResponse : SimulationManagementHeader, IPduBodyDecoder
	{		
		#region Private
		
		[SerializeField]
		protected int requestID;
		
		[SerializeField]
		protected RequestStatus requestStatus;
		
		[SerializeField]
		protected DatumSpecification datumSpecification = new DatumSpecification();
		
		#endregion Private
		
		#region Properties
		
		/// <summary>
		/// Fixed & variable datums are stored here.
		/// </summary>
		/// <value>The datums.</value>
		public DatumSpecification Datums
		{
			get
			{
				return datumSpecification;
			}
			set
			{
				datumSpecification = value;
				isDirty = true;
			}
		}
		
		/// <summary>
		/// Size of this data type in bytes
		/// </summary>
		/// <returns></returns>
		/// <value>The length.</value>
		public override int Length 
		{
			get 
			{
				length = 40 + datumSpecification.Length;
				return length;
			}
		}
		
		/// <summary>
		/// Identifies the set data request being made by the SM.
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
		
		/// <summary>
		/// Specifies the particular action that is requested by the SM.
		/// </summary>
		public RequestStatus RequestStatus
		{
			get
			{
				return requestStatus;
			}
			set
			{
				isDirty = true;
				requestStatus = value;
			}
		}
		
		#endregion Properties
		
		public ActionResponse()
		{
			pDUType = PDUType.ActionResponse;
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="br"></param>
		public ActionResponse( Header h, BinaryReader br )
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
			requestStatus = ( RequestStatus ) br.ReadUInt32();
			datumSpecification.Decode( br );  
		}
		
		/// <summary>
		/// Encode data for network transmission.
		/// </summary>
		/// <param name="bw"></param>
		public override void Encode( BinaryWriter bw )
		{
			base.Encode( bw );
			bw.Write( requestID );
			bw.Write( (uint) requestStatus );
			datumSpecification.Encode( bw );
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
			sb.AppendLine( "Request Status: " + requestStatus.ToString() );
			sb.Append( datumSpecification.ToString() );
			return sb.ToString();
		}
		
		#endregion DataTypeBase
		
		#region Operators
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="b"></param>        
		/// <returns></returns>
		public bool Equals( ActionResponse b )
		{
			if( !SimulationManagementHeader.Equals( this, b ) ) return false;
			if( requestID != b.requestID                      ) return false;
			if( requestStatus != b.requestStatus              ) return false;
			if( datumSpecification != b.datumSpecification    ) return false;
			return true;
		}
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Equals( ActionResponse a, ActionResponse b )
		{
			return a.Equals( b );
		}
		
		#endregion Operators
	}	
}