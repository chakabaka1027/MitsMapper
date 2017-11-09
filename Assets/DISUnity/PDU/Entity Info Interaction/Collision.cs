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
    /// Sent upon a collision event. This could be between a simulated entity or another object
    /// in the simulated world (e.g., a cultural feature such as a bridge or building).
    /// 
    /// A collision shall be defined as an event that occurs when all of the following conditions are true:
    /// a)Two simulated entities intersect (one of which may be a terrain object).
    /// b)Distance between the origins of the two simulated entities is decreasing.
    /// c)At least one of the entities is moving at a speed greater than COLLISION_THRSH.
    ///
    /// Issuance of the Collision PDU:
    /// The Collision PDU should be issued by an entity when a collision is detected between the issuing entity and an
    /// object or some other entity taking part in the simulation exercise. If the collision involves two entities, both
    /// entities shall issue the Collision PDU even if only one of them detected the collision. An entity that receives
    /// a Collision PDU indicating another entity has collided with it without first detecting such a collision shall
    /// issue a Collision PDU naming the entity that issued the first Collision PDU. If the entity subsequently
    /// detects the same collision event, it shall not generate a Collision PDU to report it. When a simulation
    /// application receives a Collision PDU naming an entity it simulates as the other party involved in a collision
    /// after reporting the same collision event, it shall not send another Collision PDU in response. A simulation
    /// application shall always issue one, and only one, Collision PDU per collision event it detects or is informed
    /// about in which an entity it simulates is a participant, even if that application does not perform collision
    /// detection tests.
    /// </summary>
    /// <DIS_Version>5</DIS_Version>
    /// <size>60 bytes</size>
    [Serializable]
    public class Collision : Header, IPduBodyDecoder
    {
        #region Properties

        #region Private

        [Tooltip( Tooltips.IssuingEntityID )]
        [SerializeField]
        private EntityIdentifier issuingEntityID = new EntityIdentifier();

        [Tooltip( Tooltips.CollidingEntityID )]
        [SerializeField]
        private EntityIdentifier collidingEntityID = new EntityIdentifier();

        [Tooltip( Tooltips.EventID )]
        [SerializeField]
        private EntityIdentifier eventID = new EntityIdentifier();

        [SerializeField]
        private CollisionType collisionType;

        [Tooltip( Tooltips.VelocityCollision )]
        [SerializeField]
        private Vector3 velocity = Vector3.zero;

        [Tooltip( Tooltips.Mass )]
        [SerializeField]
        private float mass;

        [Tooltip( Tooltips.LocationCollision )]
        [SerializeField]
        private Vector3 location = Vector3.zero;
        
        #endregion Private

        /// <summary>
        /// Total size of PDU in bytes.
        /// </summary>        
        public override int Length
        {
            get
            {
                return 60;
            }
        }

        /// <summary>
        /// Entity Issuing the collision.
        /// </summary>
        public EntityIdentifier IssuingEntityID
        {
            get
            {
                return issuingEntityID;
            }
            set
            {
                isDirty = true;
                issuingEntityID = value;
            }
        }

        /// <summary>
        /// Entity that has collided with the issuing entity. 
        /// If collision is with terrain or unknown entity the id will be ENTITY_ID_UNKNOWN.
        /// </summary>
        public EntityIdentifier CollidingEntityID
        {
            get
            {
                return collidingEntityID;
            }
            set
            {
                isDirty = true;
                collidingEntityID = value;
            }
        }

        /// <summary>
        /// ID generated to associate related collision events to this one.
        /// </summary>
        public EntityIdentifier EventID
        {
            get
            {
                return eventID;
            }
            set
            {
                isDirty = true;
                eventID = value;
            }
        }

        /// <summary>
        /// The type of collision.
        /// </summary>
        public CollisionType CollisionType
        {
            get
            {
                return collisionType;
            }
            set
            {
                isDirty = true;
                collisionType = value;
            }
        }

        /// <summary>
        /// The velocity (at the simulation time the collision is detected) of the issuing entity in world coordinates.
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
        /// The mass of the issuing entity in kilograms.
        /// </summary>
        public float Mass
        {
            get
            {
                return mass;
            }
            set
            {
                isDirty = true;
                mass = value;
            }
        }

        /// <summary>
        /// The location of the collision with respect to the entity with which the issuing entity collided.
        /// Entity Coordinate Vector: Location relative to a particular entity shall be specified with respect
        /// to the origin of the entity’s coordinate system.
        /// </summary>
        public Vector3 Location
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

        #endregion Properties

        public Collision()
        {
            protocolVersion = ProtocolVersion.IEEE_1278_1_1995; // Min version required to support this PDU
            pDUType = PDUType.Collision;
            protocolFamily = ProtocolFamily.EntityInformationInteraction;
        }

        /// <summary>
        /// Creates a new instance from a binary stream only decoding the body.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="br"></param>
        public Collision( Header h, BinaryReader br )
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

            issuingEntityID.Decode( br );
            collidingEntityID.Decode( br );
            eventID.Decode( br );
            collisionType = ( CollisionType )br.ReadByte();
            br.BaseStream.Seek( 1, SeekOrigin.Current ); // Skip padding
            velocity.Set( br.ReadSingle(), br.ReadSingle(), br.ReadSingle() );
            mass = br.ReadSingle();
            location.Set( br.ReadSingle(), br.ReadSingle(), br.ReadSingle() );
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            base.Encode( bw ); // Header
            issuingEntityID.Encode( bw );
            collidingEntityID.Encode( bw );
            eventID.Encode( bw );
            bw.Write( ( byte )collisionType );
            bw.Write( ( byte )0 ); // Padding
            bw.Write( velocity.x );
            bw.Write( velocity.y );
            bw.Write( velocity.z );
            bw.Write( mass );
            bw.Write( location.x );
            bw.Write( location.y );
            bw.Write( location.z );           
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.Append( "Issuing EntityID: " + issuingEntityID.ToString() );
            sb.Append( "Colliding EntityID: " + collidingEntityID.ToString() );
            sb.Append( "Event: " + eventID.ToString() );
            sb.AppendLine( "Collision Type: " + collisionType );
            sb.AppendLine( "Velocity: " + velocity.ToString() );
            sb.AppendFormat( "Mass {0}kg\n", mass );
            sb.AppendLine( "Location: " + location.ToString() );
            return sb.ToString();
        }

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>        
        /// <returns></returns>
        public bool Equals( Collision b )
        {

            if( !Header.Equals( this, b )                        ) return false;
            if( !issuingEntityID.Equals( b.issuingEntityID )     ) return false;
            if( !collidingEntityID.Equals( b.collidingEntityID ) ) return false;
            if( !eventID.Equals( b.eventID )                     ) return false;
            if( collisionType != b.collisionType                 ) return false;
            if( !velocity.Equals( b.velocity )                   ) return false;
            if( mass != b.mass                                   ) return false;
            if( !location.Equals( b.location )                   ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( Collision a, Collision b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}
