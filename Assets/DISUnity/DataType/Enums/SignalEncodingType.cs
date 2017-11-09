
namespace DISUnity.DataType.Enums
{
    /// <summary>
    /// Encoding type when EncodingClass is audio   
    /// </summary>
    public enum SignalEncodingType
    {
        _8_bit_mu_law                                                     = 1,
        CVSD_per_MIL_STD_188_113                                          = 2,
        ADPCM_per_CCITT_G_721                                             = 3,
        _16_bit_linear_PCM2sComplementBigEndian                           = 4,
        _8_bit_linear_PCM                                                 = 5,
        VQ_VectorQuantization                                             = 6,
        GSM_FullRate                                                      = 8,
        GSM_HalfRate                                                      = 9,
        SpeexNarrowBand                                                   = 10,
        _16_bit_linear_PCM2sComplementLittleEndian                        = 100
    }
}
