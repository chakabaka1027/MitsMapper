using UnityEngine;
using System.Collections;
using DISUnity.PDU.EntityInfoInteraction;
using DISUnity.DataType;

namespace DISUnity.Simulation
{
    /// <summary>
    /// A simulated entity in the DIS exercise.
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        #region Properties

        #region Private

        [SerializeField]
        protected EntityState state;

        [SerializeField]
        private float lastUpdate;

        #endregion Private

        /// <summary>
        /// Our current state for this entity.
        /// </summary>
        public EntityState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        public EntityIdentifier ID
        {
            get
            {
                return state.EntityID;
            }
            set
            {
                state.EntityID = value;
            }
        }

        /// <summary>
        /// Time last update occured for this entity.
        /// </summary>
        public float LastUpdate
        {
            get
            {
                return lastUpdate;
            }
            set
            {
                lastUpdate = value;
            }
        }

        #endregion        
    }
}