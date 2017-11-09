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
    /// Describes the capabilities of the entity.
    /// </summary>
    /// <size>4 bytes</size>
    [Serializable]
    public class EntityCapabilities : DataTypeBaseSimple
    {
        #region Properties

        #region Private

        [SerializeField]
        private int capabilities;

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
        /// The field that makes up all the capabilities. Each bit represents different capabilities.
        /// </summary>
        public int Capabilities
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
        /// Does the entity have an ammunition supply?        
        /// </summary>
        /// <remarks>
        /// Bit 0
        /// </remarks>
        public bool AmmunitionSupply
        {
            get
            {
                return ( capabilities & 0x01 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                capabilities = ( int )( value ? ( capabilities | 0x01 ) : ( capabilities & ~0x01 ) );
            }
        }

        /// <summary>
        /// Does the entity have a fuel supply?        
        /// </summary>
        /// <remarks>
        /// Bit 1
        /// </remarks>
        public bool FuelSupply
        {
            get
            {
                return ( capabilities & 0x02 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                capabilities = ( int )( value ? ( capabilities | 0x02 ) : ( capabilities & ~0x02 ) );
            }
        }

        /// <summary>
        /// Does the entity provide a recovery service?     
        /// </summary>
        /// <remarks>
        /// Bit 2
        /// </remarks>
        public bool RecoveryService
        {
            get
            {
                return ( capabilities & 0x04 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                capabilities = ( int )( value ? ( capabilities | 0x04 ) : ( capabilities & ~0x04 ) );
            }
        }

        /// <summary>
        /// Does the entity provide a repair service?     
        /// </summary>
        /// <remarks>
        /// Bit 3
        /// </remarks>
        public bool RepairService
        {
            get
            {
                return ( capabilities & 0x08 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                capabilities = ( int )( value ? ( capabilities | 0x08 ) : ( capabilities & ~0x08 ) );
            }
        }

        /// <summary>
        /// Is the entity capable of Automatic Dependent Surveillance - Broadcast (ADS-B)? 
        /// </summary>
        /// <remarks>
        /// Bit 4
        /// </remarks>
        public bool ADSB
        {
            get
            {
                return ( capabilities & 0x10 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                capabilities = ( int )( value ? ( capabilities | 0x10 ) : ( capabilities & ~0x10 ) );
            }
        }

        #endregion Properties

        /// <summary>
        /// Ctor
        /// </summary>
        public EntityCapabilities()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bits">Capability bits</param>
        public EntityCapabilities( int bits )
        {
            capabilities = bits;
        }

        /// <summary>
        /// Create a new Entity Capabilites record.
        /// </summary>
        /// <param name="ammo">Does the entity have an ammunition supply?</param>
        /// <param name="fuel">Does the entity have a fuel supply?</param>
        /// <param name="recovery">Does the entity provide a recovery service?</param>
        /// <param name="repair">Does the entity provide a repair service?</param>
        /// <param name="adsb">Is the entity capable of Automatic Dependent Surveillance - Broadcast (ADS-B)?</param>
        public EntityCapabilities( bool ammo, bool fuel, bool recovery, bool repair, bool adsb )
        {
            AmmunitionSupply = ammo;
            FuelSupply = fuel;
            RecoveryService = recovery;
            RepairService = repair;
            ADSB = adsb;
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public EntityCapabilities( BinaryReader br )
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
            capabilities = br.ReadInt32();
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( capabilities );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine( "Entity Capabilities" );
            sb.AppendFormat( "\tAmmunition Supply: {0}\n", AmmunitionSupply );
            sb.AppendFormat( "\tFuel Supply: {0}\n", FuelSupply );
            sb.AppendFormat( "\tRecovery Service: {0}\n", RecoveryService );
            sb.AppendFormat( "\tRepair Service: {0}\n", RepairService );
            sb.AppendFormat( "\tADS-B Service: {0}\n", ADSB );
            return sb.ToString();
        }

        #endregion DataTypeBase

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( EntityCapabilities b )
        {
            if( capabilities != b.capabilities ) return false;            
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( EntityCapabilities a, EntityCapabilities b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}