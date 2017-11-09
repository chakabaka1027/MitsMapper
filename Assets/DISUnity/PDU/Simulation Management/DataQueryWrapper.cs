using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.SimulationManagement
{
	[AddComponentMenu( "DISUnity/PDU/Simulation Management/Data Query" )]
	public class DataQueryWrapper : MonoBehaviour
	{
		public DataQuery pdu = new DataQuery();
	}
}