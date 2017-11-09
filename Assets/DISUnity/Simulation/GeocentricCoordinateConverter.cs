using UnityEngine;
using System.Collections;

namespace DISUnity.Simulation
{
    public class GeocentricCoordinateConverter : WorldCoordinateConverter
    {       
        /// <summary>
        /// Applies world scale to remote coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="local"></param>
        public override void RemoteToLocal( double x, double y, double z, out Vector3 local )
        {
            local = new Vector3( ( float )( x * WorldScale.x ), ( float )( y * WorldScale.y ),( float )( z * WorldScale.z ) );
        }

        /// <summary>
        /// Applies world scale to local coordinates.
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        public override void LocalToRemote( Vector3 local, out double[] remote )
        {            
            remote = new double[] { local.x / WorldScale.x, local.y / WorldScale.y, WorldScale.z / local.z };
        }
    }
}