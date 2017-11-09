using System;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using DISUnity.DataType;
using DISUnity.DataType.Enums;
using System.Text;
using System.Collections.ObjectModel;

namespace DISUnity.DataType
{
	/// <summary>
	/// Holds a collection of DatumID.    
	/// </summary>
	[Serializable]
	public class DatumIDCollection : DataTypeBaseSimple
	{
		#region Properties
		
		#region Private
		
		[SerializeField]
		protected List<DatumID> items = new List<DatumID>();
		
		#endregion Private
		
		/// <summary>
		/// Total size of the collection in bytes.
		/// </summary>
		/// <returns></returns>
		/// <value>The length.</value>
		public override int Length
		{
			get
			{
				return ( int )NumberOfRecords * 4; // 4 bytes is fixed length of a encoded DatumID data type.
			}
		}
		
		/// <summary>
		/// Contains all DatumID records combined into a single list.               
		/// </summary>                
		public ReadOnlyCollection<DatumID> Items
		{
			get
			{
				// Generates a combined list.
				List<DatumID> dis = new List<DatumID>();
				dis.AddRange( items );
				return dis.AsReadOnly();
			}
		}
		
		/// <summary>
		/// List of DatumIDs.    
		/// </summary>
		public List<DatumID> DatumIDs
		{
			get
			{
				return items;
			}
			set
			{
				isDirty = true;
				items = value;
			}
		}
		
		/// <summary>
		/// Total number of fixed datum records.
		/// </summary>
		public uint NumberOfRecords
		{
			get
			{
				return ( uint )items.Count;
			}
		}
		
		#endregion Properties
		
		public DatumIDCollection()
		{
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="br"></param>
		public DatumIDCollection( BinaryReader br )
		{
			Decode( br );
		}
		
		/// <summary>
		/// Clears all lists.
		/// </summary>
		public void Clear()
		{
			items.Clear();
		}
		
		/// <summary>
		/// Add a collection of DatumIDs.
		/// </summary>
		/// <param name="items"></param>
		public void AddItems( List<DatumID> i )
		{
			isDirty = true;
			items.AddRange(i); 
		}
		
		/// <summary>
		/// Decode network data.
		/// </summary>
		/// <param name="br"></param>
		/// <param name="numberOfRecords">Number of records to decode from the stream.</param>
		public virtual void Decode( BinaryReader br, uint numberOfRecords )
		{
			Clear();
			for( uint i = 0; i < numberOfRecords; ++i )
			{
				DatumID di = ( DatumID ) br.ReadUInt32();
				items.Add(di);            
			}
			isDirty = true;                       
		}
		
		public override void Decode( BinaryReader br )
		{
			throw new NotImplementedException( "Incorrect Decode function used. Use Decode( BinaryReader br, byte numberOfRecords )" );
		}
		
		public override void Encode( BinaryWriter bw )
		{
			items.ForEach( o => bw.Write( ( uint ) o ));            
		}
		
		/// <summary>
		/// Returns a string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine( "Datum IDs:" );
			items.ForEach( o => sb.Append( o.ToString() ) );         
			return sb.ToString();            
		}        
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public bool Equals( DatumIDCollection b )
		{
			if( !items.Equals( b.items ) ) return false;             
			return true;
		}
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Equals( DatumIDCollection a, DatumIDCollection b )
		{
			return a.Equals( b );
		}
	}
}