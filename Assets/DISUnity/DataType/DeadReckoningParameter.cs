using System;
using System.IO;
using System.Runtime.InteropServices;
using DISUnity.DataType.Enums;
using UnityEngine;
using DISUnity.DataType;
using System.Text;
using DISUnity.Attributes;
using DISUnity.Resources;

namespace DISUnity.DataType
{
    /// <summary>
    /// Parameters for dead reckoning the position and orientation of the entity.
    /// </summary>
    /// <size>40 bytes</size>
    [Serializable]
    public class DeadReckoningParameter : DataTypeBaseSimple
    {
        #region Properties

        #region Private

        [Tooltip( Tooltips.DeadReckoningAlgorithm )]
        [SerializeField]
        private DeadReckoningAlgorithm deadReckoningAlgorithm;
        
        [HideInInspector]
        [SerializeField]
        private byte[] otherParams = new byte[15];

        [SerializeField]
        private Vector3 linearAcceleration = Vector3.zero;

        [SerializeField]
        private Vector3 angularVelocity = Vector3.zero;

        #endregion Private

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        public override int Length
        {
            get
            {
                return 40;
            }
        }

        /// <summary>        
        /// Dead Reckoning Algorithms consist of 3 elements.
        /// DRM_X_Y_Z                                                           
        /// X - Specifies rotation as either fixed(F) or rotating(R).           
        /// Y - Specifies dead reckoning rates to be held constant as either rate of position (P) or rate of velocity (V).                   
        /// Z - Specifies the coordinate system as either world coordinates (W) or body axis coordinates(B).  
        /// </summary>
        public DeadReckoningAlgorithm Application
        {
            get
            {
                return deadReckoningAlgorithm;
            }
            set
            {
                isDirty = true;
                deadReckoningAlgorithm = value;
            }
        }

        /// <summary>
        /// This field shall specify other required dead reckoning parameters, see 1278.1-2012 for further info.
        /// This field must be exactky 15 bytes.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when the value is not 15 bytes in length.
        /// </exception>
        public byte[] OtherParameters
        {
            get
            {
                return otherParams;
            }
            set
            {
                if( value.Length != 15 )                
                    throw new Exception( "Other Parameters: Invalid Length. Length should be 15, it is " + value.Length.ToString() );                

                isDirty = true;
                otherParams = value;
            }
        }

        /// <summary>
        /// Linear acceleration represented as a vector with components in
        /// either world coordinate system or entity’s coordinate system depending on the value in the Dead
        /// Reckoning Algorithm field. Represented in meters per second squared.
        /// </summary>
        public Vector3 LinearAcceleration
        {
            get
            {
                return linearAcceleration;
            }
            set
            {
                isDirty = true;
                linearAcceleration = value;
            }
        }

        /// <summary>
        /// The rate at which an entity’s orientation is changing. 
        /// Measured in radians per second measured about each the entity’s own coordinate axes. 
        /// The positive direction of the angular velocity is defined by the right-hand rule.
        /// </summary>
        public Vector3 AngularVelocity
        {
            get
            {
                return angularVelocity;
            }
            set
            {
                isDirty = true;
                angularVelocity = value;
            }
        }

        #endregion Properties

        public DeadReckoningParameter()
        {
        }

        public DeadReckoningParameter( DeadReckoningAlgorithm alg, Vector3 linearAcceleration, Vector3 angularVelocity )
        {
            deadReckoningAlgorithm = alg;
            this.linearAcceleration = linearAcceleration;
            this.angularVelocity = angularVelocity;
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public DeadReckoningParameter( BinaryReader br )
        {
            Decode( br );
        }

        #region DataTypeBase

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        public override void Decode( BinaryReader br )
        {
            isDirty = true;
            deadReckoningAlgorithm = ( DeadReckoningAlgorithm )br.ReadByte();
            otherParams = br.ReadBytes( 15 );
            linearAcceleration.Set( br.ReadSingle(), br.ReadSingle(), br.ReadSingle() );
            angularVelocity.Set( br.ReadSingle(), br.ReadSingle(), br.ReadSingle() );        
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( ( byte )deadReckoningAlgorithm );
            bw.Write( otherParams );
            bw.Write( linearAcceleration.x );
            bw.Write( linearAcceleration.y );
            bw.Write( linearAcceleration.z );
            bw.Write( angularVelocity.x );
            bw.Write( angularVelocity.y );
            bw.Write( angularVelocity.z );
            isDirty = false;
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine( "Dead Reckoning Parameters:" );
            sb.AppendFormat( "\tAlgorithm: {0}\n", ( DeadReckoningAlgorithm )deadReckoningAlgorithm );
            sb.AppendFormat( "\tLinear Acceleration: {0}\n", linearAcceleration.ToString() ); 
            sb.AppendFormat( "\tAngular Velocity: {0}\n", angularVelocity.ToString() );
            return sb.ToString();
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( DeadReckoningParameter b )
        {
            if( deadReckoningAlgorithm != b.deadReckoningAlgorithm ) return false;
            if( !System.Linq.Enumerable.SequenceEqual( otherParams,b.otherParams )) return false;
            if( !linearAcceleration.Equals( b.linearAcceleration ) ) return false;
            if( !angularVelocity.Equals( b.angularVelocity )       ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( DeadReckoningParameter a, DeadReckoningParameter b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}