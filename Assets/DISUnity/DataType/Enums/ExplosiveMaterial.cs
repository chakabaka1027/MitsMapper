#if DIS_VERSION_7

namespace DISUnity.DataType.Enums
{
    /// <summary>
    /// The material that exploded. This may be a material that may explode under certain 
    /// conditions (e.g., gasoline or grain dust), as well as, a material that is intended 
    /// to cause an explosion (e.g., TNT).
    /// </summary>
    /// <remarks>
    /// These values were taken from CR02668.
    /// </remarks>
    public enum ExplosiveMaterial  
    {
        General                                                           = 0,                
        LiquidAviationMissileFuels                                        = 1,
        LiquidOtherFuels                                                  = 2,
        LiquidExplosiveMaterial                                           = 3,
        Solid                                                             = 4,
        Gaseous                                                           = 5,
        DustMaterial                                                      = 6,
        AVGAS_AviationGas                                                 = 10,
        JetFuelUnspecified                                                = 11, 
        JP4_F40JETB                                                       = 12,
        JP5_F44JETA                                                       = 13,
        JP7                                                               = 14,
        JP8F_34JETA1                                                      = 15,
        JP10MissileFuel                                                   = 16,
        JPTS                                                              = 17,
        JetA                                                              = 18,
        JetA1                                                             = 19,
        JetB                                                              = 20,
        JetBiofuel                                                        = 21,
        GasolinePetrol_UnspecifiedOctane                                  = 151,        
        Ethanol                                                           = 153,
        E85Ethanol                                                        = 154,
        FuelOil                                                           = 155,
        Kerosene                                                          = 156,
        CrudeOil_Unspecified                                              = 157,
        LightCrudeOil                                                     = 158,
        LiquidPetroleumGas                                                = 159,
        RP1RocketFuel                                                     = 160,
        LH2RocketFuel                                                     = 161,
        LOXRocketFuel                                                     = 162,
        HydrogenLiquid                                                    = 166,
        Alcohol                                                           = 164,
        Nitroglycerin                                                     = 301,
        ANFO                                                              = 302,
        Dynamite                                                          = 451,
        TNT                                                               = 452,
        RDX                                                               = 453,
        PETN                                                              = 454,
        HMX                                                               = 455,
        C4                                                                = 456,
        CompositionC4                                                     = 457,
        NaturalGas                                                        = 601,
        Butane                                                            = 602,
        Propane                                                           = 603,
        Helium                                                            = 604,
        HydrogenGaseous                                                   = 605,
        DustUnspecifiedType                                               = 801, 
        GrainDust                                                         = 802,
        FlourDust                                                         = 803,
        SugarDust                                                         = 804
    }
} 

#endif
