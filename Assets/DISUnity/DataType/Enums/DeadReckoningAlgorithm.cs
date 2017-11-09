
namespace DISUnity.DataType.Enums
{
    /// <summary>
    /// Dead Reckoning Parameters                                            
    /// Dead Reckoning Algorithms consist of 3 elements.
    /// DRM_X_Y_Z                                                           
    /// X - Specifies rotation as either fixed(F) or rotating(R).           
    /// Y - Specifies dead reckoning rates to be held constant as either rate of position (P) or rate of velocity (V).                   
    /// Z - Specifies the coordinate system as either world coordinates (W) or body axis coordinates(B).  
    /// </summary>
    public enum DeadReckoningAlgorithm : byte
    {
        Other = 0,

        /// <summary>
        /// No movement, the object is static
        /// </summary>
        Static = 1,

        /// <summary>
        /// Constant velocity (or low acceleration)linear motion    
        /// </summary>
        DRM_F_P_W = 2,

        /// <summary>
        /// Similar to F_P_W but where orientation is required (e.g. visual simulation).
        /// </summary>
        DRM_R_P_W = 3,

        /// <summary>
        /// Similar to F_V_W but where orientation is required (e.g. visual simulation).                               
        /// </summary>
        DRM_R_V_W = 4,

        /// <summary>
        /// High speed or manoeuvring  at any speed (e.g. TBM, ICBM, SAM, SSM, and ASM weapons) 
        /// </summary>
        DRM_F_V_W = 5,

        /// <summary>
        /// Similar to R_P_W but when body-centred calculation is preferred.
        /// </summary>
        DRM_F_P_B = 6,

        /// <summary>
        /// Similar to R_V_W but when body-centred calculation is preferred.
        /// </summary>
        DRM_R_P_B = 7,

        /// <summary>
        /// Similar to F_V_W but when body-centred calculation is preferred.
        /// </summary>
        DRM_R_V_B = 8,

        /// <summary>
        /// Similar to F_P_B but when body-centred calculation is preferred.   
        /// </summary>
        DRM_F_V_B = 9
    }
}

                                