using UnityEngine;
using System.Collections;
using DISUnity.DataType;

namespace DISUnity.PDU.EntityInfoInteraction
{    
    [AddComponentMenu( "DISUnity/PDU/Entity Info Interaction/Collision" )]
    public class CollisionWrapper : MonoBehaviour
    {
        public Collision pdu = new Collision();
    }
}