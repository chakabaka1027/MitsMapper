using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using DISUnity.DataType.Enums;

namespace DISUnity.Editor.Settings
{
    public class EditorSettings
    {
        #region Properties

        /// <summary>
        /// Editor advanced mode shows more options for editing. Generally fields that would not normally need to be set by the user.
        /// </summary>
        public static bool AdvancedMode
        {
            get
            {
                return EditorPrefs.GetBool( "DISUnity-AdvancedMode", false );
            }
            set
            {
                EditorPrefs.SetBool( "DISUnity-AdvancedMode", value );
            }
        }

        /// <summary>
        /// The maximun version of DIS supported. 
        /// This adds a symbol to the build options called DIS_VERSION_X where X is the defined version.
        /// E.G If ProtocolVersion 5 was defined then DIS 6 & 7 features would be disabled. This is ideal if you want to support a specific version of DIS and dont
        /// want to risk using features that may not be supported.
        /// Supported values are 5, 6, and 7.
        /// </summary>
        public static ProtocolVersion SupportedProtocolVersion
        {
            get
            {
                #if DIS_VERSION_5
                return ProtocolVersion.IEEE_1278_1_1995;
                #elif DIS_VERSION_6
                return ProtocolVersion.IEEE_1278_1A_1998;
                #elif DIS_VERSION_7
                return ProtocolVersion.IEEE_1278_1_2012;
                #else
                return ProtocolVersion.IEEE_1278_1_2012;                // Default to 7.
                #endif
            }
            set
            {
                if( value != ProtocolVersion.IEEE_1278_1_1995 && 
                    value != ProtocolVersion.IEEE_1278_1A_1998 &&
                    value != ProtocolVersion.IEEE_1278_1_2012 )
                {
                    Debug.LogWarning( "SupportedProtocolVersion can only be set to 5, 6 or 7." );
                    return;
                }
                
                // Set the define for every type of build
                foreach( BuildTargetGroup btg in Enum.GetValues( typeof( BuildTargetGroup ) ) )
                {                    
                    string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup( btg );
                    currentDefines = currentDefines.Replace( ';', ' ' );

                    // Remove our defines but leave any others the user may have added
                    currentDefines = currentDefines.Replace( "DIS_VERSION_5", string.Empty );
                    currentDefines = currentDefines.Replace( "DIS_VERSION_6", string.Empty );
                    currentDefines = currentDefines.Replace( "DIS_VERSION_7", string.Empty );                    

                    PlayerSettings.SetScriptingDefineSymbolsForGroup( btg, string.Format( "DIS_VERSION_{0} {1}",( int ) value, currentDefines ) );
                }     
            }
        }

        #endregion Properties

        /// <summary>
        /// Menu item to Enable advanced mode.
        /// </summary>
        [MenuItem( "DISUnity/Advanced Mode/Enable" )]
        private static void Enable()
        {
            EditorSettings.AdvancedMode = true;
            
            // Force refresh of the inspector
            if( Selection.activeObject != null )
            {
                EditorUtility.SetDirty( Selection.activeObject );
            }            
        }

        /// <summary>
        /// Validates menu item.
        /// </summary>
        /// <returns></returns>
        [MenuItem( "DISUnity/Advanced Mode/Enable", true )]
        private static bool IsAdvancedModeEnabled()
        {
            return !EditorSettings.AdvancedMode;
        }

        /// <summary>
        /// Menu item to disable advanced mode.
        /// </summary>
        [MenuItem( "DISUnity/Advanced Mode/Disable" )]
        private static void Disable()
        {
            EditorSettings.AdvancedMode = false;

            // Force refresh of the inspector
            if( Selection.activeObject != null )
            {
                EditorUtility.SetDirty( Selection.activeObject );
            }
        }

        /// <summary>
        /// Validates menu item.
        /// </summary>
        /// <returns></returns>
        [MenuItem( "DISUnity/Advanced Mode/Disable", true )]
        private static bool IsAdvancedModeDisabled()
        {
            return EditorSettings.AdvancedMode;
        }
                
        /// <summary>
        /// Set protocol version to 5
        /// </summary>
        [MenuItem( "DISUnity/DIS Protocol Version/DIS 5" )]
        private static void SetProtocolVersion5()
        {
            if( EditorUtility.DisplayDialog( "Change Supported Protocol Version?", 
                                             "Are you sure you wish to change protocol version? Any features that are using a higher protocol version will no longer be supported.", 
                                             "Change Version", 
                                             "Cancel" ) )
            {
                SupportedProtocolVersion = ProtocolVersion.IEEE_1278_1_1995;
            }
        }

        /// <summary>
        /// Validates protocol option.
        /// </summary>
        /// <returns></returns>
        [MenuItem( "DISUnity/DIS Protocol Version/DIS 5", true )]
        private static bool IsProtocolVersion5()
        {
            return SupportedProtocolVersion == ProtocolVersion.IEEE_1278_1_1995 ? false : true;
        }

        /// <summary>
        /// Set protocol version to 5
        /// </summary>
        [MenuItem( "DISUnity/DIS Protocol Version/DIS 6" )]
        private static void SetProtocolVersion6()
        {
            if( EditorUtility.DisplayDialog( "Change Supported Protocol Version?",
                                 "Are you sure you wish to change protocol version? Any features that are using a higher protocol version will no longer be supported.",
                                 "Change Version",
                                 "Cancel" ) )
            {
                SupportedProtocolVersion = ProtocolVersion.IEEE_1278_1A_1998;
            }
        }

        /// <summary>
        /// Validates protocol option.
        /// </summary>
        /// <returns></returns>
        [MenuItem( "DISUnity/DIS Protocol Version/DIS 6", true )]
        private static bool IsProtocolVersion6()
        {
            return SupportedProtocolVersion == ProtocolVersion.IEEE_1278_1A_1998 ? false : true;
        }

        /// <summary>
        /// Set protocol version to 5
        /// </summary>
        [MenuItem( "DISUnity/DIS Protocol Version/DIS 7" )]
        private static void SetProtocolVersion7()
        {
            SupportedProtocolVersion = ProtocolVersion.IEEE_1278_1_2012;
        }

        /// <summary>
        /// Validates protocol option.
        /// </summary>
        /// <returns></returns>
        [MenuItem( "DISUnity/DIS Protocol Version/DIS 7", true )]
        private static bool IsProtocolVersion7()
        {
            return SupportedProtocolVersion == ProtocolVersion.IEEE_1278_1_2012 ? false : true;
        }
    }
}