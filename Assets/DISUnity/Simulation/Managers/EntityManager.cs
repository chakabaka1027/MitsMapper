using UnityEngine;
using System.Collections.Generic;
using DISUnity.Network;
using DISUnity.PDU.EntityInfoInteraction;
using DISUnity.Resources;
using DISUnity.Attributes;
using DISUnity.Utils;
using System.Collections;
using System;
using DISUnity.Events;
using DISUnity.PDU;

namespace DISUnity.Simulation.Managers
{
    [AddComponentMenu( "DISUnity/Simulation/Managers/Entity Manager" )]
    public class EntityManager : MonoBehaviourSingleton<EntityManager>
    {
        #region Properties

        #region Private

        [SerializeField]
        private EntityTypeMap entityMap;

        [Tooltip( Tooltips.HeartBeat )]
        [SerializeField]
        private float heartBeat = 5;

        [SerializeField]
        private GameObject remoteEntityParent;

        #endregion

        /// <summary>
        /// How often in seconds to send an update out to the DIS network if an entity has had no changes.
        /// </summary>
        public float HeartBeat
        {
            get
            {
                return heartBeat;
            }
            set
            {
                heartBeat = value;
            }
        }

        /// <summary>
        /// Entity map used to determine what to instantiate for each type of entity.
        /// </summary>
        public EntityTypeMap EntityMap
        {
            get
            {
                return entityMap;
            }
            set
            {
                entityMap = value;
            }
        }

        /// <summary>
        /// GameObject to parent all remote entities to. If null entites will not have parents.
        /// </summary>
        public GameObject RemoteEntityParent
        {
            get
            {
                return remoteEntityParent;
            }
            set
            {
                remoteEntityParent = value;
            }
        }

        /// <summary>
        /// Entities that do no belong to this application.
        /// </summary>
        public Dictionary<long, RemoteEntity> RemoteEntities
        {
            get;
            set;
        }

        /// <summary>
        /// Entities that are part of the application, controlled by us.
        /// </summary>
        public Dictionary<long, LocalEntity> LocalEntites
        {
            get;
            set;
        }

        #endregion

        #region Events

        /// <summary>
        /// Called when a new entity is created, remote or local.
        /// </summary>
        public EntityEvent newEntity;

        /// <summary>
        /// Called when a new local entity is created.
        /// </summary>
        public LocalEntityEvent newLocalEntity; 

        /// <summary>
        /// Called when a new remote entity is created.
        /// </summary>
        public RemoteEntityEvent newRemoteEntity;        

        /// <summary>
        /// Called when a local or remote entity is removed from the sim.
        /// </summary>
        public EntityEvent removeEntity;        

        /// <summary>
        /// Called when a local entity is removed from the sim.
        /// </summary>
        public LocalEntityEvent removeLocalEntity;        

        /// <summary>
        /// Called when a remote entity is removed from the sim.
        /// </summary>
        public RemoteEntityEvent removeRemoteEntity;        

        #endregion Events

        /// <summary>
        /// Init.
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            RemoteEntities = new Dictionary<long, RemoteEntity>();
            LocalEntites = new Dictionary<long, LocalEntity>();
        }

        /// <summary>
        /// Start listening to events.
        /// </summary>
        public void OnEnable()
        {            
            DecodeFactory.Instance.entityStateReceived.AddListener( OnEntityState );            
        }

        /// <summary>
        /// Stop listening to events.
        /// </summary>
        public void OnDisable()
        {
            DecodeFactory.Instance.entityStateReceived.RemoveListener( OnEntityState );                       
        }

        /// <summary>
        /// Registers a new local entity to the sim.
        /// </summary>
        /// <param name="ls"></param>
        public void JoinExercise( LocalEntity ls )
        {            
            // Check entity does not already exist
            if( !LocalEntites.ContainsKey( ls.ID.HashCode ) )
            {
                // Add entity
                LocalEntites.Add( ls.ID.HashCode, ls );
            }
            else
            {
                Debug.LogWarning( string.Format( "Entity is already managed, ignoring: Name - {0}, ID - {1}", ls.name, ls.ID ) );
            }            
        }

        /// <summary>
        /// Removes an entity from the sim
        /// </summary>
        /// <param name="ls"></param>
        public void LeaveExercise( LocalEntity ls )
        {
            // Check entity does not already exist
            if( !LocalEntites.Remove( ls.ID.HashCode ) )
            {
                Debug.LogWarning( string.Format( "Can not remove entity, it is not part of the exercise: Name - {0}, ID - {1}", ls.name, ls.ID ) );
            }
        }

        /// <summary>
        /// Checks entity to see if it has timedout.
        /// </summary>
        /// <param name="re"></param>
        /// <returns></returns>
        private IEnumerator TimeoutRemoteEntity( RemoteEntity re )
        {
            while( Application.isPlaying )
            {                
                float elapsedTime = Time.timeSinceLevelLoad - re.LastUpdate;
                if( elapsedTime > heartBeat )
                {
                    break;
                }
                else
                {
                    yield return new WaitForSeconds( elapsedTime );
                }
            }

            // Entity has expired, remove it
            Debug.Log( "Entity Timeout. Removing: " + re.State.Marking.ASCII );
            
            // Fire events                        
            RemoveEntity( re, true );
        }

        /// <summary>
        /// Remove a entity from the sim and fire events. 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="destroy">Destroy the entity?</param>
        private void RemoveEntity( Entity e, bool destroy )
        {
            if( e is RemoteEntity )
            {
                Debug.Log( string.Format( "Removing entity: {0}({1})", e.State.Marking.ASCII, e.State.EntityID ) );
                if( RemoteEntities.Remove( e.ID.HashCode ) )
                {
                    removeRemoteEntity.Invoke( e as RemoteEntity );                    
                }
                else
                {
                    return;
                }
            }
            else
            {
                if( LocalEntites.Remove( e.ID.HashCode ) )
                {
                    removeLocalEntity.Invoke( e as LocalEntity );                    
                }
                else
                {
                    return;
                }
            }

            removeEntity.Invoke( e );            

            if( destroy )
            {
                Destroy( e.gameObject );
            }
        }

        /// <summary>
        /// Creates a new remote entity from an entity state pdu
        /// </summary>
        /// <param name="from">GameObject to use, from entity map.</param>
        /// <param name="es">The entity state pdu.</param>
        /// <returns></returns>
        private RemoteEntity CreateEntity( GameObject from, EntityState es )
        {
            GameObject entityGO = Instantiate( from ) as GameObject;
            RemoteEntity re = entityGO.GetComponent<RemoteEntity>() ?? entityGO.AddComponent<RemoteEntity>();
            RemoteEntities.Add( es.EntityID.HashCode, re ); // Add new entity
            re.Init( es );

            // Fire events
            newEntity.Invoke( re );
            newRemoteEntity.Invoke( re );            

            // Start periodic checks, if the entity is not updated within the HeartBeat duration then it will be removed from the sim.            
            re.LastUpdate = Time.timeSinceLevelLoad;
            StartCoroutine( TimeoutRemoteEntity( re ) );

            if( remoteEntityParent != null )
            {
                entityGO.transform.parent = remoteEntityParent.transform;
            }

            return re;
        }

        #region Event Handlers

        /// <summary>
        /// On ESPDU.
        /// </summary>
        /// <param name="es"></param>
        private void OnEntityState( Header h )
        {
            EntityState es = h as EntityState;
            // Does the entity already exist?
            RemoteEntity re;
            if( !RemoteEntities.TryGetValue( es.EntityID.HashCode, out re ) )
            {
                // Create a new entity using the entity map.
                GameObject foundGO = entityMap.GetMatch( es.EntityType );
                if( foundGO != null )
                {
                    re = CreateEntity( foundGO, es );                   
                    Debug.Log( string.Format( "Discovered new entity: Name - {0}, ID - {1}", es.Marking.ASCII, es.EntityID ) );
                }
                else
                {
                    Debug.LogWarning( string.Format( "Could not create object {0}. Not found in entity map.", es.EntityType.ToString() ) );
                    return;
                }
            }
            else
            {
                // Update entity
                re.UpdateEntity( es );
                re.LastUpdate = Time.timeSinceLevelLoad;
            }

            // Has the entity been deactivated? 
            if( !es.Appearance.State )
            {
                RemoveEntity( re, true );
            }
        }
        #endregion Event Handlers
    }        
}