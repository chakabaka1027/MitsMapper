using UnityEngine;
using System;
using DISUnity.DataType;
using DISUnity.Attributes;
using DISUnity.Resources;
using DISUnity.DataType.Enums;
using System.IO;
using System.Text;
using DISUnity.Simulation.Managers;

namespace DISUnity.PDU.Warfare
{
    /// <summary>
    /// Communicates the firing of a weapon or expendable.
    /// </summary>
    /// <DIS_Version>5, updated in 7</DIS_Version>
    /// <size>96 bytes</size>
    [Serializable]
    public class Fire : WarfareHeader, IPduBodyDecoder
    {
        #region Properties

        #region Private

        [Tooltip( Tooltips.FireMissionIndex )]
        [SerializeField]
        private int fireMissionIndex;

        [Tooltip( Tooltips.MunitionLocation )]
        [SerializeField]
        private WorldCoordinates locationInWorldCoordinates = new WorldCoordinates();

        [Tooltip( Tooltips.Descriptor )]
        [SerializeField]
        private Descriptor descriptor;

        [Tooltip( Tooltips.VelocityWarfare )]
        [SerializeField]
        private Vector3 velocity;

        [Tooltip( Tooltips.Range )]
        [SerializeField]
        private float range;

        #endregion Private

        /// <summary>
        /// Total size of PDU in bytes.
        /// </summary>        
        public override int Length
        {
            get
            {                
                return 36;
            }
        }

        #region PDU Status
        #if DIS_VERSION_7

        /// <summary>
        /// Indicates whether the data contained in this PDU is related to a     
        /// live, virtual or constructive entity. If the LVC designation is not 
        /// able to be determined, this field shall be set to No Statement (0).         
        /// </summary>
        /// <remarks>
        /// DIS 7 feature.
        /// Bits 1-2 of PDU status field.
        /// </remarks>
        public LiveVirtualConstructiveIndicator LVC
        {
            get
            {
                return ( LiveVirtualConstructiveIndicator )( ( pduStatus & 0x06 ) >> 3 );
            }
            set
            {
                isDirty = true;
                pduStatus = ( byte )( ( ( int )value << 1 ) | ( pduStatus & ~0x06 ) );
            }
        }

        /// <summary>
        /// Indicates whether the type of object fired was a Munition or an Expendable.
        /// Note: This field is set automatically so you should never really need to use it...
        /// </summary>
        /// DIS 7 feature.
        /// Bit 4 of PDU status field.
        /// </remarks>
        public FireTypeIndicator FTI
        {
            get
            {
                return ( FireTypeIndicator )( pduStatus & 0x10 );
            }
            set
            {
                isDirty = true;
                pduStatus = ( byte )( ( int )value == 1 ? ( pduStatus | 0x10 ) : ( pduStatus & ~0x10 ) );
            }
        }

        #endif
        #endregion PDU Status

        /// <summary>
        /// Identifies the fire mission. If unknown value will be symbolic value: NO_FIRE_MISSION.
        /// </summary>
        public int FireMissionIndex
        {
            get
            {
                return fireMissionIndex;
            }
            set
            {
                isDirty = true;
                fireMissionIndex = value;
            }
        }

        /// <summary>
        /// Location from which the munition was launched in world coordinates.
        /// </summary>
        public WorldCoordinates Location
        {
            get
            {
                return locationInWorldCoordinates;
            }
            set
            {
                isDirty = true;
                locationInWorldCoordinates = value;
            }
        }

        /// <summary>
        /// Describes the firing or launch of a munition or expendable.
        /// The following descriptors are available:
        /// DIS_VERSION_5 or DIS_VERSION_6 = MunitionDescriptor.
        /// DIS_VERSION_7 = MunitionDescriptor or ExpendableDescriptor.
        /// </summary>
        public Descriptor Descriptor
        {
            get
            {
                return descriptor;
            }
            set
            {
                isDirty = true;

                descriptor = value;
                
                #if DIS_VERSION_7

                // Set FTI
                if( descriptor is MunitionDescriptor )
                {
                    FTI = FireTypeIndicator.Munition;
                }
                else if( descriptor is ExpendableDescriptor )
                {
                    FTI = FireTypeIndicator.Expendable;
                }
                else
                {
                    Debug.LogWarning( "Invalid Descriptor Type, can not set FTI field." );
                }

                #endif
            }
        }

        /// <summary>
        /// The velocity of the fired munition at the point when the issuing simulation application intends the
        /// externally visible effects of the launch (e.g., exhaust plume or muzzle blast) to first become apparent. 
        /// Represented in world coordinates. Meters per second.
        /// </summary>
        public Vector3 Veclocity
        {
            get
            {
                return velocity;
            }
            set
            {
                isDirty = true;
                velocity = value;
            }
        }

        /// <summary>
        /// The range that an entity’s fire control system has assumed in computing the fire control solution. 0 if unknown.
        /// </summary>
        public float Range
        {
            get
            {
                return range;
            }
            set
            {
                isDirty = true;
                range = value;
            }
        }

        #endregion Properties

        public Fire()
        {
            pDUType = PDUType.Fire;

            #if DIS_VERSION_5 || DIS_VERSION_6
            descriptor = new MunitionDescriptor();
            #endif
        }

        /// <summary>
        /// Creates a new instance from a binary stream only decoding the body.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public Fire( Header h, BinaryReader br )
        {
            Decode( h, br );
        }

         /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public override void Decode( Header h, BinaryReader br )
        {
            base.Decode( h, br );
            fireMissionIndex = ( int )br.ReadUInt32();
            locationInWorldCoordinates.Decode( br );
         
            #if DIS_VERSION_7

            // Handle the descriptor based on the PDU protocol version.
            if( ProtocolVersion != ProtocolVersion.IEEE_1278_1_2012 )
            {
                if( descriptor == null || !( descriptor is MunitionDescriptor ) )
                {
                    descriptor = new MunitionDescriptor();
                }                          
            }
            else
            {
                // The FTI field indicates if the descriptor is a Munition or Ependable
                if( FTI == FireTypeIndicator.Munition )
                {
                    if( descriptor == null || !( descriptor is MunitionDescriptor ) )
                    {
                        descriptor = new  MunitionDescriptor();
                    }  
                }
                else
                {
                    if( descriptor == null || !( descriptor is ExpendableDescriptor ) )
                    {
                        descriptor = new ExpendableDescriptor();
                    }
                }              
            }

            #else 

            // Before DIS 7 the only descriptor was the BurstDescriptor aka MunitionDescriptor.
            if( descriptor == null )
            {
                descriptor = new MunitionDescriptor();
            }                             
           
            #endif
                        
            descriptor.Decode( br );
            velocity.Set( br.ReadSingle(), br.ReadSingle(), br.ReadSingle() );
            range = br.ReadSingle();
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            base.Encode( bw ); // Header
            bw.Write( fireMissionIndex );
            locationInWorldCoordinates.Encode( bw );

            // Default descriptor
            if( descriptor == null )
            {
                descriptor = new MunitionDescriptor();
            }            
            descriptor.Encode( bw );
            
            bw.Write( velocity.x );
            bw.Write( velocity.y );
            bw.Write( velocity.z );
            bw.Write( range );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.AppendLine( "Fire Mission Index: " + fireMissionIndex );
            sb.AppendLine( "Location: " + locationInWorldCoordinates.ToString() );
            if( descriptor != null )
            {
                sb.AppendLine( descriptor.ToString() );
            }
            sb.AppendLine( "Velcoity: " + velocity.ToString() );
            sb.AppendLine( "Range: " + range );
            return sb.ToString();
        }

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( Fire b )
        {
            if( !WarfareHeader.Equals( this, b )                                   ) return false; 
            if( fireMissionIndex != b.fireMissionIndex                             ) return false;
            if( !locationInWorldCoordinates.Equals( b.locationInWorldCoordinates ) ) return false;
            if( descriptor != null && !descriptor.Equals( b.descriptor )           ) return false;             
            if( !velocity.Equals( b.velocity )                                     ) return false;
            if( range != b.range                                                   ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( Fire a, Fire b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}