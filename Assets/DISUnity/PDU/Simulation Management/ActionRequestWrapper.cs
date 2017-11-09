using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.SimulationManagement
{
	[AddComponentMenu( "DISUnity/PDU/Simulation Management/Action Request" )]
	public class ActionRequestWrapper : MonoBehaviour
	{
		public ActionRequest pdu = new ActionRequest();
	}
}