using UnityEngine;
using System.IO;
using System;

namespace DISUnity.DataType
{
    /// <summary>
    /// Base class for all simple DISUnity objects.
    /// </summary>
    public abstract class DataTypeBaseSimple 
    {
        #region Properties
            
        /// <summary>
        /// <see cref="IsDirty"/>
        /// </summary>
        protected bool isDirty = true;

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        public abstract int Length
        {
            get;            
        }

        /// <summary>
        /// Have any changes been made to the internal data?
        /// This data can not be set to false however it can be set to true by using <c>SetDirty</c>.
        /// Value is set to true when <see cref="Encode"/> is called.        
        /// </summary>
        /// <remarks>
        /// This value is not used much at the moment however it will be used in the future to 
        /// enhance performance by only encoding new data and reusing previously sent PDU data.
        /// </remarks>
        public virtual bool IsDirty
        {
            get
            {
                return isDirty;
            }
        }

        #endregion Properties

        /// <summary>
        /// Forces the <see cref="IsDirty"/> flag to true. 
        /// </summary>
        public void SetDirty()
        {            
            isDirty = true;
        }

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        public abstract void Decode( BinaryReader br );

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public abstract void Encode( BinaryWriter bw );       
    }
}
