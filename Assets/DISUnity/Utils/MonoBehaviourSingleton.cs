using UnityEngine;
using System.Collections;

namespace DISUnity.Utils
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static T Instance
        {
            get
            {
                if( _Instance == null )
                {
                    _Instance = FindObjectOfType<T>();
                    if( _Instance == null )
                    {
                        Debug.Log( "Creating singleton instance " + typeof( T ) );
                        GameObject g = new GameObject( typeof( T ) + " Singleton" );
                        _Instance = g.AddComponent<T>();
                    }
                }
                return _Instance;
            }
        }
        private static T _Instance;

        /// <summary>
        /// Validates this is a single instance.
        /// </summary>
        public virtual void Awake()
        {
            if( _Instance == null )
            {
                _Instance = this as T;
            }
            else if( _Instance != this )
            {
                Debug.LogWarning( "More than one instance of " + typeof( T ) + " found, destroying this one: " + gameObject.name, gameObject );
                Destroy( this );
            }
        }
    }
}