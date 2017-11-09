using System;
using UnityEngine.Events;
using DISUnity.PDU;
using DISUnity.PDU.EntityInfoInteraction;
using DISUnity.PDU.Warfare;
using DISUnity.PDU.RadioCommunications;
using DISUnity.PDU.SimulationManagement;

/// <summary>
/// Unity can not serialize/display in the inspector generics so we must create derived classes to fix the issue.
/// </summary>
namespace DISUnity.Events
{
    [Serializable]
    public class PduEvent : UnityEvent<Header> { }
}