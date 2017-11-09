#if DIS_VERSION_7

namespace DISUnity.DataType.Enums
{
    /// <summary>
    /// Indicates the type of descriptor used in a Fire PDU in DIS 7.
    /// </summary>
    public enum FireTypeIndicator : byte
    {
        Munition = 0,
        Expendable = 1
    }
} 

#endif