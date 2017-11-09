using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DISUnity.Simulation.Managers;

namespace DISUnity.Simulation.Overlay
{
    /// <summary>
    /// Draws symbols to represent entities. E.G 2525B, APP-6A etc    
    /// </summary>
    public abstract class SymbolDrawer : MonoBehaviour
    {
        #region Properties

        #region Private

        [SerializeField]
        private Vector2 symbolSize = new Vector2( 32, 32 );

        [SerializeField]
        private Texture2D unknownSymbol;

        /// <summary>
        /// Stores entity and associated symbol.
        /// </summary>
        private class SymbolInfo
        {
            public Entity entity;
            public Texture2D symbol;
        }
        private List<SymbolInfo> symbolsToDraw = new List<SymbolInfo>();       

        #endregion Private

        /// <summary>
        /// Size of symbol on screen in pixels.
        /// </summary>
        public Vector2 SymbolSize
        {
            get
            {
                return symbolSize;
            }
            set
            {
                symbolSize = value;
            }
        }

        /// <summary>
        /// Symbol to use if one can not be found.
        /// </summary>
        public Texture2D UnknownSymbol
        {
            get
            {
                return unknownSymbol;
            }
            set
            {
                unknownSymbol = value;
            }
        }
        
        #endregion Properties

        /// <summary>
        /// Init. Subscribe to events.
        /// </summary>
        protected virtual void Start()
        {
            // Subscribe to new entity events.
            EntityManager.Instance.newEntity.AddListener( NewEntity );            
            
            // Subscribe to entity removed events
            EntityManager.Instance.removeEntity.AddListener( RemoveEntity );            
        }

        /// <summary>
        /// Adds new entity to symbols to draw
        /// </summary>
        /// <param name="e"></param>
        private void NewEntity( Entity e )
        {
            SymbolInfo si = new SymbolInfo();
            si.entity = e;
            
            // Get texture
            si.symbol = GetSymbol( e );
            if( si.symbol == null )si.symbol = unknownSymbol;
           
            if( si.symbol != null ) symbolsToDraw.Add( si );
            else Debug.LogWarning( string.Format( "No symbol found for {0}, ignoring this entity", e.State.Marking.ASCII ) );            
        }

        /// <summary>
        /// Remove an entity from our symbol drawing list.
        /// </summary>
        /// <param name="e"></param>
        private void RemoveEntity( Entity e )
        {
            for( int i = 0; i < symbolsToDraw.Count; ++i )
            {
                if( symbolsToDraw[i].entity == e )
                {
                    symbolsToDraw.RemoveAt( i );
                    return;
                }
            }

            Debug.LogWarning( string.Format( "Could not remove entity {0}, it is not known.", e.State.Marking.ASCII ) );
        }

        /// <summary>
        /// Return a symbol for this type of entity. The type can be found in e.State.EntityType.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected abstract Texture2D GetSymbol( Entity e );

        /// <summary>
        /// Draws the symbols
        /// </summary>
        protected virtual void OnGUI()
        {
            Camera cam = Camera.main;
            Rect rect = new Rect( 0, 0, symbolSize.x, symbolSize.y );

            for( int i = 0; i < symbolsToDraw.Count; ++i )
            {
                Vector3 screenPos = cam.WorldToScreenPoint( symbolsToDraw[i].entity.transform.position );
                rect.x = screenPos.x;
                rect.y = screenPos.y;

                GUI.DrawTexture( rect, symbolsToDraw[i].symbol );
            }
        }
    }
}