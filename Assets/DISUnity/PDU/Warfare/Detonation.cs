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
    /// Communicates detonation or impact of munitions.
    /// </summary>
    /// <DIS_Version>5, updated in 7</DIS_Version>
    /// <size>104 bytes + 128/16 * Num Art Params</size>
    [Serializable]
    public class Detonation : WarfareHeader, IPduBodyDecoder
    {
        #region Properties

        #region Private

        [Tooltip( Tooltips.VelocityWarfare )]
        [SerializeField]
        private Vector3 velocity;

        [Tooltip( Tooltips.MunitionLocation )]
        [SerializeField]
        private WorldCoordinates locationInWorldCoordinates = new WorldCoordinates();

        [Tooltip( Tooltips.Descriptor )]
        [SerializeField]
        private Descriptor descriptor;

        [Tooltip( Tooltips.VelocityWarfare )]
        [SerializeField]
        private Vector3 locationInEntityCoordinates;

        [SerializeField]
        private DetonationResult detonationResult;

        [SerializeField]
        public VariableParameterCollection variableParameters = new VariableParameterCollection();

        #endregion Private

        /// <summary>
        /// Total size of PDU in bytes.
        /// </summary>        
        public override int Length
        {
            get
            {
                length = 104 + variableParameters.Length;
                return length;
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
                return ( LiveVirtualConstructiveIndicator )( ( pduStatus & 0x06 ) >> 1 );
            }
            set
            {
                isDirty = true;
                pduStatus = ( byte )( ( ( int )value << 1 ) | ( pduStatus & ~0x06 ) );
            }
        }

        /// <summary>
        /// Indicates whether the type of object fired was a Munition, Expendable or a Non-Munition Explosion.
        /// Note: This field is set automatically so you should never really need to use it...
        /// </summary>
        /// DIS 7 feature.
        /// Bits 4-5 of PDU status field.
        /// </remarks>
        public DetonationTypeIndicator DTI
        {
            get
            {
                return ( DetonationTypeIndicator )( ( pduStatus & 0x30 ) >> 4 );
            }
            set
            {
                isDirty = true;
                pduStatus = ( byte )( ( ( int )value << 4 ) | ( pduStatus & ~0x30 ) );
            }
        }

        #endif
        #endregion PDU Status

        /// <summary>
        /// The velocity of the munition immediately before detonation/impact, the velocity of 
        /// a non-munition entity immediately before exploding, or the velocity of an expendable
        /// immediately before a chaff burst or ignition of a flare.
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
        /// Location of the detonation in world coordinates.
        /// </summary>
        public WorldCoordinates LocationInWorldCoordinates
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
        /// DIS_VERSION_7 = MunitionDescriptor, ExpendableDescriptor or ExplosionDescriptor.
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

                // Set DTI
                if( descriptor is MunitionDescriptor )
                {
                    DTI = DetonationTypeIndicator.Munition;
                }
                else if( descriptor is ExpendableDescriptor )
                {
                    DTI = DetonationTypeIndicator.Expendable;
                }
                else if( descriptor is ExplosionDescriptor )
                {
                    DTI = DetonationTypeIndicator.NonMunitionExplosion;
                }
                else
                {
                    Debug.LogWarning( "Invalid Descriptor Type, can not set DTI field." );
                }

                #endif
            }
        }

        /// <summary>
        /// Location of detonation event in entity coordinates. Use for damage assessment to the entity.
        /// </summary>
        public Vector3 LocationInEntityCoordinates
        {
            get
            {
                return locationInEntityCoordinates;
            }
            set
            {
                isDirty = true;
                locationInEntityCoordinates = value;
            }
        }

        /// <summary>
        /// The result of the detonation.
        /// </summary>
        public DetonationResult DetonationResult
        {
            get
            {
                return detonationResult;
            }
            set
            {
                isDirty = true;
                detonationResult = value;
            }
        }

        /// <summary>
        /// Information associated with an entity or detonation, not otherwise accounted for in the PDU.
        /// </summary>
        public VariableParameterCollection VariableParameters
        {
            get
            {
                return variableParameters;
            }
            set
            {
                isDirty = true;
                variableParameters = value;
            }
        }

        #endregion Properties

        public Detonation()
        {
            pDUType = PDUType.Detonation;
        }

        /// <summary>
        /// Creates a new instance from a binary stream only decoding the body.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public Detonation( Header h, BinaryReader br )
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
            velocity.Set( br.ReadSingle(), br.ReadSingle(), br.ReadSingle() );
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
                // The DTI field indicates if the descriptor is a Munition, Ependable or Explosion.
                if( DTI == DetonationTypeIndicator.Munition )
                {
                    if( descriptor == null || !( descriptor is MunitionDescriptor ) )
                    {
                        descriptor = new MunitionDescriptor();
                    }              
                }
                if( DTI == DetonationTypeIndicator.Expendable )
                {
                    if( descriptor == null || !( descriptor is ExpendableDescriptor ) )
                    {
                        descriptor = new ExpendableDescriptor();
                    }
                }
                else
                {
                    if( descriptor == null || !( descriptor is ExplosionDescriptor ) )
                    {
                        descriptor = new ExplosionDescriptor();
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
            locationInEntityCoordinates.Set( br.ReadSingle(), br.ReadSingle(), br.ReadSingle() );
            detonationResult = ( DetonationResult )br.ReadByte();
            byte numOfArticulationParams = br.ReadByte();
            br.BaseStream.Seek( 2, SeekOrigin.Current ); // Skip padding
            variableParameters.Decode( br, numOfArticulationParams ); 
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            base.Encode( bw ); // Header
            bw.Write( velocity.x );
            bw.Write( velocity.y );
            bw.Write( velocity.z );
            locationInWorldCoordinates.Encode( bw );
            
            // Default descriptor
            if( descriptor == null )            
                descriptor = new MunitionDescriptor();            

            descriptor.Encode( bw );

            bw.Write( locationInEntityCoordinates.x );
            bw.Write( locationInEntityCoordinates.y );
            bw.Write( locationInEntityCoordinates.z );            
            bw.Write( ( byte )detonationResult );
            
            bw.Write( ( byte )variableParameters.NumberOfRecords );

            bw.Write( ( ushort )0 ); // Padding
            variableParameters.Encode( bw );   
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );            
            sb.AppendLine( "Velcoity: " + velocity.ToString() );
            sb.AppendLine( "Location: " + locationInWorldCoordinates.ToString() );
            if( descriptor != null )           
                sb.AppendLine( descriptor.ToString() );            
            sb.AppendLine( "Entity Location: " + locationInEntityCoordinates.ToString() );
            sb.AppendLine( "Detonation Result: " + detonationResult );
            sb.AppendLine( variableParameters.ToString() );
            return sb.ToString();
        }

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( Detonation b )
        {
            if( !WarfareHeader.Equals( this, b )                                     ) return false; 
            if( !velocity.Equals( b.velocity )                                       ) return false;
            if( !locationInWorldCoordinates.Equals( b.locationInWorldCoordinates )   ) return false;
            if( descriptor != null && !descriptor.Equals( b.descriptor )             ) return false;
            if( !locationInEntityCoordinates.Equals( b.locationInEntityCoordinates ) ) return false;
            if( !detonationResult.Equals( b.detonationResult )                       ) return false;
            if( !variableParameters.Equals( b.variableParameters )                   ) return false;            
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( Detonation a, Detonation b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}