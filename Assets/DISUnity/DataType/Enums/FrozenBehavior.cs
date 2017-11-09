
namespace DISUnity.DataType.Enums
{
	/// <summary>
	/// Describes frozen behavior in StopFreeze PDU.
	/// </summary>
	public enum FrozenBehavior : byte
	{
		SimClock                                                          = 0,
		TransmitPDU                                                       = 1,
		ReceivePDU                                                        = 2
	}
}