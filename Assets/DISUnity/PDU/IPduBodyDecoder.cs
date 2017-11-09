using UnityEngine;
using System.Collections;
using System.IO;
using DISUnity.PDU;

namespace DISUnity.PDU
{
    /// <summary>
    /// Interface to implment decoding a PDU with a pre decoded header.
    /// </summary>
    public interface IPduBodyDecoder
    {
        /// <summary>
        /// Performs a decode after the header values.        
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        void Decode( Header h, BinaryReader br );
    }
}