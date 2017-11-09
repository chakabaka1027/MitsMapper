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
	/// Arbitrary message (E.G char strings) shall be entered into the data stream by using a comment PDU.
    /// For use as comment, test message, error etc.
	/// </summary>
	/// <DIS_Version>5</DIS_Version>
    /// <size>32 bytes - not including fix and var datum sizes</size>
	[Serializable]
    public class Comment : SimulationManagementHeader, IPduBodyDecoder
    {		
		#region Private

		[SerializeField]
		protected DatumSpecification datumSpecification;

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
                length = 32 + datumSpecification.Length;
                return length;
			}
		}
				
		#endregion Properties

		public Comment()
		{
            pDUType = PDUType.Message;
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="br"></param>
        public Comment( Header h, BinaryReader br )
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
			datumSpecification.Decode( br );             
		}
		
		/// <summary>
		/// Encode data for network transmission.
		/// </summary>
		/// <param name="bw"></param>
		public override void Encode( BinaryWriter bw )
		{
			base.Encode( bw );
			datumSpecification.Encode( bw );		
		}
		
		/// <summary>
		/// Returns a string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

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
        public bool Equals( Comment b )
		{
            if( !SimulationManagementHeader.Equals( this, b ) ) return false;
			if( datumSpecification != b.datumSpecification    ) return false;
			return true;			
		}
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
        public static bool Equals( Comment a, Comment b )
		{
			return a.Equals( b );
		}
		
		#endregion Operators
	}	
}