using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DISUnity.DataType.Enums.SISO_REF_010.SISO_REF_010_2011_1_RC2;
using DISUnity.DataType.Enums;
using Object = UnityEngine.Object;
using System.Xml.Serialization;
using System.IO;
using DISUnity.DataType;

namespace DISUnity.Simulation
{
    /// <summary>
    /// Allows for mapping objects to an entity type.    
    /// </summary>
    [AddComponentMenu( "DISUnity/Simulation/Entity Map" )]
    public class EntityTypeMap : MonoBehaviour
    {
        #region Properties

        #region Private

        [SerializeField]
        private Node root;

        #endregion Private

        [Serializable]
        public class Node
        {
            public string name; // Name/Description
            public int value;   // Enum value
            public List<Node> children = new List<Node>();
            public GameObject obj;

            // Used in the editor to know if this values children are shown.
            public bool isExpanded = false;

            public Node() { }

            public Node( int val, string desc )
            {
                value = val;
                name = desc;           
            }
        }

        /// <summary>
        /// Root node for all mappings. Children represent the kind value.
        /// </summary>
        public Node Root
        {
            get
            {
                return root;
            }
        }

        #endregion Properties

        /// <summary>
        /// Returns an exact or closet match for an entity type.
        /// </summary>
        /// <param name="typ"></param>
        /// <returns></returns>
        public GameObject GetMatch( EntityType typ )
        {
            GameObject match = null;

            int[] types = new int[] { ( int )typ.Kind, 
                                      ( int )typ.Domain,
                                      ( int )typ.Country, 
                                      typ.Category, 
                                      typ.SubCategory,
                                      typ.Specific, 
                                      typ.Extra };

            Node current = root;
            foreach( int val in types )
            {
                current = current.children.Find( o => o.value == val );
                if( current != null )
                {
                    if( current.obj != null )
                    {
                        // Closest match so far
                        match = current.obj;
                    }
                }
                else
                {
                    // Gone as far as we can
                    break;
                }
            }

            return match;
        }
        
        /// <summary>
        /// Find a node based on kind, domain and country. Returns country node.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="domain"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public Node Find( int kind, int domain, int country )
        {
            Node node = null;
            foreach( Node nKind in root.children )
            {
                if( nKind.value == kind )
                {
                    foreach( Node nDomain in nKind.children )
                    {
                        if( nDomain.value == domain )
                        {
                            node = nDomain.children.Find( nCountry => nCountry.value == country );
                            if( node != null )
                            {
                                return node;
                            }
                        }
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Searches for SISO xml file and loads it if found. 
        /// Does not work on all platforms, use Load( string ) instead.
        /// </summary>                
        public void Load()
        {
            #if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_ANDROID || UNITY_IPHONE
            // find the file
            string[] files = Directory.GetFiles( Application.dataPath, "SISO-REF-010.xml", SearchOption.AllDirectories );
            if( files == null || files.Length == 0 )
            {
                throw new Exception( "Could not find file: SISO-REF-010.xml" );
            }
            else
            {
                Load( files[0] );
            }
            #else
            Debug.LogWarning( "Feature is not supported on this platform, use Load( string ) instead." );
            #endif
        }        

        /// <summary>
        /// Loads the SISO xml file 
        /// </summary>                
        public void Load( string file )
        {
            ReadFromXML( file );    
        }

        /// <summary>
        /// Read the SISO_REF_010 xml file to generate a skeleton type structure.
        /// </summary>
        /// <param name="file"></param>
        public void ReadFromXML( string file )
        {
            FileStream f = new FileStream( file, FileMode.Open, FileAccess.Read );
            XmlSerializer ser = new XmlSerializer( typeof( ebv ) );
            ebv e = ser.Deserialize( f ) as ebv;

            // Find the cet table. should be at pos 35
            cet_t cet;
            if( e.Items[35] is cet_t )
            {
                cet = e.Items[35] as cet_t;
            }
            else
            {
                // Try and find it, it may have moved..
                generictable_t get = e.Items.First( o => o.GetType() == typeof( cet_t ) );
                if( get == null )
                {
                    throw new Exception( "Failed to process enum file. Could not find CET." );
                }
                cet = get as cet_t;
            }

            root = new Node();

            // Create kind, domain and country nodes using internal enums.
            foreach( EntityKind kind in Enum.GetValues( typeof( EntityKind ) ) )
            {
                Node k = new Node( ( int )kind, kind.ToString() );
                root.children.Add( k );
                foreach( EntityDomain domain in Enum.GetValues( typeof( EntityDomain ) ) )
                {
                    Node d = new Node( ( int )domain, domain.ToString() );
                    k.children.Add( d );
                    foreach( Country country in Enum.GetValues( typeof( Country ) ) )
                    {
                        d.children.Add( new Node( ( int )country, country.ToString() ) );
                    }
                }
            }

            // The first 3 values kind, domain and country are all in a single node, we want a value per node so we will need to go through them and split them out.
            foreach( entity_t ent in cet.entity )
            {
                // find the node
                Node n = Find( int.Parse( ent.kind ), int.Parse( ent.domain ), int.Parse( ent.country ) );

                if( n != null )
                {
                    foreach( genericentrydescription_t ged1 in ent.Items1 )
                    {
                        category_t cat = ged1 as category_t;
                        Node catNode = new Node( cat.value, cat.description );
                        n.children.Add( catNode );

                        // sub-cat
                        if( cat.Items1 != null )
                        {
                            foreach( genericentrydescription_t ged2 in cat.Items1 )
                            {
                                if( ged2 is subcategory_t )
                                {
                                    subcategory_t subcat = ged2 as subcategory_t;
                                    Node subcatNode = new Node( subcat.value, subcat.description );
                                    catNode.children.Add( subcatNode );

                                    // specific
                                    if( subcat.Items1 != null )
                                    {
                                        foreach( genericentrydescription_t ged3 in subcat.Items1 )
                                        {
                                            if( ged3 is specific_t )
                                            {
                                                specific_t spec = ged3 as specific_t;
                                                Node specNode = new Node( spec.value, spec.description );
                                                subcatNode.children.Add( specNode );

                                                // extra
                                                if( spec.Items1 != null )
                                                {
                                                    foreach( genericentrydescription_t ged4 in spec.Items1 )
                                                    {
                                                        extra_t ext = ged4 as extra_t;
                                                        specNode.children.Add( new Node( ext.value, ext.description ) );
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

