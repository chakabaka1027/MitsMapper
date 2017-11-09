using System;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using DISUnity.DataType;
using System.Text;
using System.Collections.ObjectModel;

namespace DISUnity.DataType
{
    /// <summary>
    /// Holds a collection of VariableParameters and derived classes.    
    /// </summary>
    [Serializable]
    public class VariableParameterCollection : DataTypeBaseSimple
    {
        #region Properties

        #region Private

        // VariableParameters are seperated into a list per derived class due to Unity being unable to serialize complex data types (ScriptableObjects introduce more problems).

        protected List<VariableParameter> variableParameters = new List<VariableParameter>();

        [SerializeField]
        protected List<ArticulatedPart> articulatedParts = new List<ArticulatedPart>();

        [SerializeField]
        protected List<AttachedPart> attachedParts = new List<AttachedPart>();

        #endregion Private

        /// <summary>
        /// Total size of the collection in bytes.
        /// </summary>
        /// <returns></returns>
        /// <value>The length.</value>
        public override int Length
        {
            get
            {
                int len = 0;
                variableParameters.ForEach( o => len += o.Length );
                articulatedParts.ForEach( o => len += o.Length );
                attachedParts.ForEach( o => len += o.Length );
                return len;
            }
        }

        /// <summary>
        /// Contains all VariableParameter records combined into a single list.               
        /// </summary>                
        public ReadOnlyCollection<VariableParameter> Items
        {
            get
            {
                // Generates a combined list.
                List<VariableParameter> varParams = new List<VariableParameter>();                
                varParams.AddRange( variableParameters );
                varParams.AddRange( articulatedParts.Cast<VariableParameter>() );
                varParams.AddRange( attachedParts.Cast<VariableParameter>() );
                return varParams.AsReadOnly();               
            }
        }

        /// <summary>
        /// List of variable parameters. Derived versions are stored in separate lists so they can 
        /// be shown in the Unity inspector (Articulated Parts and Attached Parts).
        /// If you do not need an item to be shown in the inspector then simply add them to this list.
        /// If you would like them to be shonw use the Items set function to have them filtered into 
        /// their correct lists or use the relveant list property.        
        /// </summary>
        public List<VariableParameter> VariableParameters
        {
            get
            {
                return variableParameters;
            }
            set
            {
                isDirty = true;
                variableParameters = value;                
            }
        }

        /// <summary>
        /// Articulated parts collection.
        /// </summary>
        public List<ArticulatedPart> ArticulatedParts
        {
            get
            {
                return articulatedParts;
            }
            set
            {
                isDirty = true;
                articulatedParts = value;                
            }
        }

        /// <summary>
        /// Attached parts collection.
        /// </summary>
        public List<AttachedPart> AttachedParts
        {
            get
            {
                return attachedParts;
            }
            set
            {
                isDirty = true;
                attachedParts = value;
            }
        }

        /// <summary>
        /// Total number of variable parameter records.
        /// </summary>
        public int NumberOfRecords
        {
            get
            {
                return variableParameters.Count + articulatedParts.Count + attachedParts.Count;
            }
        }

        #endregion Properties

        public VariableParameterCollection()
        {
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public VariableParameterCollection( BinaryReader br )
        {
            Decode( br );
        }

        /// <summary>
        /// Clears all variable parameter lists.
        /// </summary>
        public void Clear()
        {
            variableParameters.Clear();
            articulatedParts.Clear();
            attachedParts.Clear();
        }

        /// <summary>
        /// Add a collection of VariableParameters. This list will be filtered and broken into sub lists by type to allow the inspector to display the contents.
        /// </summary>
        /// <param name="items"></param>
        public void AddItems( List<VariableParameter> items )
        {
            isDirty = true;

            variableParameters.Clear();

            // Split the collection.
            foreach( var vp in items )
            {
                if( vp is ArticulatedPart )
                    articulatedParts.Add( vp as ArticulatedPart );
                else if( vp is AttachedPart )
                    attachedParts.Add( vp as AttachedPart );
                else
                    variableParameters.Add( vp );
            }
        }

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="numberOfRecords">Number of records to decode from the stream.</param>
        public virtual void Decode( BinaryReader br, byte numberOfRecords )
        {
            Clear();
            for( int i = 0; i < numberOfRecords; ++i )
            {
                long pos = br.BaseStream.Position; // Save position for peek
                int typ = ( int )br.ReadUInt32();
                br.BaseStream.Position = pos; // Reset 
                
                VariableParameter vp = VariableParameter.FactoryDecodeComplex( typ, br );

                if( vp is ArticulatedPart )
                    articulatedParts.Add( vp as ArticulatedPart );
                else if( vp is AttachedPart )
                    attachedParts.Add( vp as AttachedPart );
                else
                    variableParameters.Add( vp );                
            }

            isDirty = true;                       
        }

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>        
        public override void Decode( BinaryReader br )
        {
            throw new NotImplementedException( "Incorrect Decode function used. Use Decode( BinaryReader br, byte numberOfRecords )" );
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            variableParameters.ForEach( o => o.Encode( bw ) );
            articulatedParts.ForEach( o => o.Encode( bw ) );
            attachedParts.ForEach( o => o.Encode( bw ) );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine( "Variable Parameters:" );
            variableParameters.ForEach( o => sb.Append( o.ToString() ) );
            articulatedParts.ForEach( o => sb.Append( o.ToString() ) );
            attachedParts.ForEach( o => sb.Append( o.ToString() ) );            
            return sb.ToString();            
        }        
        
        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( VariableParameterCollection b )
        {
            if( !variableParameters.SequenceEqual( b.variableParameters ) ) return false;
            if( !articulatedParts.SequenceEqual( b.articulatedParts ) ) return false;
            if( !attachedParts.SequenceEqual( b.attachedParts ) ) return false;      
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( VariableParameterCollection a, VariableParameterCollection b )
        {
            return a.Equals( b );
        }
    }
}