using UnityEngine;
using System.Collections;
using DISUnity.PDU.EntityInfoInteraction;
using DISUnity.DataType;
using DISUnity.Simulation.Managers;

namespace DISUnity.Simulation
{
    /// <summary>
    /// A simulated entity in the DIS exercise controlled by us.
    /// </summary>
    [AddComponentMenu( "DISUnity/Simulation/Entity" )]
    public class LocalEntity : Entity
    {
        /// <summary>
        /// Join the sim.
        /// </summary>
        public void OnEnable()
        {
            EntityManager.Instance.JoinExercise( this );
        }

        /// <summary>
        /// Remove the entity from the sim.
        /// </summary>
        public void OnDisable()
        {
            EntityManager.Instance.LeaveExercise( this );
        }
    }
}