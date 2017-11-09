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
	/// The Data Query PDU shall be used by an SM to communicate a request for data from a simulated entity.	
	/// </summary>
	/// <DIS_Version>5</DIS_Version>
	/// <size>40 bytes + data query datum specification length</size>
	[Serializable]
	public class DataQuery : SimulationManagementHeader, IPduBodyDecoder
	{		
		#region Private
		
		[SerializeField]
		protected int requestID;

		[SerializeField]
		protected TimeStamp timeInterval;

		[SerializeField]
		protected DataQueryDatumSpecification dataQueryDatumSpecification = new DataQueryDatumSpecification();
		
		#endregion Private
		
		#region Properties
		
		/// <summary>
		/// Fixed & variable datum ids are stored here.
		/// </summary>
		/// <value>The datums.</value>
		public DataQueryDatumSpecification DataQueryDatums
		{
			get
			{
				return dataQueryDatumSpecification;
			}
			set
			{
				dataQueryDatumSpecification = value;
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
				length = 40 + dataQueryDatumSpecification.Length;
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
		/// Specifies the time interval between issues of Data PDUs. 
		/// A value of zero in this field shall mean that the requested data should be sent once 
		/// and not at any previously specified time interval.
		/// </summary>
		public TimeStamp TimeInterval
		{
			get
			{
				return timeInterval;
			}
			set
			{
				isDirty = true;
				timeInterval = value;
			}
		}
		
		#endregion Properties
		
		public DataQuery()
		{
			pDUType = PDUType.DataQuery;
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="br"></param>
		public DataQuery( Header h, BinaryReader br )
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
			timeInterval.Decode( br );
			dataQueryDatumSpecification.Decode( br );  
		}
		
		/// <summary>
		/// Encode data for network transmission.
		/// </summary>
		/// <param name="bw"></param>
		public override void Encode( BinaryWriter bw )
		{
			base.Encode( bw );
			bw.Write( requestID );
			timeInterval.Encode( bw );
			dataQueryDatumSpecification.Encode( bw );
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
			sb.AppendLine( "Time interval: " + timeInterval.ToString() );
			sb.Append( dataQueryDatumSpecification.ToString() );
			return sb.ToString();
		}
		
		#endregion DataTypeBase
		
		#region Operators
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="b"></param>        
		/// <returns></returns>
		public bool Equals( DataQuery b )
		{
			if( !SimulationManagementHeader.Equals( this, b )                ) return false;
			if( requestID != b.requestID                                     ) return false;
			if( timeInterval != b.timeInterval                               ) return false;
			if( dataQueryDatumSpecification != b.dataQueryDatumSpecification ) return false;
			return true;
		}
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Equals( DataQuery a, DataQuery b )
		{
			return a.Equals( b );
		}
		
		#endregion Operators
	}	
}