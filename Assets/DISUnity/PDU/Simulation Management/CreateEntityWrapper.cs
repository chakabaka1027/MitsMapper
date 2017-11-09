using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.EntityInfoInteraction
{
    [AddComponentMenu( "DISUnity/PDU/Simulation Management/Create Entity" )]
    public class CreateEntityWrapper : MonoBehaviour
    {
        public CreateEntity pdu = new CreateEntity();
    }
}