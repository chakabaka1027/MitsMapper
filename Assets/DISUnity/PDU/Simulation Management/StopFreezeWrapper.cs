using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.EntityInfoInteraction
{
    [AddComponentMenu( "DISUnity/PDU/Simulation Management/Stop Freeze" )]
    public class StopFreezeWrapper : MonoBehaviour
    {
        public StopFreeze pdu = new StopFreeze();
    }
}