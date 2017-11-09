using UnityEngine;
using System.Collections;
using System;

namespace DISUnity.Attributes
{
    /// <summary>
    /// Verifies if an IP address is a broadcast address,
    /// </summary>
    public class BroadcastAddressAttribute : TooltipAttribute
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="e">Enum type</param>
        /// <param name="name">Name in inspector</param>
        /// <param name="tooltip">Tooltip for label</param>
        public BroadcastAddressAttribute( string name, string tooltip = "" )  :
            base( tooltip )
        {
        }
    }
}