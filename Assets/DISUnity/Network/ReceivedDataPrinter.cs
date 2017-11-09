using UnityEngine;
using System.Collections;
using DISUnity.PDU;

namespace DISUnity.Network
{
    /// <summary>
    /// Simple class that prints out every PDU received into the console, good for debugging.
    /// </summary>
    [AddComponentMenu( "DISUnity/Network/Received Data Printer" )]
    public class ReceivedDataPrinter : MonoBehaviour
    {
        /// <summary>
        /// Start listening
        /// </summary>
        public void OnEnable()
        {            
            DecodeFactory.Instance.anyReceived.AddListener( OnPDU );            
        }

        /// <summary>
        /// Stop listening
        /// </summary>
        public void OnDisable()
        {
            DecodeFactory.Instance.anyReceived.RemoveListener( OnPDU );            
        }

        /// <summary>
        /// On any PDU received.
        /// </summary>
        /// <param name="es"></param>
        private void OnPDU( Header h )
        {
            Debug.Log( h.ToString() );           
        }
    }
}