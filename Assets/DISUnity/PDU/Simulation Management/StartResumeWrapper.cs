using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.PDU.SimulationManagement;

namespace DISUnity.PDU.EntityInfoInteraction
{
    [AddComponentMenu( "DISUnity/PDU/Simulation Management/Start Resume" )]
    public class StartResumeWrapper : MonoBehaviour
    {
        public StartResume pdu = new StartResume();
    }
}