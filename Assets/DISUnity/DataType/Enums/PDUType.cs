
namespace DISUnity.DataType.Enums
{
    /// <summary>
    /// The type of PDU.
    /// </summary>
    public enum PDUType : byte
    {
        Other                                               = 0,
        EntityState                                         = 1,  // X
        Fire                                                = 2,  // X
        Detonation                                          = 3,  // X
        Collision                                           = 4,  // X
        ServiceRequest                                      = 5,  
        ResupplyOffer                                       = 6,  
        ResupplyReceived                                    = 7,  
        ResupplyCancel                                      = 8,  
        RepairComplete                                      = 9,  
        RepairResponse                                      = 10, 
        CreateEntity                                        = 11, 
        RemoveEntity                                        = 12, 
        StartResume                                         = 13, 
        StopFreeze                                          = 14, 
        Acknowledge                                         = 15, 
        ActionRequest                                       = 16, 
        ActionResponse                                      = 17, 
        DataQuery                                           = 18, 
        SetData                                             = 19, 
        Data                                                = 20, 
        EventReport                                         = 21, 
        Message                                             = 22, // AKA Comment PDU
        ElectromagneticEmission                             = 23, 
        Designator                                          = 24, 
        Transmitter                                         = 25, 
        Signal                                              = 26, // X
        Receiver                                            = 27, 
        IFF_ATC_NAVAIDS                                     = 28, 
        UnderwaterAcoustic                                  = 29, 
        SupplementalEmissionEntityState                     = 30, 
        IntercomSignal                                      = 31, 
        IntercomControl                                     = 32, 
        AggregateState                                      = 33, 
        IsGroupOf                                           = 34, 
        TransferControl                                     = 35, // AKA Transfer Ownership PDU
        IsPartOf                                            = 36, 
        MinefieldState                                      = 37, 
        MinefieldQuery                                      = 38, 
        MinefieldData                                       = 39, 
        MinefieldResponseNAK                                = 40, 
        EnvironmentalProcess                                = 41, 
        GriddedData                                         = 42, 
        PointObjectState                                    = 43, 
        LinearObjectState                                   = 44, 
        ArealObjectState                                    = 45, 
        TSPI                                                = 46, 
        Appearance                                          = 47, 
        ArticulatedParts                                    = 48, 
        LEFire                                              = 49, 
        LEDetonation                                        = 50, 
        CreateEntityR                                       = 51, 
        RemoveEntityR                                       = 52, 
        StartResumeR                                        = 53, 
        StopFreezeR                                         = 54, 
        AcknowledgeR                                        = 55, 
        ActionRequestR                                      = 56, 
        ActionResponseR                                     = 57, 
        DataQueryR                                          = 58, 
        SetDataR                                            = 59, 
        DataR                                               = 60, 
        EventReportR                                        = 61, 
        CommentR                                            = 62, 
        RecordR                                             = 63, 
        SetRecordR                                          = 64, 
        RecordQueryR                                        = 65, 
        CollisionElastic                                    = 66, 
        EntityStateUpdate                                   = 67, 
        DirectedEnergyFire                                  = 68, // Taken from 'IEEE 1278.1-200X Draft' SISO-REF-010 PCR Changes
        EntityDamageStatus                                  = 69, // Taken from 'IEEE 1278.1-200X Draft' SISO-REF-010 PCR Changes
        IOAction                                            = 70, // Taken from 'IEEE 1278.1-200X Draft' SISO-REF-010 PCR Changes
        IOReport                                            = 71, // Taken from 'IEEE 1278.1-200X Draft' SISO-REF-010 PCR Changes
        Attribute                                           = 72, // Taken from 'IEEE 1278.1-200X Draft' SISO-REF-010 PCR Changes
        AnnounceObject                                      = 129,
        DeleteObject                                        = 130,
        DescribeEvent                                       = 132,
        DescribeObject                                      = 133,
        RequestEvent                                        = 134,
        RequestObject                                       = 135
    }
}

