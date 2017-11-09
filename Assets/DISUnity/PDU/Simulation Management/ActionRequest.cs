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
	/// The Action Request PDU shall be used by the SM to request that a specific action be performed by a simulation entity. 
	/// Information required for the entity to perform the requested action shall also be included in this PDU.	
	/// </summary>
	/// <DIS_Version>5</DIS_Version>
	/// <size>40 bytes + size of fixed & variable datums</size>
	[Serializable]
	public class ActionRequest : SimulationManagementHeader, IPduBodyDecoder
	{		
		#region Private
		
		[SerializeField]
		protected int requestID;

		[SerializeField]
		protected ActionID actionID;
		
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
		public ActionID ActionID
		{
			get
			{
				return actionID;
			}
			set
			{
				isDirty = true;
				actionID = value;
			}
		}
		
		#endregion Properties
		
		public ActionRequest()
		{
			pDUType = PDUType.ActionRequest;
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="br"></param>
		public ActionRequest( Header h, BinaryReader br )
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
			actionID = ( ActionID ) br.ReadUInt32();
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
			bw.Write( ( uint ) actionID );
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
			sb.AppendLine( "Action ID: " + actionID.ToString() );
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
		public bool Equals( ActionRequest b )
		{
			if( !SimulationManagementHeader.Equals( this, b ) ) return false;
			if( requestID != b.requestID                      ) return false;
			if( actionID  != b.actionID                       ) return false;
			if( datumSpecification != b.datumSpecification    ) return false;
			return true;
		}
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Equals( ActionRequest a, ActionRequest b )
		{
			return a.Equals( b );
		}
		
		#endregion Operators
	}	
}