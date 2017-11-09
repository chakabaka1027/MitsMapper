using UnityEngine;
using System.Collections;
using DISUnity.DataType;
using DISUnity.Utils;

namespace DISUnity.Simulation.Managers
{
    /// <summary>
    /// Stores general exercise info
    /// </summary>
    [AddComponentMenu( "DISUnity/Simulation/Managers/Exercise Manager" )]
    public class ExerciseManager : MonoBehaviourSingleton<ExerciseManager>
    {
        #region Properties

        #region Private

        [SerializeField]
        private byte exerciseID = 1;

        [SerializeField]
        private SimulationAddress address = new SimulationAddress();

        [SerializeField]
        private WorldCoordinateConverter worldCoordinateConverter;

        #endregion

        /// <summary>
        /// The exercise we are part of.
        /// </summary>
        public byte ExerciseID
        {
            get
            {
                return exerciseID;
            }
            set
            {
                exerciseID = value;
            }
        }

        /// <summary>
        /// The address of our application, used to identify it in the DIS exercise.
        /// </summary>
        public SimulationAddress Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }

        /// <summary>
        /// Converts between remote(DIS) and local(Unity) coordinate systems.
        /// </summary>
        public WorldCoordinateConverter WorldCoordinateConverter
        {
            get
            {
                return worldCoordinateConverter;
            }
            set
            {
                worldCoordinateConverter = value;
            }
        }

        #endregion
    }
}