using System;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using DISUnity.DataType;
using System.Text;
using System.Collections.ObjectModel;

namespace DISUnity.DataType
{
	/// <summary>
	/// Holds a collection of VariableDatum and derived classes.    
	/// </summary>
	[Serializable]
	public class VariableDatumCollection : DataTypeBaseSimple
	{
		#region Properties
		
		#region Private
		
		[SerializeField]
		protected List<VariableDatum> items = new List<VariableDatum>();
		
		// Add derived variable datum lists here: See VariableParameters for how its done.
		
		//[SerializeField]
		//protected List<MyDerivedVariableDatumClass> myDerivedVariableDatumClassList = new List<MyDerivedVariableDatumClass>();
		
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
				int length = 0;
				foreach ( var item in items )
				{
					length += item.Length;
				}
				return length;
			}
		}
		
		/// <summary>
		/// Contains all VariableDatum records combined into a single list.               
		/// </summary>                
		public ReadOnlyCollection<VariableDatum> Items
		{
			get
			{
				// Generates a combined list.
				List<VariableDatum> fds = new List<VariableDatum>();
				fds.AddRange( items );
				return fds.AsReadOnly();
			}
		}
		
		/// <summary>
		/// List of fixed datums. Derived versions are stored in separate lists so they can 
		/// be shown in the Unity inspector.
		/// If you do not need an item to be shown in the inspector then simply add them to this list.
		/// If you would like them to be shonw use the Items set function to have them filtered into 
		/// their correct lists or use the relveant list property.        
		/// </summary>
		public List<VariableDatum> VariableDatums
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
		
		public VariableDatumCollection()
		{
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="br"></param>
		public VariableDatumCollection( BinaryReader br )
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
		/// Add a collection of VariableDatums. This list will be filtered and broken into sub lists by type to allow the inspector to display the contents.
		/// </summary>
		/// <param name="items"></param>
		public void AddItems( List<VariableDatum> i )
		{
			isDirty = true;
			
			//VariableDatums.Clear();
			
			// TODO: No other types at the moment.
			items = i; 
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
				long pos = br.BaseStream.Position; // Save position for peek
				byte typ = br.ReadByte();
				br.BaseStream.Position = pos; // Reset 
				
				VariableDatum fd = VariableDatum.FactoryDecodeComplex( typ, br );
				
				// TODO: Split into sub lists by type when other types exist.
				items.Add( fd);            
			}
			
			isDirty = true;                       
		}
		
		public override void Decode( BinaryReader br )
		{
			throw new NotImplementedException( "Incorrect Decode function used. Use Decode( BinaryReader br, byte numberOfRecords )" );
		}
		
		public override void Encode( BinaryWriter bw )
		{
			items.ForEach( o => o.Encode( bw ) );            
		}
		
		/// <summary>
		/// Returns a string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine( "Variable Datums:" );
			items.ForEach( o => sb.Append( o.ToString() ) );         
			return sb.ToString();            
		}        
		
		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public bool Equals( VariableDatumCollection b )
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
		public static bool Equals( VariableDatumCollection a, VariableDatumCollection b )
		{
			return a.Equals( b );
		}
	}
}