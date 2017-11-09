using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.EntityInfoInteraction
{
    [AddComponentMenu( "DISUnity/PDU/Simulation Management/Acknowledge" )]
    public class AcknowledgeWrapper : MonoBehaviour
    {
        public Acknowledge pdu = new Acknowledge();
    }
}