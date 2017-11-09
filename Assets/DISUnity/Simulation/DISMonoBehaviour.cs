using UnityEngine;

namespace DISUnity.Simulation
{    
    /// <summary>
    /// Base class for all DIS behaviours. 
    /// </summary>
	public class MonoBehaviour : UnityEngine.MonoBehaviour 
	{
		#region Properties
		
        #region Private

        [SerializeField]		
		private DISView dISView;
		
		#endregion Private
		
		#region Public
				
		/// <summary>
		/// DISVIew for this behaviour.
		/// </summary>				
		public DISView DISView
		{
			get
			{
                if( dISView == null )
                {
                    dISView = DISView.Get( this );
                }

                return dISView;				
			}			
		}
		
		#endregion Public
		
		#endregion Properties	
	}
}