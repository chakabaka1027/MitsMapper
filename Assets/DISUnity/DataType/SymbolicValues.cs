using UnityEngine;
using System.Collections;

namespace DISUnity.DataType
{
    /// <summary>
    /// DIS Symbolic names used to represent DIS numeric values.
    /// </summary>
    public class SymbolicValues
    {
        /// <summary>
        /// s
        /// </summary>
        public const int AGG_RESPONSE = 10;                                    

        public const int ALL_AGGREGATES = 0xffff;

        public const int ALL_APPLIC = 0xffff;

        public const int ALL_BEAMS = 0xffff;

        public const int ALL_EMITTERS = 0xffff;

        public const int ALL_ENTITIES = 0xffff;

        public const int ALL_OBJECTS = 0xffff;

        public const int ALL_SITES = 0xffff;

        /// <summary>
        /// s
        /// </summary>
        public const int COLLISION_ELASTIC_TIMEOUT = 5;

        /// <summary>
        /// m/s
        /// </summary>
        public const float COLLISION_THRSH = 0.1f; 

        public const float DE_ENERGY_THRSH = 0.01f;

        /// <summary>
        /// m
        /// </summary>
        public const float DE_PRECISION_AIMING_THRSH = 0.5f;

        /// <summary>
        /// degs
        /// </summary>
        public const int DRA_ORIENT_THRSH_DFLT = 3; 

        /// <summary>
        /// m
        /// </summary>
        public const int DRA_POS_THRSH_DFLT = 1; 

        public readonly EntityIdentifier D_SPOT_NO_ENTITY = new EntityIdentifier( NO_SITE , NO_APPLIC , NO_ENTITY );

        /// <summary>
        /// rads/s^2
        /// </summary>
        public const float EE_AD_PULRAT_THRSH = 0.017f;

        /// <summary>
        /// rads/s^2
        /// </summary>
        public const float EE_AD_PULACC_THRSH = 0.017f; 

        /// <summary>
        /// degs
        /// </summary>
        public const int EE_AZ_THRSH = 1;

        /// <summary>
        /// degs
        /// </summary>
        public const int EE_EL_THRSH = 1; 

        /// <summary>
        /// dB
        /// </summary>
        public const int EE_ERP_THRSH = 1;

        /// <summary>
        /// Hz
        /// </summary>
        public const int EE_FREQ_THRSH = 1; 

        /// <summary>
        /// Hz
        /// </summary>
        public const int EE_FRNG_THRSH = 1; 

        /// <summary>
        /// m/s
        /// </summary>
        public const int EE_FT_VEL_THRSH = 1; 

        /// <summary>
        /// m/s^2
        /// </summary>
        public const int EE_FT_ACC_THRSH = 1; 

        /// <summary>
        /// m
        /// </summary>
        public const int EE_FT_MWD_THRSH = 10000; 

        /// <summary>
        /// s
        /// </summary>
        public const int EE_FT_KT_THRSH = 10; 

        /// <summary>
        /// m
        /// </summary>
        public const int EE_FT_ESP_THRSH = 10; 

        /// <summary>
        /// entities/beam
        /// </summary>
        public const int EE_HIGH_DENSITY_THRSH = 10;

        /// <summary>
        /// Hz
        /// </summary>
        public const int EE_PRF_THRSH = 1;

        /// <summary>
        /// µs
        /// </summary>
        public const int EE_PW_THRSH = 1;

        public readonly EntityIdentifier ENTITY_ID_UNKNOWN = new EntityIdentifier( NO_SITE, NO_APPLIC, NO_ENTITY );

        /// <summary>
        /// m
        /// </summary>
        public const int EP_DIMENSION_THRSH = 1; 

        public const int EP_NO_SEQUENCE = 0xffff;

        /// <summary>
        /// m
        /// </summary>
        public const int EP_POS_THRSH = 1; 

        /// <summary>
        /// ±%
        /// </summary>
        public const int EP_STATE_THRSH = 10; 

        /// <summary>
        /// ±%
        /// </summary>
        public const int GD_GEOMETRY_CHANGE = 10; 

        /// <summary>
        /// ±%
        /// </summary>
        public const int GD_STATE_CHANGE = 10; 

        /// <summary>
        /// min tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_KIND_CULTURAL_FEATURE = 1;

        /// <summary>
        /// min tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_KIND_ENVIRONMENTAL = 1;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_KIND_EXPENDABLE = 5; 

        /// <summary>
        /// min tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_KIND_LIFE_FORM = 1;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_KIND_MUNITION = 5; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_KIND_RADIO = 5;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_KIND_SENSOR_EMITTER = 5; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_KIND_SUPPLY = 5;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_PLATFORM_AIR = 5;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_PLATFORM_LAND = 55;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_PLATFORM_SPACE = 5;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_PLATFORM_SUBSURFACE = 55;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_ESPDU_PLATFORM_SURFACE = 55;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_AGGREGATE_STATE = 30;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_APPEARANCE = 60; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const float HBT_PDU_DE_FIRE = 0.5f; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_DESIGNATOR = 5;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_EE = 10; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_ENTITY_DAMAGE = 10; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_ENVIRONMENTAL_PROCESS = 15; 

        /// <summary>
        /// min tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_GRIDDED_DATA = 15; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_IFF = 10; 

        /// <summary>
        /// min tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_ISGROUPOF = 1;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_MINEFIELD_DATA = 5; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_MINEFIELD_STATE = 5; 

        /// <summary>
        /// min tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_RECEIVER = 1;

        /// <summary>
        /// min tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_SEES = 3; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_TRANSMITTER = 2;

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_TSPI = 30; 

        /// <summary>
        /// min tolerance: ±10%
        /// </summary>
        public const int HBT_PDU_UA = 3; 

        /// <summary>
        /// min tolerance: ±10%
        /// </summary>
        public const int HBT_STATIONARY = 1; 

        /// <summary>
        /// The entity timeout parameter is based on taking the specific entity heartbeat parameter and multiplying it by the HBT_TIMEOUT_MPLIER.
        /// </summary>
        public const float HBT_TIMEOUT_MPLIER = 2.4f; 

        /// <summary>
        /// ms
        /// </summary>
        public const int HQ_TOD_DIFF_THRSH = 20; 

        /// <summary>
        /// s
        /// </summary>
        public const int IFF_CHG_LATENCY = 2;

        /// <summary>
        /// deg
        /// </summary>
        public const int IFF_AZ_THRSH = 3;

        /// <summary>
        /// deg
        /// </summary>
        public const int IFF_EL_THRSH = 3; 

        /// <summary>
        /// s
        /// </summary>
        public const int IFF_PDU_FINAL = 10; 

        /// <summary>
        /// s
        /// </summary>
        public const int IFF_PDU_RESUME = 10; 
        
        public const int MAX_PDU_SIZE_BITS = 65536;
        
        public const int MAX_PDU_SIZE_OCTETS = 8192;

        /// <summary>
        /// s
        /// </summary>
        public const float MINEFIELD_CHANGE = 2.5f; 

        /// <summary>
        /// s tolerance: ±10%
        /// </summary>
        public const int MINEFIELD_RESPONSE_TIMER = 1; 

        public const int MULTIPLES_PRESENT = 0;

        public const int NO_AGG = 0;

        public const ushort NO_APPLIC = 0;

        public const int NO_BEAM = 0;

        public const int NO_CATEGORY = 0;

        public const int NO_EMITTER = 0;

        public const ushort NO_ENTITY = 0;

        public readonly EntityIdentifier NO_ENTITY_IMPACTED = new EntityIdentifier( NO_SITE , NO_APPLIC , NO_ENTITY );

        public const int NO_FIRE_MISSION = 0;

        public const int NO_KIND = 0;

        public readonly EntityIdentifier NO_LOCATION = new EntityIdentifier( NO_SITE , NO_APPLIC , NO_ENTITY );      
        
        public const int NO_OBJECT = 0;

        public const int NO_PATTERN = 0;

        public const int NO_REF_NUMBER = 0;

        public const ushort NO_SITE = 0;

        public const int NO_SPECIFIC = 0;

        public readonly EntityIdentifier NO_SPECIFIC_ENTITY = new EntityIdentifier( NO_SITE, NO_APPLIC, NO_ENTITY );

        public const int NO_SUBCAT = 0;

        public const int NO_VALUE = 0;

        /// <summary>
        /// min
        /// </summary>
        public const int NON_SYNC_THRSH = 1; 

        /// <summary>
        /// s
        /// </summary>
        public const int REPAR_REC_T1_DFLT = 5;

        /// <summary>
        /// s
        /// </summary>
        public const int REPAR_SUP_T1_DFLT = 12; 

        /// <summary>
        /// s
        /// </summary>
        public const int REPAR_SUP_T2_DFLT = 12; 

        /// <summary>
        /// s
        /// </summary>
        public const int RESUP_REC_T1_DFLT = 5; 

        /// <summary>
        /// s
        /// </summary>
        public const int RESUP_REC_T2_DFLT = 55; 

        /// <summary>
        /// min
        /// </summary>
        public const int RESUP_SUP_T1_DFLT = 1; 

        public const int RQST_ASSIGN_ID = 0xffff;

        /// <summary>
        /// ±° in the axis of deflection
        /// </summary>
        public const int SEES_NDA_THRSH = 2; 

        /// <summary>
        /// ±% of the maximum value of the Power
        /// </summary>
        public const int SEES_PS_THRSH = 10; 

        /// <summary>
        /// ±% of the maximum speed in RPM
        /// </summary>
        public const int SEES_RPM_THRSH = 5; 

        /// <summary>
        /// Octets for Internet Protocol Version 4 networks
        /// </summary>
        public const ushort SMALLEST_MTU_OCTETS = 1400; 

        public const int SM_REL_RETRY_CNT = 3;

        /// <summary>
        /// s
        /// </summary>
        public const int SM_REL_RETRY_DELAY = 2; // s

        public readonly EntityIdentifier TARGET_ID_UNKNOWN = new EntityIdentifier( NO_SITE, NO_APPLIC, NO_ENTITY );
        
        /// <summary>
        /// s
        /// </summary>
        public const int TIMESTAMP_AHEAD = 5; 

        /// <summary>
        /// s
        /// </summary>
        public const int TIMESTAMP_BEHIND = 5; 

        /// <summary>
        /// s
        /// </summary>
        public const int TI_TIMER1 = 2; 

        /// <summary>
        /// s
        /// </summary>
        public const int TI_TIMER2 = 12; 

        /// <summary>
        /// s
        /// </summary>
        public const int TO_AUTO_RESPONSE_TIMER = 5; 

        /// <summary>
        /// s
        /// </summary>
        public const int TO_MAN_RESPONSE_TIMER = 120; 

        /// <summary>
        /// s
        /// </summary>
        public const int TR_TIMER1 = 5; 

        /// <summary>
        /// s
        /// </summary>
        public const int TR_TIMER2 = 60; 

        /// <summary>
        /// deg
        /// </summary>
        public const int TRANS_ORIENT_THRSH_DFLT = 180; 

        /// <summary>
        /// m
        /// </summary>
        public const int TRANS_POS_THRSH_DFLT = 500; 

        /// <summary>
        /// deg
        /// </summary>
        public const int UA_ORIENT_THRSH = 2; 

        /// <summary>
        /// m
        /// </summary>
        public const int UA_POS_THRSH = 10; 

        /// <summary>
        /// ±% of maximum rate of change
        /// </summary>
        public const int UA_SRPM_ROC_THRSH = 10; 

        /// <summary>
        /// ±% of maximum shaft rate in RPM
        /// </summary>
        public const int UA_SRPM_THRSH = 5;

        public const int UNTIL_FURTHER_NOTICE = 65535;

        public readonly EntityIdentifier MUNITION_NOT_TRACKED = new EntityIdentifier( NO_SITE , NO_APPLIC , NO_ENTITY );
        
        public const int TARGET_IN_TJ_FIELD_DFLT = 10;

        /// <summary>
        /// Deprecated Symbolic Value
        /// </summary>
        public const int HRT_BEAT_MOVE_TIMER = 2;

        /// <summary>
        /// Deprecated Symbolic Value
        /// </summary>
        public const float HRT_BEAT_MPLIER = 2.4f;

        /// <summary>
        /// Deprecated Symbolic Value
        /// </summary>
        public const int HRT_BEAT_TIMER = 5;
    }
}