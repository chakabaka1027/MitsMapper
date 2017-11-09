using System;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using DISUnity.DataType;
using System.Text;
using System.Reflection;

namespace DISUnity.DataType
{
    /// <summary>
    /// Base class for all complex DISUnity objects. 
    /// Any class/data type that needs to support custom versions when decoding needs to inherit from this,
    /// by doing do DISUnity will be aware of the custom class and use it when decoding when necessary.        
    [Serializable]
    public abstract class DataTypeBaseComplex<T> : DataTypeBaseSimple
    {
        #region Properties
            
        // Custom decoders. Discovered using reflection at runtime
        protected static Dictionary<int, MethodInfo> customDecoders;
        protected static MethodInfo defaultDecoder;

        /// <summary>
        /// The enum values this class represents.
        /// Generally a class will only represent 1 however you may want to support more.
        /// This class will be used to represent any data types that have these enum types.
        /// Do 'public static new int[] ConcreteTypeEnum' to override this in your derived types.
        /// A value of null means the type is the default(usually the base class).
        /// </summary>
        public static int[] ConcreteTypeEnums
        {
            get
            {                
                return null;
            }
        }

        #endregion Properties

        /// <summary>
        /// Register a custom class to use for decoding from type T.
        /// You should not need to do this manually, it is determined at runtime using reflection.
        /// Make sure this class meets the required criteria:
        ///     Must be of type T or inherit from type T
        ///     Must have a static property called ConcreteTypeEnums
        ///     Must have a static function called FactoryDecode
        /// </summary>
        /// <param name="t"></param>
        public static void RegisterClass( Type t )
        {
            if( customDecoders == null )
            {
                customDecoders = new Dictionary<int, MethodInfo>();
            }

            // Check type
            if( !t.IsSubclassOf( typeof( T ) ) && t != typeof( T ) )
            {
                Debug.LogWarning( string.Format( "RegisterClass: Class must be or inherit from type '{0}'", typeof( T ) ) );
                return;
            }

            // Check for required property
            PropertyInfo pi = t.GetProperty( "ConcreteTypeEnums" );

            if( pi == null )
            {
                Debug.LogWarning( string.Format( "RegisterClass: Can not find property 'ConcreteTypeEnums' in '{0}', a custom class must implement this property so we know what type to use this class for.", t ) );
                return;
            }

            // Check for required function
            MethodInfo mi = t.GetMethod( "FactoryDecode" );
            if( mi == null )
            {
                Debug.LogWarning( string.Format( "RegisterClass: Can not find method 'FactoryDecode' in '{0}', a custom class must implement this property so we can decode data types that belong to it.", t ) );
                return;
            }

            // Get the types this class supports and add each to our dict.
            int[] i = null;
            i = ( int[] )pi.GetValue( null, null );
            if( i == null )
            {
                if( defaultDecoder != null )
                {
                    Debug.LogWarning( string.Format( "Replacing Default type '{0}' with '{1}'", defaultDecoder.DeclaringType.ToString(), t ) );
                }

                defaultDecoder = mi;
            }
            else
            {
                i.ToList().ForEach( o => customDecoders.Add( o, mi ) );
            }
        }

        /// <summary>
        /// The types that this collection supports.
        /// Returns all types that derive from class T including class T itself.
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetSupportedTypes()
        {
            // Find all implementations of class T including the original if it is not abstract.
            Type typeToFind = typeof( T );
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            List<Type> found = new List<Type>();

            foreach( Type typ in types )
            {
                // Register each of the found classes
                if( typ.IsSubclassOf( typeToFind ) || ( !typeToFind.IsAbstract && typ == typeToFind ) )
                {
                    found.Add( typ );
                }
            }

            return found;
        }

        /// <summary>
        /// Finds all implementations of T at runtime, using the ConcreteTypeEnums property field the different types will be used to decode.
        /// </summary>
        public static void InitCustomDecoders()
        {
            customDecoders = new Dictionary<int, MethodInfo>();

            List<Type> supportedTypes = GetSupportedTypes();

            // Record each class found for debug purposes
            StringBuilder sb = new StringBuilder();
            sb.AppendLine( "Found Implementations Of " + typeof( T ).ToString() );
            foreach( Type typ in supportedTypes )
            {
                RegisterClass( typ );
                sb.AppendLine( "\t" + typ );
            }

            Debug.Log( sb.ToString() );
        }

        /// <summary>
        /// Attempts to find a decoder for the complex type, if a specific can not be found then the default <see cref="defaultDecoder"/> will be used if one exists.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="br"></param>
        /// <returns>New instance of decoded data.</returns>
        /// <exception cref="Exception">
        /// If no decoder can be found and the default is null.
        /// </exception>
        public static T FactoryDecodeComplex( int type, BinaryReader br )
        {
            if( customDecoders == null )
            {
                InitCustomDecoders();
            }

            // Get the decoder or use default
            MethodInfo mi = null;
            if( !customDecoders.TryGetValue( type, out mi ) )
            {
                mi = defaultDecoder;
            }

            // Error check
            if( mi == null )
            {
                // Should never happen
                throw new Exception( "Factory decode failed to find a decoder and we have no default." );
            }

            // Invoke the decoder
            return ( T )mi.Invoke( null, new object[] { type, br } );
        }
    }
}