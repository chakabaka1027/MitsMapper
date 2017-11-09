
namespace DISUnity.DataType.Enums
{
    /// <summary>
    /// The family of protocols to which the PDU belongs.
    /// </summary>
    public enum ProtocolFamily : byte
    {
        Other                                                         = 0,
        EntityInformationInteraction                                  = 1,
        Warfare                                                       = 2,
        Logistics                                                     = 3,
        RadioCommunications                                           = 4,
        SimulationManagement                                          = 5,
        DistributedEmissionRegeneration                               = 6,
        EntityManagement                                              = 7,
        Minefield                                                     = 8,
        SyntheticEnvironment                                          = 9,
        SimulationManagementwithReliability                           = 10,
        LiveEntity                                                    = 11,
        NonRealTime                                                   = 12,
        InformationOperations                                         = 13, //   Taken from 'IEEE 1278.1-200X Draft' SISO-REF-010 PCR Changes
        ExperimentalCGF                                               = 129,
        ExperimentalEntityInfomationFieldInstrumentation              = 130,
        ExperimentalWarfareFieldInstrumentation                       = 131,
        ExperimentalEnviromentObjectInfomationInteraction             = 132,
        ExperimentalEntityManagement                                  = 133
    }
}

