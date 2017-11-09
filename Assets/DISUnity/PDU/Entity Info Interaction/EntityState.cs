using System;
using UnityEngine;
using DISUnity.DataType.Enums;
using DISUnity.DataType;
using System.IO;
using DISUnity.Attributes;
using DISUnity.Resources;
using System.Text;

namespace DISUnity.PDU.EntityInfoInteraction
{
    /// <summary>
    /// Contains information about a particular entity.
    /// 
    /// Issuance of the Entity State PDU:
    /// A simulation shall issue an Entity State PDU when any of the following occur:
    /// a)The discrepancy between an entity’s actual state (as determined by its own internal model) and its
    /// dead reckoned state (state using specified dead reckoning algorithms) exceeds a predetermined
    /// threshold.
    /// b)A change in the entity’s appearance occurs, for instance, if the entity begins to burn or emit smoke.
    /// c)A predetermined length of real-world time has elapsed since the issuing of the last Entity State PDU.    
    /// </summary>
    /// <DIS_Version>5</DIS_Version>
    /// <size>144 bytes + 128/16 * Num Art Params</size>
    [Serializable]
    public class EntityState : Header, IPduBodyDecoder
    {
        #region Properties

        #region Private

        [Tooltip( Tooltips.EntityID )]
        [SerializeField]        
        protected EntityIdentifier entityID = new EntityIdentifier();
                
        [SerializeField]        
        protected ForceID forceID;

        [SerializeField]
        protected EntityType entityType = new EntityType();

        [Tooltip( Tooltips.AlternativeEntityType )]
        [SerializeField]
        protected EntityType alternateEntityType = new EntityType();

        [SerializeField]
        protected Vector3 linearVelocity = Vector3.zero;

        [Tooltip( Tooltips.WorldCoordinates )]
        [SerializeField]
        protected WorldCoordinates location = new WorldCoordinates();

        [Tooltip( Tooltips.Orientation )]
        [SerializeField]
        protected Vector3 orientation = Vector3.zero;

        [SerializeField]
        protected EntityAppearance appearance = new EntityAppearance();

        [SerializeField]
        protected DeadReckoningParameter deadReckoningParameter = new DeadReckoningParameter();

        [SerializeField]
        protected EntityMarking marking = new EntityMarking();

        [SerializeField]
        protected EntityCapabilities capabilities = new EntityCapabilities();

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
                length = 144 + variableParameters.Length;
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
        /// Transferred Entity Indicator
        /// Identifies whether the Simulation Address of the Entity ID contained in this PDU is the owner of the entity.
        /// false = no difference. The Simulation Address of the Entity ID is the owner of Indicator.
        /// true = difference. The Simulation Address of the Entity ID is not the owner of this entity.
        /// </summary>
        /// <remarks>
        /// DIS 7 feature.
        /// Bit 0 of PDU status field.
        /// </remarks>
        public bool TEI
        {
            get
            {
                return ( pduStatus & 0x01 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                pduStatus = ( byte )( value ? ( pduStatus | 0x01 ) : ( pduStatus & ~0x01 ) );
            }
        }

        #endif
        #endregion PDU Status

        /// <summary>
        /// ID of entity issuing the PDU. This is a unique identifier made up of 3 values. 
        /// The first 2 represent the simulation address.(site, application) and the final is the entity. 
        /// This ID should be unqiue to the exercise.
        /// </summary>
        public EntityIdentifier EntityID
        {
            get
            {
                return entityID;
            }
            set
            {
                isDirty = true;
                entityID = value;
            }
        }

        /// <summary>
        /// Force ID. Enumerated value representing the force the entity belongs to,
        /// such as friendly, opposing or neutral.
        /// </summary>
        public ForceID Force
        {
            get
            {
                return forceID;
            }
            set
            {
                isDirty = true;
                forceID = value;
            }
        }

        /// <summary>
        /// Entity Type. Consists of 7 values used to represent the type of entity.
        /// Please see DIS Enums document found on the SISO website for a full list
        /// of enumerations available.
        /// </summary>
        public EntityType EntityType
        {
            get
            {
                return entityType;
            }
            set
            {
                isDirty = true;
                entityType = value;
            }
        }

        /// <summary>
        /// This identifies the entity type to be displayed by members of forces other than that of the issuing entity.
        /// This could be used to represent an entity in disguise.
        /// </summary>
        public EntityType AlternateEntityType
        {
            get
            {
                return alternateEntityType;
            }
            set
            {
                isDirty = true;
                alternateEntityType = value;
            }
        }

        /// <summary>
        /// Linear Velocity Vector. m/s.        
        /// </summary>
        /// <remarks>
        /// The coordinate system for an entity’s linear velocity depends on the dead reckoning algorithm used.
        /// </remarks>
        public Vector3 LinearVelocity
        {
            get
            {                
                return linearVelocity;
            }
            set
            {
                isDirty = true;
                linearVelocity = value;
            }
        }

        /// <summary>
        /// The physical location of the entity in the simulated world.
        /// </summary>
        public WorldCoordinates Location
        {
            get
            {
                return location;
            }
            set
            {
                isDirty = true;
                location = value;
            }
        }

        /// <summary>
        /// Orientation of entity in euler using radians.
        /// </summary>
        public Vector3 Orientation
        {
            get
            {
                return orientation;
            }
            set
            {                
                isDirty = true;
                orientation = value;
            }
        }

        /// <summary>
        /// Entity Appearance.
        /// </summary>
        /// <remarks>
        /// Appearance is communicated via a series of bit flags/values. Such as damage states, smoking, on fire etc.        
        /// </remarks>          
        public EntityAppearance Appearance
        {
            get
            {
                return appearance;
            }
            set
            {
                isDirty = true;
                appearance = value;
            }
        }

        /// <summary>
        /// Used to provide parameters for dead reckoning the position and orientation of the entity.
        /// </summary>
        public DeadReckoningParameter DeadReckoningParameter
        {
            get
            {
                return deadReckoningParameter;
            }
            set
            {
                isDirty = true;
                deadReckoningParameter = value;
            }
        }

        /// <summary>
        /// Identifies any unique markings on an entity (for example, a bumper number or country symbol).
        /// Often used for the name of the entity.
        /// </summary>
        public EntityMarking Marking
        {
            get
            {
                return marking;
            }
            set
            {
                isDirty = true;
                marking = value;
            }
        }

        /// <summary>
        /// The entity’s capabilities.
        /// </summary>
        public EntityCapabilities Capabilities
        {
            get
            {
                return capabilities;
            }
            set
            {
                isDirty = true;
                capabilities = value;
            }
        }

        /// <summary>
        /// The Variable Parameter Records for this entity.
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

        public EntityState()
        {
            protocolVersion = ProtocolVersion.IEEE_1278_1_1995; // Min version required to support this PDU
            pDUType = PDUType.EntityState;
            protocolFamily = ProtocolFamily.EntityInformationInteraction;
        }

        /// <summary>
        /// Creates a new instance from a binary stream only decoding the body.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public EntityState( Header h, BinaryReader br )
        {
            Decode( h, br );
        }

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public virtual void Decode( Header h, BinaryReader br )
        {
            // Copy header values.
            Clone( h );

            entityID.Decode( br );
            forceID = ( ForceID )br.ReadByte();
            byte numOfArticulationParams = br.ReadByte();        
            entityType.Decode( br );
            alternateEntityType.Decode( br );
            linearVelocity.Set( br.ReadSingle(), br.ReadSingle(), br.ReadSingle() );
            location.Decode( br );
            orientation.Set( br.ReadSingle(), br.ReadSingle(), br.ReadSingle() );
            appearance.Decode( br );
            deadReckoningParameter.Decode( br );
            marking.Decode( br );
            capabilities.Decode( br );
            variableParameters.Decode( br, numOfArticulationParams );            
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            base.Encode( bw ); // Header
            entityID.Encode( bw );
            bw.Write( ( byte )forceID );            
            bw.Write( ( byte )variableParameters.NumberOfRecords );
            entityType.Encode( bw );
            alternateEntityType.Encode( bw );
            bw.Write( linearVelocity.x );
            bw.Write( linearVelocity.y );
            bw.Write( linearVelocity.z );
            location.Encode( bw );
            bw.Write( orientation.x );
            bw.Write( orientation.y );
            bw.Write( orientation.z );
            appearance.Encode( bw );
            deadReckoningParameter.Encode( bw );
            marking.Encode( bw );
            capabilities.Encode( bw );
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
            sb.Append( "Entity Identifier: " + entityID.ToString() );
            sb.AppendLine( "Force: " + forceID );
            sb.Append( entityType.ToString() );
            sb.Append( alternateEntityType.ToString() );
            sb.AppendLine( "Linear Velocity: " + linearVelocity.ToString() );
            sb.AppendLine( "World Coordinates: " + location.ToString() );
            sb.AppendLine( "Orientation: " + orientation.ToString() );
            sb.Append( appearance.ToString() ); // TODO: Entity Type specific appearance ToString()
            sb.Append( deadReckoningParameter.ToString() );
            sb.Append( marking.ToString() );
            sb.Append( capabilities.ToString() );
            sb.Append( variableParameters.ToString() );
            return sb.ToString();
        }

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( EntityState b )
        {
            if( !Header.Equals( this, b )                                  ) return false;            
            if( !entityID.Equals( b.entityID )                             ) return false;
            if( forceID != b.forceID                                       ) return false;
            if( !entityType.Equals( b.entityType )                         ) return false;
            if( !alternateEntityType.Equals( b.alternateEntityType )       ) return false;
            if( !linearVelocity.Equals( b.linearVelocity )                 ) return false;
            if( !location.Equals( b.location )                             ) return false;
            if( !orientation.Equals( b.orientation )                       ) return false;
            if( !appearance.Equals( b.appearance )                         ) return false;
            if( !deadReckoningParameter.Equals( b.deadReckoningParameter ) ) return false;
            if( !marking.Equals( b.marking )                               ) return false;
            if( !capabilities.Equals( b.capabilities )                     ) return false;
            if( !variableParameters.Equals( b.variableParameters )         ) return false;      
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( EntityState a, EntityState b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}
