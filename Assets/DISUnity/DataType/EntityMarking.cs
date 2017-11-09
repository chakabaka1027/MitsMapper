using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using DISUnity.DataType;
using System.Text;
using DISUnity.DataType.Enums.EntityMarking;
using System.Linq;

namespace DISUnity.DataType
{
    /// <summary>
    /// Character set used in the marking and the string of characters to be interpreted for display.    
    /// </summary>
    /// <remarks>
    /// Currently ASCII is the only marking set with support however you can access the Characters directly to set/get using the other 2 types.
    /// </remarks>
    /// <size>12 bytes</size>
    [Serializable]
    public class EntityMarking : DataTypeBaseSimple
    {
        #region Properties

        #region Private

        [SerializeField]
        private CharacterSet mcs = CharacterSet.ASCII; // Default

        [SerializeField]
        private byte[] characters = new byte[11];

        #endregion Private

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        public override int Length
        {
            get
            {
                return 12;
            }
        }

        /// <summary>
        /// Type of marking set used.
        /// </summary>
        public CharacterSet MarkingSet
        {
            get
            {
                return mcs;
            }
            set
            {
                isDirty = true;
                mcs = value;
            }
        }

        /// <summary>
        /// Raw characters, 11 digits long.
        /// </summary>
        public byte[] Characters
        {
            get
            {
                return characters;
            }
            set
            {
                isDirty = true;
                Array.Clear( characters, 0, 11 );                  
                Array.Copy( value, characters, Mathf.Min( value.Length, 11 ) );           
            }
        }

        /// <summary>
        /// Get/Set using an ASCII marking set. Will automatically set the character set to ASCII.
        /// Should be no more than 11 characters in length, extra characters will be ignored.
        /// </summary>
        public string ASCII
        {
            get
            {
                return Encoding.ASCII.GetString( characters );        
            }
            set
            {
                MarkingSet = CharacterSet.ASCII;
				
				// Truncate or pad?
				string s = value;
				if( s.Length > 11 )
				{
					s = s.Substring( 0, 11 );
				}
				else if( s.Length != 11 )
				{
                    s = s.PadRight( 11 );
				}
				
                Characters = Encoding.ASCII.GetBytes( s );
            }            
        }

        #endregion Properties

        public EntityMarking(){ }

        /// <summary>
        /// Use ASCII Marking Set.
        /// </summary>
        /// <param name="ASCII">Should be no more than 11 char in length, extra chars will be ignored.</param>
        public EntityMarking( string mark )
        {
            ASCII = mark;
        }

        /// <summary>
        /// Use ASCII Marking Set
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="chars">Array of characters, should be no more than 12 bytes.</param>        
        public EntityMarking( CharacterSet cs, byte[] chars )
        {
            mcs = cs;
            Characters = chars;
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public EntityMarking( BinaryReader br )
        {
            Decode( br );
        }

        #region DataTypeBase

        /// <summary>
        /// Decode network data.
        /// </summary>
        /// <param name="br"></param>
        public override void Decode( BinaryReader br )
        {
            isDirty = true;
            mcs = ( CharacterSet )br.ReadByte();
            characters = br.ReadBytes( 11 );    
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {
            bw.Write( ( byte )mcs );
            bw.Write( characters );
            isDirty = false;
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Format( "Marking({0})", MarkingSet );
            if( MarkingSet == CharacterSet.ASCII )
            {
                return string.Format( "{0} - \"{1}\"", s, ASCII ); 
            }
            
            return s;
        }
        
        #endregion DataTypeBase
        
        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( EntityMarking b )
        {
            if( mcs != b.mcs                              ) return false;
            if( !Enumerable.SequenceEqual( characters, b.characters ) ) return false;            
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( EntityMarking a, EntityMarking b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}