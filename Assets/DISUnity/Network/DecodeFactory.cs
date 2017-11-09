using UnityEngine;
using System.Collections.Generic;
using DISUnity.PDU.EntityInfoInteraction;
using DISUnity.PDU;
using System.IO;
using DISUnity.Network.Filters;
using System;
using DISUnity.DataType.Enums;
using DISUnity.PDU.Warfare;
using Collision = DISUnity.PDU.EntityInfoInteraction.Collision;
using DISUnity.PDU.RadioCommunications;
using DISUnity.PDU.SimulationManagement;
using UnityEngine.Events;
using DISUnity.Events;
using DISUnity.Utils;

namespace DISUnity.Network
{
    /// <summary>
    /// Interface to allow for adding custom decoders for PDU's to the DecodeFactory.
    /// </summary>
    public interface IFactoryDecoder
    {
        Header DecodeBody( Header h, BinaryReader br );
    }

    /// <summary>
    /// Generic version of a custom decoder.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FactoryDecoderCreator<T> : IFactoryDecoder where T : Header, IPduBodyDecoder, new()
    {
        public Header DecodeBody( Header h, BinaryReader br ) 
        {
 	        T pdu = new T();
            pdu.Decode( h, br );
            return pdu;
        }
    }
    
    /// <summary>
    /// Responsible for decoding the data from the network into PDU's.
    /// </summary>
    [AddComponentMenu( "DISUnity/Network/Decode Factory" )]
    public class DecodeFactory : MonoBehaviourSingleton<DecodeFactory>
    {
        #region Properties
        
        #region Private
        
        [Tooltip( "Filters used to determine if a PDU should be processed by the simulation." )]
        protected List<IFactoryFilter> filters = new List<IFactoryFilter>();

        /// <summary>
        /// Pair up a factory pdu decoder with its event.
        /// </summary>
        protected class DecoderAndEventPair
        {
            public IFactoryDecoder pduDecoder;
            public PduEvent pduEvent;

            public DecoderAndEventPair( IFactoryDecoder d, PduEvent e )
            {
                pduDecoder = d;
                pduEvent = e;
            }
        }
                
        protected Dictionary<PDUType, DecoderAndEventPair> factoryDecoders;
        
        #endregion Private   
        
        /// <summary>
        /// Filters applied at various stages to determine if a PDU should be allowed. 
        /// The most common use for this would be a filter that only allows a specific ExerciseID however it could also be used to limit the PDU types etc.
        /// </summary>
        public List<IFactoryFilter> Filters
        {
            get
            {
                return filters;
            }
            set
            {
                filters = value;
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Called when any type of PDU is received on the DIS network.
        /// </summary>                
        public PduEvent anyReceived;  

        /// <summary>
        /// Called when a entity state PDU is received on the DIS network.
        /// </summary>
        public PduEvent entityStateReceived;

        /// <summary>
        /// Called when a fire PDU is received on the DIS network.
        /// </summary>
        public PduEvent fireReceived;

        /// <summary>
        /// Called when a detonation PDU is received on the DIS network.
        /// </summary>
        public PduEvent detonationReceived;

        /// <summary>
        /// Called when a collision PDU is received on the DIS network.
        /// </summary>
        public PduEvent collisionReceived;

        /// <summary>
        /// Called when a signal PDU is received on the DIS network.
        /// </summary>
        public PduEvent signalReceived;

		/// <summary>
		/// Called when an acknowledge PDU is received on the DIS network.
		/// </summary>
        public PduEvent acknowledgeReceived;

		/// <summary>
		/// Called when a comment PDU is received on the DIS network.
		/// </summary>
        public PduEvent commentReceived;

		/// <summary>
		/// Called when a create entity PDU is received on the DIS network.
		/// </summary>
        public PduEvent createEntityReceived;

		/// <summary>
		/// Called when a data PDU is received on the DIS network.
		/// </summary>
        public PduEvent dataReceived;

		/// <summary>
		/// Called when a remove entity PDU is received on the DIS network.
		/// </summary>
        public PduEvent removeEntityReceived;

		/// <summary>
		/// Called when a set data PDU is received on the DIS network.
		/// </summary>
        public PduEvent setDataReceived;

		/// <summary>
		/// Called when a start resume PDU is received on the DIS network.
		/// </summary>
        public PduEvent startResumeReceived;

		/// <summary>
		/// Called when a stop freeze PDU is received on the DIS network.
		/// </summary>
        public PduEvent stopFreezeReceived;    

		/// <summary>
		/// Called when an action request PDU is received on the DIS network.
		/// </summary>
		public PduEvent actionRequestReceived;

		/// <summary>
		/// Called when an action response PDU is received on the DIS network.
		/// </summary>
		public PduEvent actionResponseReceived;

		/// <summary>
		/// Called when a data query PDU is received on the DIS network.
		/// </summary>
		public PduEvent dataQueryReceived;

        #endregion Events
        
        /// <summary>
        /// Init our factory decoder dict.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            // Init our decoders
            factoryDecoders = new Dictionary<PDUType, DecoderAndEventPair>()
            {
                { PDUType.EntityState,    new DecoderAndEventPair( new FactoryDecoderCreator<EntityState>(),    entityStateReceived   ) },
                { PDUType.Fire,           new DecoderAndEventPair( new FactoryDecoderCreator<Fire>(),           fireReceived          ) },
                { PDUType.Detonation,     new DecoderAndEventPair( new FactoryDecoderCreator<Detonation>(),     detonationReceived    ) },
                { PDUType.Collision,      new DecoderAndEventPair( new FactoryDecoderCreator<Collision>(),      collisionReceived     ) },
                { PDUType.Signal,         new DecoderAndEventPair( new FactoryDecoderCreator<Signal>(),         signalReceived        ) },
                { PDUType.Acknowledge,    new DecoderAndEventPair( new FactoryDecoderCreator<Acknowledge>(),    acknowledgeReceived   ) },
                { PDUType.Message,        new DecoderAndEventPair( new FactoryDecoderCreator<Comment>(),        commentReceived       ) },
                { PDUType.CreateEntity,   new DecoderAndEventPair( new FactoryDecoderCreator<CreateEntity>(),   createEntityReceived  ) },
                { PDUType.Data,           new DecoderAndEventPair( new FactoryDecoderCreator<Data>(),           dataReceived          ) },
                { PDUType.RemoveEntity,   new DecoderAndEventPair( new FactoryDecoderCreator<RemoveEntity>(),   removeEntityReceived  ) },
                { PDUType.SetData,        new DecoderAndEventPair( new FactoryDecoderCreator<SetData>(),        setDataReceived       ) },
                { PDUType.StartResume,    new DecoderAndEventPair( new FactoryDecoderCreator<StartResume>(),    startResumeReceived   ) },
                { PDUType.StopFreeze,     new DecoderAndEventPair( new FactoryDecoderCreator<StopFreeze>(),     stopFreezeReceived    ) },
				{ PDUType.ActionRequest,  new DecoderAndEventPair( new FactoryDecoderCreator<ActionRequest>(),  actionRequestReceived ) },
				{ PDUType.ActionResponse, new DecoderAndEventPair( new FactoryDecoderCreator<ActionResponse>(), actionResponseReceived) },
				{ PDUType.DataQuery,      new DecoderAndEventPair( new FactoryDecoderCreator<DataQuery>(),      dataQueryReceived     ) }
            };
        }

        /// <summary>
        /// All PDU of pduType will now be decoded using the supplied decoder.
        /// </summary>
        /// <param name="pduType"></param>
        /// <param name="decoder"></param>
        public void SetPduDecoder( PDUType pduType, IFactoryDecoder decoder )
        {
            DecoderAndEventPair dep;
            if( factoryDecoders.TryGetValue( pduType, out dep ) )
            {
                Debug.Log( "Swapping out default decoder for " + pduType + " with " + decoder.GetType().ToString() );
                dep.pduDecoder = decoder;
            }
            else
            {                
                factoryDecoders.Add( pduType, new DecoderAndEventPair( decoder, null ) );
            }
        }

        /// <summary>
        /// Decodes the data and fires events if required.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual void Decode( byte[] data )
        {
            BinaryReader br = BitConverter.IsLittleEndian ? new BinaryReaderBigEndian( new MemoryStream( data ) ) : new BinaryReader( new MemoryStream( data ) );

            // Data may contain more than 1 PDU, e.g PDU bundles.
            while( ( br.BaseStream.Length - br.BaseStream.Position ) > 0 )
            {
                long currentPos = br.BaseStream.Position;				
                Header h = new Header( br );				

                // Apply filters to ensure this PDU is allowed.
                bool allowPDU = true;
                if( filters != null )
                {
                    foreach( var filter in filters )
                    {
                        if( !filter.OnHeaderDecoded( h ) )
                            allowPDU = false;
                    }
                }			

                if( allowPDU )
                {
                    // Find a decoder for this PDU type.
                    DecoderAndEventPair foundDecoder;
                    if( factoryDecoders.TryGetValue( h.PDUType, out foundDecoder ) )
                    {
                        h = foundDecoder.pduDecoder.DecodeBody( h, br );
                        
                        // Fire PDU specific event if one exists.
                        if( foundDecoder.pduEvent != null )
                            foundDecoder.pduEvent.Invoke( h );
                        
                        // Fire event for all PDU's
                        anyReceived.Invoke( h );                    
                    }
                    else
                    {
                        Debug.LogWarning( "Unknown PDU, could not decode the body. No PDU decoder exists for this type. Header contents:\n" + h.ToString() );
                    }                  
                }
				
				// Move forward in stream, If the header contains an invalid length value then continue from the last byte read.
                if( h.Length != 0 )
                    br.BaseStream.Seek( currentPos + h.Length, SeekOrigin.Begin ); 
            }
        }
    }
}