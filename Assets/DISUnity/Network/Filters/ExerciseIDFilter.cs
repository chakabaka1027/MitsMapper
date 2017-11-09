using UnityEngine;
using System.Collections;
using DISUnity.PDU;
using DISUnity.Resources;

namespace DISUnity.Network.Filters
{
    /// <summary>
    /// Checks if the exercise ID is correct, if not rejects the PDU.
    /// </summary>
    [AddComponentMenu( "DISUnity/Network/Filter/Exercise ID" )]
    public class ExerciseIDFilter : IFactoryFilter
    {
        #region Properties

        #region Private

        [Tooltip( Tooltips.ExerciseID )]
        [SerializeField]
        private int allowedExerciseID;

        #endregion

        /// <summary>
        /// The exercise ID that will be allowed.
        /// </summary>
        public int AllowedExerciseID
        {
            get
            {
                return allowedExerciseID;
            }
            set
            {
                allowedExerciseID = value;
            }
        }

        #endregion

        /// <summary>        
        /// Returns true if the exercise ID is allowed through the filter.
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public override bool OnHeaderDecoded( Header h )
        {
            return h.ExerciseID == allowedExerciseID;          
        }
    }
}