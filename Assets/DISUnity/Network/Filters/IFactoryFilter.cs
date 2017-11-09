using DISUnity.PDU;
using UnityEngine;

namespace DISUnity.Network.Filters
{
    /// <summary>
    /// Interface for all factory filters.
    /// </summary>
    public abstract class IFactoryFilter : MonoBehaviour
    {
        /// <summary>
        /// Called just after the Header has been decoded but before the PDU decoding.
        /// Return true to allow the PDU to be decoded or false to halt any further processing of this PDU.
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public abstract bool OnHeaderDecoded( Header h );
    }
}