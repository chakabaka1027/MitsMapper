using UnityEngine;
using System.Collections;

namespace DISUnity.Simulation
{
    /// <summary>
    /// Converts between remote(DIS) and local(Unity) coordinate systems.
    /// </summary>
    public abstract class WorldCoordinateConverter : MonoBehaviour
    {
        #region Properties

        #region Private

        [SerializeField]
        private Vector3 worldScale = Vector3.one;

        #endregion

        /// <summary>
        /// Scale, 1 = 1m
        /// </summary>
        public Vector3 WorldScale
        {
            get
            {
                return worldScale;
            }
            set
            {
                worldScale = value;
            }
        }

        #endregion

        /// <summary>
        /// Convert from DIS coordinates to local.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public virtual Vector3 RemoteToLocal( double x, double y, double z )
        {
            Vector3 v = Vector3.zero;
            RemoteToLocal( x, z, y, out v ); // Z is up in DIS
            return v;
        }

        /// <summary>
        /// Convert from DIS coordinates to local.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public abstract void RemoteToLocal( double x, double y, double z, out Vector3 local );

        /// <summary>
        /// Convert from local coordinates to DIS.
        /// </summary>
        /// <param name="local"></param>
        /// <returns></returns>
        public virtual double[] LocalToRemote( Vector3 local )
        {
            double[] xyz = new double[3];
            LocalToRemote( local, out xyz );
            return xyz;
        }

        /// <summary>
        /// Convert from local coordinates to DIS.
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        public abstract void LocalToRemote( Vector3 local, out double[] remote );
    }
}