using System;
using UnityEngine.Events;
using DISUnity.Simulation;

/// <summary>
/// Unity can not serialize/display in the inspector generics so we must create derived classes to fix the issue.
/// </summary>
namespace DISUnity.Events
{
    [Serializable]
    public class EntityEvent : UnityEvent<Entity> { }

    [Serializable]
    public class RemoteEntityEvent : UnityEvent<RemoteEntity> { }

    [Serializable]
    public class LocalEntityEvent : UnityEvent<LocalEntity> { }
}