using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using DISUnity.Resources;
using DISUnity.Editor.Attributes;

namespace DISUnity.Editor.DataType
{
    [CustomPropertyDrawer( typeof( EntityCapabilities ) )]
    public class EntityCapabilitiesPropertyDrawer : LabelPropertyDrawer
    {
        #region Properties

        private SerializedProperty capabilities;

        private GUIContent ammoLabel;
        private GUIContent fuelLabel;
        private GUIContent recoveryLabel;
        private GUIContent repairLabel;
        private GUIContent adsbLabel;

        private EntityCapabilities src;

        #endregion Properties

        /// <summary>
        /// Setup SerializedProperty's and any labels used, tools tips etc.
        /// </summary>
        /// <param name="property"></param>
        private void Init( SerializedProperty property )
        {
            capabilities = property.FindPropertyRelative( "capabilities" );
            src = new EntityCapabilities( capabilities.intValue );

            // Labels
            ammoLabel = new GUIContent( "Ammunition Supply", Tooltips.AmmoSupply );
            fuelLabel = new GUIContent( "Fuel Supply", Tooltips.FuelSupply );
            recoveryLabel = new GUIContent( "Recovery Service", Tooltips.RecoveryService );
            repairLabel = new GUIContent( "Repair Service", Tooltips.RepairService );
            adsbLabel = new GUIContent( "ADS-B", Tooltips.ADSB );   
        }

        /// <summary>
        /// Current height of this property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            return EditorGUIUtility.singleLineHeight +
                ( property.isExpanded ? EditorGUIUtility.singleLineHeight * 5 : 0 ) + // Fields
                ( property.isExpanded && EditorSettings.AdvancedMode ? EditorGUIUtility.singleLineHeight : 0 ); // Advanced mode - bit field.
        }

        /// <summary>
        /// Draw property
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            label = GetLabel( label );

            Init( property ); 

            EditorGUI.BeginProperty( position, label, property );
            position.height = EditorGUIUtility.singleLineHeight;

            // Label        
            property.isExpanded = EditorGUI.Foldout( position, property.isExpanded, label );
            position.y += EditorGUIUtility.singleLineHeight;

            if( property.isExpanded )
            {
                EditorGUI.BeginChangeCheck();

                EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

                if( EditorSettings.AdvancedMode )
                {
                    src.Capabilities = EditorGUI.IntField( position, "Bit Field", src.Capabilities );
                    position.y += EditorGUIUtility.singleLineHeight;
                }

                src.AmmunitionSupply = EditorGUI.Toggle( position, ammoLabel, src.AmmunitionSupply );
                position.y += EditorGUIUtility.singleLineHeight;
                src.FuelSupply = EditorGUI.Toggle( position, fuelLabel, src.FuelSupply );
                position.y += EditorGUIUtility.singleLineHeight;
                src.RecoveryService = EditorGUI.Toggle( position, recoveryLabel, src.RecoveryService );
                position.y += EditorGUIUtility.singleLineHeight;
                src.RepairService = EditorGUI.Toggle( position, repairLabel, src.RepairService );
                position.y += EditorGUIUtility.singleLineHeight;
                src.ADSB = EditorGUI.Toggle( position, adsbLabel, src.ADSB );

                if( EditorGUI.EndChangeCheck() )
                {
                    capabilities.intValue = src.Capabilities;
                }
            }
            EditorGUI.EndProperty();
        }
    }
}