using System;
using System.IO;
using System.Runtime.InteropServices;
using DISUnity.DataType.Enums;
using UnityEngine;
using DISUnity.DataType;
using System.Text;

namespace DISUnity.DataType
{
	[Serializable]
	public class ClockTime : DataTypeBaseSimple
    {
        #region Properties

		#region Private

		[SerializeField]
		private int hours;

		[SerializeField]
		private TimeStamp timePastHour = new TimeStamp();

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
				return 8;
			}
		}

		/// <summary>
		/// Specifies the hours since 0000 hours January 1, 1970 UTC.
		/// </summary>
		public int Hours
		{
			get
			{
				return hours;
			}
			set
			{
				isDirty = true;
				hours = value;
			}
		}

		/// <summary>
		/// This field shall specify the time past the hour indicated in the Hour field.
		/// See TimeStamp class.
		/// </summary>
		public TimeStamp TimePastHour
		{
			get
			{
				return timePastHour;
			}
			set
			{
				isDirty = true;
				timePastHour = value;
			}
		}

		#endregion Properties
	
		public ClockTime()
		{
		}

		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="br"></param>
		public ClockTime( BinaryReader br )
		{
			Decode( br );
		}

		#region DataTypeBase

		/// <summary>
		/// Decode network data.
		/// </summary>
		/// <param name="br"></param>
		public override void Decode (BinaryReader br)
		{
			isDirty = true;
			hours = br.ReadInt32();
			timePastHour.Decode(br);
		}

		/// <summary>
		/// Encode data for network transmission.
		/// </summary>
		/// <param name="bw"></param>
		public override void Encode (BinaryWriter bw)
		{
			bw.Write(hours);
			timePastHour.Encode(bw);
			isDirty = false;
		}

		/// <summary>
		/// Returns a string representation
		/// </summary>
		public override string ToString ()
		{
			return string.Format ("Hours={0}, TimePastHour={1}", Hours, TimePastHour);
		}

		#endregion DataTypeBase

		#region Operators

		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		public bool Equals( ClockTime b )
		{
			if ( hours != b.hours 					    ) return false;
			if ( !timePastHour.Equals( b.timePastHour ) ) return false;
			return true;
		}


		/// <summary>
		/// Compares internal data for equality.
		/// </summary>
		public static bool Equals( ClockTime a, ClockTime b )
		{
			return a.Equals( b );
		}

		#endregion Operators
	}
}