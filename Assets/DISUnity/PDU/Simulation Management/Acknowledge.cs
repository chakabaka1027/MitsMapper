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
	/// Used to acknowledge the receipt of a Create Entity PDU, a Remove Entity PDU, a Start/Resume PDU,
	/// or a Stop/Freeze PDU. This PDU verifies to the SM the receipt of the issued PDU. The Acknowledge PDU
	/// shall be sent as soon as possible after the receipt of the above PDUs.
	/// 
	/// Issuance of Acknowledge PDU:
	/// The Acknowledge PDU shall be issued by a simulation application in response to a Create Entity PDU, 
	/// a Remove Entity PDU, a Start/Resume PDU, or a Stop/Freeze PDU
	/// </summary>
	/// <DIS_Version>5</DIS_Version>
	/// <size>32 bytes</size>
	[Serializable]
    public class Acknowledge : SimulationManagementHeader, IPduBodyDecoder
    {		
		#region Properties
		
		#region Private
		
		[SerializeField]
        private AcknowledgeFlag acknowledgeFlag;
		
		[SerializeField]
        private AcknowledgeResponseFlag responseFlag;

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
				return 32;
			}
		}
		
		/// <summary>
		/// Indicates what type of message has been acknowledged.
		/// </summary>
        public AcknowledgeFlag AcknowledgeFlag
		{
			get
			{
				return acknowledgeFlag;
			}
			set
			{
				isDirty = true;
				acknowledgeFlag = value;
			}
		}
		
		/// <summary>
		/// Indicates whether or not the receiving entity was able to comply with the request, 
		/// and for what reason.
		/// </summary>
        public AcknowledgeResponseFlag ResponseFlag
		{
			get
			{
				return responseFlag;
			}
			set
			{
				isDirty = true;
				responseFlag = value;
			}
		}
		
		/// <summary>
		/// Identifies the matching response to the specific a Start/Resume, Stop/ Freeze, 
		/// Create Entity, or Remove Entity PDU sent by the SM.
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

		public Acknowledge()
		{
			pDUType = PDUType.Acknowledge;
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="br"></param>
		public Acknowledge( Header h, BinaryReader br )
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
			acknowledgeFlag = ( AcknowledgeFlag )br.ReadUInt16();
			responseFlag = ( AcknowledgeResponseFlag )br.ReadUInt16();
			requestID = ( int )br.ReadUInt32();
		}
		
		/// <summary>
		/// Encode data for network transmission.
		/// </summary>
		/// <param name="bw"></param>
		public override void Encode( BinaryWriter bw )
		{
			base.Encode( bw );
			bw.Write( ( ushort )acknowledgeFlag );
			bw.Write( ( ushort )responseFlag );
			bw.Write( ( uint )requestID );
		}
		
		/// <summary>
		/// Returns a string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( base.ToString() );
			sb.AppendLine( "Acknowledge flag: " + acknowledgeFlag.ToString() );
			sb.AppendLine( "Response flag: " + responseFlag.ToString() );
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
		public bool Equals( Acknowledge b )
		{
            if( !SimulationManagementHeader.Equals( this, b ) ) return false;			
			if( acknowledgeFlag != b.acknowledgeFlag          ) return false;
			if( responseFlag != b.responseFlag                ) return false;
			if( requestID != b.requestID                      ) return false;
			return true;
		}
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Equals( Acknowledge a, Acknowledge b )
		{
			return a.Equals( b );
		}
		
		#endregion Operators
	}	
}