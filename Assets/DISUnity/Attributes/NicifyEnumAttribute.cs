using UnityEngine;
using System.Collections;
using System;

namespace DISUnity.Attributes
{
    /// <summary>
    /// Nicifys enum names using ObjectNames.NicifyVariableName and shows an int field to allow an enum value to be typed directly.
    /// </summary>
    /// <example>
    /// "SomeEnum1" would become "Some Enum 1"
    /// </example>
    public class NicifyEnumAttribute : TooltipAttribute
    {
        public Type type;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="e">Enum type</param>
        /// <param name="name">Name in inspector</param>
        /// <param name="tooltip">Tooltip for label</param>
        public NicifyEnumAttribute( Type e, string name, string tooltip = "" )  :
            base( tooltip )
        {
            type = e;
        }
    }
}