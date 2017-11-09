
namespace DISUnity.DataType.Enums
{
    /// <summary>
    /// Represents the result of a detonation.
    /// </summary>
    public enum DetonationResult : byte
    {
        Other                                                             = 0,
        EntityImpact                                                      = 1,
        EntityProximateDetonation                                         = 2,
        GroundImpact                                                      = 3,
        GroundProximateDetonation                                         = 4,
        Detonation                                                        = 5,
        NoneOrNoDetonationDud                                             = 6,
        HE_HitSmall                                                       = 7,
        HE_HitMedium                                                      = 8,
        HE_HitLarge                                                       = 9,
        ArmorPiercingHit                                                  = 10,
        DirtblastSmall                                                    = 11,
        DirtblastMedium                                                   = 12,
        DirtblastLarge                                                    = 13,
        WaterblastSmall                                                   = 14,
        WaterblastMedium                                                  = 15,
        WaterblastLarge                                                   = 16,
        AirHit                                                            = 17,
        BuildingHitSmall                                                  = 18,
        BuildingHitMedium                                                 = 19,
        BuildingHitLarge                                                  = 20,
        MineClearingLineCharge                                            = 21,
        EnvironmentObjectImpact                                           = 22,
        EnvironmentObjectProximateDetonation                              = 23,
        WaterImpact                                                       = 24,
        AirBurst                                                          = 25,
        KillWithFragmentType1                                             = 26,
        KillWithFragmentType2                                             = 27,
        KillWithFragmentType3                                             = 28,
        KillWithFragmentType1AfterflyOutFailure                           = 29,
        KillWithFragmentType2AfterflyOutFailure                           = 30,
        MissDueToflyOutFailure                                            = 31,
        MissDueToEndGameFailure                                           = 32,
        MissDueToflyOutAndEndGameFailure                                  = 33
    }
}
