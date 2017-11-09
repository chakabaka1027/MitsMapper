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
	/// The Data PDU shall be used by an entity in response to a Data Query PDU or a Set Data PDU. 
	/// This PDU allows the entity to provide information requested in a Data Query PDU. 
	/// It also allows the entity to confirm the information received in a Set Data PDU.
	/// Data PDUs may be issued at a periodic rate. 
	/// This rate can be set in the Data Query PDU (see 4.5.5.4.8.1).
	/// Data PDUs may be issued by a simulation application without first receiving a Set Data or Data Query PDU. 
	/// The contents of the Data PDU in this case can be determined by the simulation application. 
	/// Error reporting shall be implemented in the Event Report PDU (see 4.5.5.4.11).
	/// </summary>
	/// <DIS_Version>5</DIS_Version>
	/// <size>40 bytes + size of fixed & variable datums</size>
	[Serializable]
    public class Data : SetData, IPduBodyDecoder
	{		
		public Data()
		{
			pDUType = PDUType.Data;
		}
		
		/// <summary>
		/// Create a new instance from binary data.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="br"></param>
		public Data( Header h, BinaryReader br )
		{
			Decode( h, br );
		}
	}	
}