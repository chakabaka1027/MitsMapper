
namespace DISUnity.DataType.Enums
{
	/// <summary>
	/// Describes the reason for StopFreeze PDU.   
	/// </summary>
	public enum StopFreezeReason : byte
	{
		OtherStopFreezeReason                                             = 0,
		Recess                                                            = 1,
		Termination                                                       = 2,
		SystemFailure                                                     = 3,
		SecurityViolation                                                 = 4,
		EntityReconstitution                                              = 5,
		StopForReset                                                      = 6,
		StopForRestart                                                    = 7,
		AbortTrainingReturnToTacticalOperations                           = 8
	}
}