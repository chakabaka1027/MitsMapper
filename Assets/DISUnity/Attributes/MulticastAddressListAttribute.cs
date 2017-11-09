using UnityEngine;
using System.Collections;
using System;

namespace DISUnity.Attributes
{
    /// <summary>
    /// List of multicast addresses.
    /// </summary>
    public class MulticastAddressListAttribute : TooltipAttribute
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="e">Enum type</param>
        /// <param name="name">Name in inspector</param>
        /// <param name="tooltip">Tooltip for label</param>
        public MulticastAddressListAttribute( string name, string tooltip = "" )  :
            base( tooltip )
        {
        }
    }
}