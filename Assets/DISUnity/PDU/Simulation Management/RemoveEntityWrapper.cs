using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.EntityInfoInteraction
{
    [AddComponentMenu( "DISUnity/PDU/Simulation Management/Remove Entity" )]
    public class RemoveEntityWrapper : MonoBehaviour
    {
        public RemoveEntity pdu = new RemoveEntity();
    }
}