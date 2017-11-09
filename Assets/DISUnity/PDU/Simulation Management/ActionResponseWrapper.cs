using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.SimulationManagement
{
	[AddComponentMenu( "DISUnity/PDU/Simulation Management/Action Response" )]
	public class ActionResponseWrapper : MonoBehaviour
	{
		public ActionResponse pdu = new ActionResponse();
	}
}