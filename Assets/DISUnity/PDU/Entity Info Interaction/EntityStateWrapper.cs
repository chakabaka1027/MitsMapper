using UnityEngine;
using System.Collections;
using DISUnity.DataType;

namespace DISUnity.PDU.EntityInfoInteraction
{
    [AddComponentMenu( "DISUnity/PDU/Entity Info Interaction/Entity State" )]
    public class EntityStateWrapper : MonoBehaviour
    {
        public EntityState pdu = new EntityState();
    }
}