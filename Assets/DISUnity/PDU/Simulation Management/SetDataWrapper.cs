using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.EntityInfoInteraction
{
    [AddComponentMenu( "DISUnity/PDU/Simulation Management/Set Data" )]
    public class SetDataWrapper : MonoBehaviour
    {
        public SetData pdu = new SetData();
    }
}