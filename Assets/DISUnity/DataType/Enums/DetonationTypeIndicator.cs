#if DIS_VERSION_7

namespace DISUnity.DataType.Enums
{
    /// <summary>
    /// Indicates the type of descriptor used in a Detonation PDU in DIS 7.
    /// </summary>
    public enum DetonationTypeIndicator : byte
    {
        Munition = 0,
        Expendable = 1,
        NonMunitionExplosion = 2
    }
} 

#endif