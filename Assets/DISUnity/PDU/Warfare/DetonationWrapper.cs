using UnityEngine;
using System.Collections;
using DISUnity.DataType;

namespace DISUnity.PDU.Warfare
{
    [AddComponentMenu( "DISUnity/PDU/Warfare/Detonation" )]
    public class DetonationWrapper : MonoBehaviour
    {
        public Detonation pdu = new Detonation();
    }
}