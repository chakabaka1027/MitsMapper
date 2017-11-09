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
    /// Stores absolute/ relative timestamps.
    /// Timestamps are used to reduce the error in a simulation.
    ///
    /// A lot of DIS simulators simply ignore timestamps however if error(I.E inaccuracy
    /// between a entities position and its actual position) is an issue then the timestamp
    /// can be used to reduce this error.
    ///
    /// Absolute timestamps tend to require the use of very accurate and expesnive clocks which are synchronized.
    /// Relative timestamps are less accurate and tend to use the computers internal clock.
    ///
    /// The following is taken straight from the DIS standard:
    /// "To make sure that relative timestamps are synchronized, you need to compare information about a received
    /// PDU that contains a relative timestamp with the time you are maintaining in your simulation application.
    /// This is done using software and without any special hardware. It does require that a few packets be observed
    /// before time is well synchronized. As packets are received, the difference between their relative timestamps
    /// and the receiver's clock is averaged. This average will correspond to the average latency, and the difference
    /// represents clock skew. After a few dozen packets, the difference between the observed average and the real
    /// average latency is around 5ms. After several hundred, the difference is in the 1ms neighborhood. This is not
    /// as long a time to wait as might be imagined. A dozen ES PDU's per minute are emitted by simulators for
    /// entities which are stopped. Thus, a few minutes of idle time before the exercise starts provides data for 5ms
    /// accuracy, and at 1-2 PDU's per second while moving, 1ms accuracy can be had in a matter of minutes.
    /// Exercises with stricter needs really should use absolute based time."
    /// </summary>
    /// <size>4 bytes</size>
    [Serializable] 
    public class TimeStamp : DataTypeBaseSimple
    {
        #region Properties

        #region Private

        [SerializeField]           
        private int allFields;

        #endregion Private

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        public override int Length
        {
            get
            {
                return 4;
            }
        }    

        /// <summary>
        /// The type of time stamp.
        /// </summary>
        public TimeStampType Type
        {
            get
            {
                return ( TimeStampType )( allFields & 1 );
            }
            set
            {                
                isDirty = true;
                allFields = ( int )value | ( allFields & -2 ); // Clear bit 0 and perform or with new value to set only bit 0          
            }
        }

        /// <summary>
        /// Time value. Scale of the time is determined by setting one hour equal to (2^31 - 1), 
        /// thereby resulting in each time unit representing 3600 s/( 2^31 - 1 ) = 1.676 micro secs
        /// or 0.000001676 seconds. This is the time for the current hour.
        /// </summary>
        public uint Time
        {
            get
            {
                return ( uint )( allFields >> 1 );
            }
            set
            {
                isDirty = true;

                // Bit 0 is used by the Type field so dont change it.
                // Shift value left by one bit and set bit 0 if it was set before.
                allFields = ( ( int )value << 1 ) | ( allFields & 1 );
            }
        }

        /// <summary>
        /// Type and Time in a single int.
        /// </summary>
        public int AllFields
        {
            get
            {
                return allFields;
            }
            set
            {
                isDirty = true;
                allFields = value;
            }
        }

        #endregion Properties

        public TimeStamp()
        {
        }

        public TimeStamp( TimeStampType type, uint time )
        {
            Type = type;
            Time = time;
        }

        public TimeStamp( int typeAndTime )
        {
            allFields = typeAndTime;
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public TimeStamp( BinaryReader br )
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
            allFields = ( int )br.ReadUInt32();            
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( ( uint )allFields );
            isDirty = false;
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format( "{0, -20} : {1, -30}\n", string.Format( "Time Stamp({0})", Type ), Time );
        }

        #endregion DataTypeBase

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( TimeStamp b )
        {
            if( allFields != b.allFields ) return false;            
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( TimeStamp a, TimeStamp b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}