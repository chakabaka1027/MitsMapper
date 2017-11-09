
namespace DISUnity.DataType.Enums
{
	/// <summary>
	/// Indicates the type of PDU being acknowledged.
	/// Used In:                                                            
	///  Acknowledge PDU                                                     
	///  Acknowledge-R PDU     
	/// </summary>
	public enum AcknowledgeFlag 
	{
        Unknown                                                           = 0,
		CreateEntityPDU                                                   = 1,
		RemoveEntityPDU                                                   = 2,
		Start_ResumePDU                                                   = 3,
		Stop_FreezePDU                                                    = 4,
		TransferControlRequest                                            = 5
	}
}
