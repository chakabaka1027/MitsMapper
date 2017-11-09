using System;
using System.IO;
using System.Runtime.InteropServices;
using DISUnity.DataType.Enums;
using UnityEngine;
using DISUnity.DataType;
using System.Text;

namespace DISUnity.DataType
{
    /// <summary>
    /// World coordinate system.     
    /// Using a right-handed, geocentric Cartesian coordinate system.
    /// The origin of the coordinate system is the centroid of the World Geodetic System 1984 (WGS 84) reference frame.
    /// Scale is 1 unit equals 1m.
    /// </summary>
    /// <size>24 bytes</size>
    [Serializable]
    public class WorldCoordinates : DataTypeBaseSimple
    {
        #region Properties

        #region Private

        [SerializeField]
        private double x;

        [SerializeField]
        private double y;

        [SerializeField]
        private double z;

        #endregion Private

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        public override int Length
        {
            get
            {
                return 24;
            }
        }

        /// <summary>
        /// X
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                isDirty = true;
                x = value;
            }
        }

        /// <summary>
        /// Y
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                isDirty = true;
                y = value;
            }
        }

        /// <summary>
        /// Z
        /// </summary>
        public double Z
        {
            get
            {
                return z;
            }
            set
            {
                isDirty = true;
                z = value;
            }
        }

        #endregion Properties

        public WorldCoordinates()
        {
        }

        public WorldCoordinates( double x, double y, double z )
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public WorldCoordinates( BinaryReader br )
        {
            Decode( br );
        }

        #region DataTypeBase

        /// <summary>
        /// Set x,y and z.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Set( double x, double y, double z )
        {
            this.x = x;
            this.y = y;
            this.z = z;
            isDirty = true;
        }

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        public override void Decode( BinaryReader br )
        {
            isDirty = true;
            
            x = br.ReadDouble();
            y = br.ReadDouble();
            z = br.ReadDouble();
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {            
            bw.Write( x );
            bw.Write( y );
            bw.Write( z );
            isDirty = false;            
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {            
            return string.Format( "{0},{1},{2}", x, y, z );
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( WorldCoordinates b )
        {
            if( x != b.x ) return false;
            if( y != b.y ) return false;
            if( z != b.z ) return false;            
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( WorldCoordinates a, WorldCoordinates b )
        {
            return a.Equals( b );            
        }

        /// <summary>
        /// Get/Set using index, if an invalid index is used then it will be ignored.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index]
        {
            get
            {
                switch( index )
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default:
                        Debug.LogError( "Invalid index. Should be between 0-2, not " + index + ". Returning 0." );
                        return 0;
                }                
            }
            set
            {
                switch( index )
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default:
                        Debug.LogError( "Invalid index. Should be between 0-2, not " + index + ". This will be ignored." );
                        break;
                }                
            }
        }

        #endregion Operators
    }
}

