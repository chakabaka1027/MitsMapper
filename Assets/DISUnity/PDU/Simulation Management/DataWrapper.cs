using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.EntityInfoInteraction
{
	[AddComponentMenu( "DISUnity/PDU/Simulation Management/Data" )]
	public class DataWrapper : MonoBehaviour
	{
		public Data pdu = new Data();
	}
}