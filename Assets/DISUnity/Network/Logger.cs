using UnityEngine;
using System.Collections;
using DISUnity.PDU.EntityInfoInteraction;
using DISUnity.PDU;

namespace DISUnity.Network
{
    [AddComponentMenu( "DISUnity/Network/Logger" )]
    public class Logger : MonoBehaviour
    {
        #region Properties
        
        private GameObject espdus;
        
        #endregion Properties

        /// <summary>
        /// Init
        /// </summary>
        public void Start()
        {
            espdus = new GameObject( "Logged Entity State PDU's" );
        }

        /// <summary>
        /// Start listening
        /// </summary>
        public void OnEnable()
        {            
            DecodeFactory.Instance.anyReceived.AddListener( OnPDU );
        }

        /// <summary>
        /// Stop listening
        /// </summary>
        public void OnDisable()
        {
            DecodeFactory.Instance.anyReceived.RemoveListener( OnPDU );                        
        }

        /// <summary>
        /// On ESPDU.
        /// </summary>
        /// <param name="es"></param>
        private void OnPDU( Header es )
        {
            if( es is EntityState )
            {
                GameObject g = new GameObject( System.DateTime.Now.ToString() );
                EntityStateWrapper e = g.AddComponent<EntityStateWrapper>();
                e.pdu = es as EntityState;
                g.transform.parent = espdus.transform;
            }
        }
    }
}