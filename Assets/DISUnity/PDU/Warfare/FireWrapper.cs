using UnityEngine;
using System.Collections;
using DISUnity.DataType;

namespace DISUnity.PDU.Warfare
{
    [AddComponentMenu( "DISUnity/PDU/Warfare/Fire" )]
    public class FireWrapper : MonoBehaviour
    {
        public Fire pdu = new Fire();
    }
}