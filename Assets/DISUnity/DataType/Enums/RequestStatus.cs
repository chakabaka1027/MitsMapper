
namespace DISUnity.DataType.Enums
{
	/// <summary>
	/// Indicates the response to an action request.
	/// Used In:                                                            
	///  Action Response PDU                                                     
	///  Action Response-R PDU     
	/// </summary>
	public enum RequestStatus 
	{
		OtherRequestStatus                                                = 0,
		Pending                                                           = 1,
		Executing                                                         = 2,
		PartiallyComplete                                                 = 3,
		Complete                                                          = 4,
		RequestRejected                                                   = 5,
		ReTransmitRequestNow                                              = 6,
		ReTransmitRequestLater                                            = 7,
		InvalidTimeParameters                                             = 8,
		SimulationTimeExceeded                                            = 9,
		RequestDone                                                       = 10,
		TACCSFLOSReply_Type1                                              = 100,
		TACCSFLOSReply_Type2                                              = 101,
		JoinExerciseRequestRejected                                       = 201
	}
}
