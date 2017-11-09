
namespace DISUnity.DataType.Enums
{
	/// <summary>
	/// Indicates if the receiving entity is able to comply with the request.
	/// Used in:
	///  Acknowledge PDU    
	///  Acknowledge-R PDU  
	/// </summary>
	public enum AcknowledgeResponseFlag 
	{
		OtherAcknowledgeResponseFlag                                      = 0,
		AbleToComply                                                      = 1,
		UnableToComply                                                    = 2
	}
}
