using UnityEngine;
using System.Collections;
using DISUnity.DataType;

namespace DISUnity.Simulation
{
    // TODO: readonly properties to make the ID locked.

	/// <summary>
	/// DIS view. Use like NetworkView.
	/// </summary>
    public class DISView : DISUnity.Simulation.MonoBehaviour
	{		
		#region Properties
		
        #region Private

        [SerializeField]		
		private SimulationAddress simID;

        [SerializeField]
        private EntityIdentifier entID;
		
		#endregion Private
		
		#region Public
				
		/// <summary>
		/// ID of the application that owns this DIS view.		
		/// </summary>				
		public SimulationAddress SimulationID
		{
            get
            {
                return simID;
            }
            set
            {
                simID = value;
            }
		}

        /// <summary>
        /// Entity ID, not all DISViews will have one.
        /// </summary>
        public EntityIdentifier EntityID
        {
            get
            {
                return entID;
            }
            set
            {
                entID = value;
            }
        }

        /// <summary>
        /// Is this view mine?
        /// </summary>
        //public bool IsMine
        //{
        //    get
        //    {
        //        return SimulationID == DISNetwork.applicationID );
        //    }
        //}
		
		#endregion Public
		
		#endregion Properties
		
        /// <summary>
        /// Get the DISView component from this GameObject.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
	    public static DISView Get( Component component )
	    {
	        return component.GetComponent<DISView>();
	    }

        /// <summary>
        /// Get the DISView component from the GameObject.
        /// </summary>
        /// <param name="gameObj"></param>
        /// <returns></returns>
	    public static DISView Get( GameObject gameObj )
	    {
	        return gameObj.GetComponent<DISView>();
		}	
	}
}