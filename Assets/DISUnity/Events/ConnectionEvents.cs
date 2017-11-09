using System;
using UnityEngine.Events;
using DISUnity.Network;

/// <summary>
/// Unity can not serialize/display in the inspector generics so we must create derived classes to fix the issue.
/// </summary>
namespace DISUnity.Events
{
    [Serializable]
    public class ConnectionEvent : UnityEvent<Connection> { }
}