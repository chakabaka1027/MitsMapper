
namespace DISUnity.DataType.Enums
{
    /// <summary>
    /// Version of DIS being used.
    /// </summary>
    public enum ProtocolVersion : byte
    {
        Other                                                             = 0,
        DIS_PDU_Version_1                                                 = 1,
        IEEE_1278_1993                                                    = 2,
        DIS_PDU_Version_2_Third_Draft                                     = 3,
        DIS_PDU_Version_2_Fourth_Draft                                    = 4,
        IEEE_1278_1_1995                                                  = 5,
        IEEE_1278_1A_1998                                                 = 6,
        IEEE_1278_1_2012                                                  = 7  
    }
}

