using UnityEngine;
using System.Collections;
using DISUnity.DataType;

namespace DISUnity.PDU.RadioCommunications
{    
    [AddComponentMenu( "DISUnity/PDU/Radio Communications/Signal" )]
    public class SignalWrapper : MonoBehaviour
    {
        public Signal pdu = new Signal();
    }
}