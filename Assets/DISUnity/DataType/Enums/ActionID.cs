
namespace DISUnity.DataType.Enums
{
	/// <summary>
	/// Indicates the type of action requested.
	/// Used In:                                                            
	///  Action Request PDU                                                     
	///  Action Request-R PDU     
	/// </summary>
	public enum ActionID 
	{
		OtherActionID                                                     = 0,
		LocalStorageOfTheRequestedInformation                             = 1,
		InformSMofEvent_RanOutOfAmmunition                                = 2,
		InformSMofEvent_KilledInaAction                                   = 3,
		InformSMofEvent_Damage                                            = 4,
		InformSMofEvent_MobilityDisabled                                  = 5,
		InformSMofEvent_FireDisabled                                      = 6,
		InformSMofEvent_RanOutOfFuel                                      = 7,
		RecallCheckPointData                                              = 8,
		RecallInitialParameters                                           = 9,
		InitiateTether_Lead                                               = 10,
		InitiateTether_Follow                                             = 11,
		Unthether                                                         = 12,
		InitiateServiceStationResupply                                    = 13,
		InitiatetailGateResupply                                          = 14,
		InitiaTehitchLead                                                 = 15,
		InitiaTehitchFollow                                               = 16,
		Unhitch                                                           = 17,
		Mount                                                             = 18,
		Dismount                                                          = 19,
		StartDRC_DailyReadinessCheck                                      = 20,
		StopDRC                                                           = 21,
		DataQuery                                                         = 22,
		StatusRequest                                                     = 23,
		SendObjectStateData                                               = 24,
		Reconstitute                                                      = 25,
		LockSiteConfiguration                                             = 26,
		UnlockSiteConfiguration                                           = 27,
		UpdateSiteConfiguration                                           = 28,
		QuerySiteConfiguration                                            = 29,
		TetheringInformation                                              = 30,
		MountIntent                                                       = 31,
		AcceptSubscription                                                = 33,
		UnSubscribe                                                       = 34,
		TeleportEntity                                                    = 35,
		Changeaggregatestate                                              = 36,
		RequestStartPDU                                                   = 37,
		WakeUpGetReadyForInitialization                                   = 38,
		InitializeInternalparameters                                      = 39,
		SendPlanData                                                      = 40,
		SynchronizeInternalClocks                                         = 41,
		Run                                                               = 42,
		SaveInternalParameters                                            = 43,
		SimulatemalFunction                                               = 44,
		JoinExercise                                                      = 45,
		ResignExercise                                                    = 46,
		TimeAdvance                                                       = 47,
		TACCSFLOSRequest_Type1                                            = 100,
		TACCSFLOSRequest_Type2                                            = 101
	}
}
