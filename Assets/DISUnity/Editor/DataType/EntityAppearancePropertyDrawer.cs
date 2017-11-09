using UnityEngine;
using UnityEditor;
using System.Collections;
using DISUnity.DataType;
using EditorSettings = DISUnity.Editor.Settings.EditorSettings;
using DISUnity.DataType.Enums;
using DISUnity.Resources;
using DISUnity.Editor.Attributes;
using System.Reflection;
using DISUnity.DataType.Enums.Apperance;

// TODO: Add support for no entity type and unknown types

namespace DISUnity.Editor.DataType
{
    [CustomPropertyDrawer( typeof( EntityAppearance ) )]
    public class EntityAppearancePropertyDrawer : LabelPropertyDrawer
    {
        #region Properties

        private SerializedProperty kind;
        private SerializedProperty domain;
        private SerializedProperty category;

        private int kindDefault = 0;
        private int domainDefault = 0;
        private int categoryDefault = 0;

        // Supported appearance types
        private AppearanceTypes selectedAppearanceType;
        private enum AppearanceTypes
        {
            Unknown,
            Air,
            Cultural,
            Environmental,
            GuidedMunition,
            Land,
            Lifeform,
            Sensor,
            Space,
            Subsurface,
            Surface
        };

        /// <summary>
        /// Have we found an entity type instance to determine the appearance? 
        /// </summary>
        private bool FoundEntityType
        {
            get
            {
                if( kind == null || domain == null || category == null )
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Kind value for appearance
        /// </summary>
        private int Kind
        {
            get
            {
                if( kind != null )
                {
                    kindDefault = kind.intValue;
                }

                return kindDefault;
            }
        }

        /// <summary>
        /// Domain value for appearance
        /// </summary>
        private int Domain
        {
            get
            {
                if( domain != null )
                {
                    domainDefault = domain.intValue;
                }

                return domainDefault;
            }
        }

        /// <summary>
        /// Category value for appearance
        /// </summary>
        private int Category
        {
            get
            {
                if( category != null )
                {
                    categoryDefault = category.intValue;
                }

                return categoryDefault;
            }
        }

        private SerializedProperty appearance;

        private EntityAppearance tmpAppearance;

        #endregion Properties

        /// <summary>
        /// Draw appearance options for air entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawAir( Rect position )
        {
            tmpAppearance.PaintScheme = ( PaintScheme )EditorGUI.EnumPopup( position, "Paint Scheme", tmpAppearance.PaintScheme );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MobilityKill = EditorGUI.Toggle( position, "Mobility Kill", tmpAppearance.MobilityKill );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Damage = ( Damage )EditorGUI.EnumPopup( position, "Damage", tmpAppearance.Damage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Smoke = ( Smoke )EditorGUI.EnumPopup( position, "Smoke", tmpAppearance.Smoke );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.TrailingEffect = ( TrailingEffect )EditorGUI.EnumPopup( position, "Trailing Effect", tmpAppearance.TrailingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.CanopyState = ( CanopyState )EditorGUI.EnumPopup( position, "Canopy", tmpAppearance.CanopyState );
            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField( position, "Lights" );
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.indentLevel++;
            Color defaultCol = GUI.backgroundColor;

            GUI.backgroundColor = tmpAppearance.AntiCollisionLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Anti-Collision", false ) )
            {
                tmpAppearance.AntiCollisionLights = !tmpAppearance.AntiCollisionLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.FormationLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Formation", false ) )
            {
                tmpAppearance.FormationLights = !tmpAppearance.FormationLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.InteriorLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Interior", false ) )
            {
                tmpAppearance.InteriorLights = !tmpAppearance.InteriorLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.LandingLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Landing", false ) )
            {
                tmpAppearance.LandingLights = !tmpAppearance.LandingLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.NavigationLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Navigation", false ) )
            {
                tmpAppearance.NavigationLights = !tmpAppearance.NavigationLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.SpotLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Spot", false ) )
            {
                tmpAppearance.SpotLights = !tmpAppearance.SpotLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = defaultCol;
            EditorGUI.indentLevel--;

            tmpAppearance.FlamingEffect = EditorGUI.Toggle( position, "Flaming Effect", tmpAppearance.FlamingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Afterburner = EditorGUI.Toggle( position, "Afterburner", tmpAppearance.Afterburner );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.PowerPlant = EditorGUI.Toggle( position, new GUIContent( "Power Plant", "Engine on?" ), tmpAppearance.PowerPlant );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Draw appearance options for cultural entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawCultural( Rect position )
        {
            tmpAppearance.Damage = ( Damage )EditorGUI.EnumPopup( position, "Damage", tmpAppearance.Damage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Smoke = ( Smoke )EditorGUI.EnumPopup( position, "Smoke", tmpAppearance.Smoke );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.FlamingEffect = EditorGUI.Toggle( position, "Flaming Effect", tmpAppearance.FlamingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.InternalHeat = EditorGUI.Toggle( position, "Internal Heat", tmpAppearance.InternalHeat );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField( position, "Lights" );
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.indentLevel++;
            Color defaultCol = GUI.backgroundColor;

            GUI.backgroundColor = tmpAppearance.ExteriorLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Exterior", false ) )
            {
                tmpAppearance.ExteriorLights = !tmpAppearance.ExteriorLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.InteriorLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Interior", false ) )
            {
                tmpAppearance.InteriorLights = !tmpAppearance.InteriorLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = defaultCol;
            EditorGUI.indentLevel--;

            tmpAppearance.MaskedCloaked = EditorGUI.Toggle( position, "Masked/Cloaked", tmpAppearance.MaskedCloaked );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Draw appearance options for environmental entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawEnvironmentals( Rect position )
        {
            tmpAppearance.Density = ( Density )EditorGUI.EnumPopup( position, "Density", tmpAppearance.Density );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MaskedCloaked = EditorGUI.Toggle( position, "Masked/Cloaked", tmpAppearance.MaskedCloaked );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Draw appearance options for guided munitions entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawGuidedMunitions( Rect position )
        {
            tmpAppearance.Damage = ( Damage )EditorGUI.EnumPopup( position, "Damage", tmpAppearance.Damage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Smoke = ( Smoke )EditorGUI.EnumPopup( position, "Smoke", tmpAppearance.Smoke );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.TrailingEffect = ( TrailingEffect )EditorGUI.EnumPopup( position, "Trailing Effect", tmpAppearance.TrailingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.FlamingEffect = EditorGUI.Toggle( position, "Flaming Effect", tmpAppearance.FlamingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.LaunchFlash = EditorGUI.Toggle( position, "Launch Flash", tmpAppearance.LaunchFlash );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MaskedCloaked = EditorGUI.Toggle( position, "Masked/Cloaked", tmpAppearance.MaskedCloaked );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Draw appearance options for land entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawLand( Rect position )
        {
            tmpAppearance.PaintScheme = ( PaintScheme )EditorGUI.EnumPopup( position, "Paint Scheme", tmpAppearance.PaintScheme );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MobilityKill = EditorGUI.Toggle( position, "Mobility Kill", tmpAppearance.MobilityKill );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.FirePower = EditorGUI.Toggle( position, "Fire Power Kills", tmpAppearance.FirePower );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Damage = ( Damage )EditorGUI.EnumPopup( position, "Damage", tmpAppearance.Damage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Smoke = ( Smoke )EditorGUI.EnumPopup( position, "Smoke", tmpAppearance.Smoke );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.TrailingEffect = ( TrailingEffect )EditorGUI.EnumPopup( position, "Trailing Effect", tmpAppearance.TrailingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.HatchState = ( HatchState )EditorGUI.EnumPopup( position, "Hatch", tmpAppearance.HatchState );
            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField( position, "Lights" );
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.indentLevel++;
            Color defaultCol = GUI.backgroundColor;

            GUI.backgroundColor = tmpAppearance.BlackoutLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Blackout", false ) )
            {
                tmpAppearance.BlackoutLights = !tmpAppearance.BlackoutLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.BlackoutBrakeLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Blackout Brake", false ) )
            {
                tmpAppearance.BlackoutBrakeLights = !tmpAppearance.BlackoutBrakeLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.BrakeLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Brake", false ) )
            {
                tmpAppearance.BrakeLights = !tmpAppearance.BrakeLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.HeadLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Head", false ) )
            {
                tmpAppearance.HeadLights = !tmpAppearance.HeadLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.InteriorLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Interior", false ) )
            {
                tmpAppearance.InteriorLights = !tmpAppearance.InteriorLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.SpotLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Spot", false ) )
            {
                tmpAppearance.SpotLights = !tmpAppearance.SpotLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.TailLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Tail", false ) )
            {
                tmpAppearance.TailLights = !tmpAppearance.TailLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = defaultCol;
            EditorGUI.indentLevel--;

            tmpAppearance.FlamingEffect = EditorGUI.Toggle( position, "Flaming Effect", tmpAppearance.FlamingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.LauncherRaised = EditorGUI.Toggle( position, "Launcher Raised", tmpAppearance.LauncherRaised );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Camouflage = ( Camouflage )EditorGUI.EnumPopup( position, "Camouflage", tmpAppearance.Camouflage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Concealed = EditorGUI.Toggle( position, "Concealed", tmpAppearance.Concealed );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.PowerPlant = EditorGUI.Toggle( position, new GUIContent( "Power Plant", "Engine on?" ), tmpAppearance.PowerPlant );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Tent = EditorGUI.Toggle( position, "Tent Raised", tmpAppearance.Tent );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Ramp = EditorGUI.Toggle( position, "Ramp Down", tmpAppearance.Ramp );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Surrendered = EditorGUI.Toggle( position, new GUIContent( "Surrendered", "Has the vehicle occupant surrendered?" ), tmpAppearance.Surrendered );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MaskedCloaked = EditorGUI.Toggle( position, "Masked/Cloaked", tmpAppearance.MaskedCloaked );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Draw appearance options for lifeform entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawLifeform( Rect position )
        {
            tmpAppearance.PaintScheme = ( PaintScheme )EditorGUI.EnumPopup( position, "Paint Scheme", tmpAppearance.PaintScheme );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Damage = ( Damage )EditorGUI.EnumPopup( position, "Damage", tmpAppearance.Damage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Compliance = ( Compliance )EditorGUI.EnumPopup( position, "Compliance", tmpAppearance.Compliance );
            position.y += EditorGUIUtility.singleLineHeight;

            Color defaultCol = GUI.backgroundColor;
            GUI.backgroundColor = tmpAppearance.Flashlight ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Flashlight", false ) )
            {
                tmpAppearance.Flashlight = !tmpAppearance.Flashlight;
            }
            GUI.backgroundColor = defaultCol;
            position.y += EditorGUIUtility.singleLineHeight;

            tmpAppearance.LifeformState = ( LifeformState )EditorGUI.EnumPopup( position, "Lifeform State", tmpAppearance.LifeformState );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.LifeformWeaponPrimary = ( LifeformWeapon )EditorGUI.EnumPopup( position, "Weapon - Primary", tmpAppearance.LifeformWeaponPrimary );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.LifeformWeaponSecondary = ( LifeformWeapon )EditorGUI.EnumPopup( position, "Weapon - Secondary", tmpAppearance.LifeformWeaponSecondary );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.CamouflageLifeform = ( Camouflage )EditorGUI.EnumPopup( position, "Camouflage", tmpAppearance.CamouflageLifeform );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.ConcealedStationary = EditorGUI.Toggle( position, "Concealed - Stationary", tmpAppearance.ConcealedStationary );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.ConcealedMovement = EditorGUI.Toggle( position, "Concealed - Movement", tmpAppearance.ConcealedMovement );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Draw appearance options for sensor emitter entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawSensorEmitter( Rect position )
        {
            tmpAppearance.PaintScheme = ( PaintScheme )EditorGUI.EnumPopup( position, "Paint Scheme", tmpAppearance.PaintScheme );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MobilityKill = EditorGUI.Toggle( position, "Mobility Kill", tmpAppearance.MobilityKill );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MissionKill = EditorGUI.Toggle( position, "Mission Kill", tmpAppearance.MissionKill );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Damage = ( Damage )EditorGUI.EnumPopup( position, "Damage", tmpAppearance.Damage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Smoke = ( Smoke )EditorGUI.EnumPopup( position, "Smoke", tmpAppearance.Smoke );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.TrailingEffect = ( TrailingEffect )EditorGUI.EnumPopup( position, "Trailing Effect", tmpAppearance.TrailingEffect );
            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField( position, "Lights" );
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.indentLevel++;
            Color defaultCol = GUI.backgroundColor;

            GUI.backgroundColor = tmpAppearance.BlackoutLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Blackout", false ) )
            {
                tmpAppearance.BlackoutLights = !tmpAppearance.BlackoutLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.InteriorLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Interior", false ) )
            {
                tmpAppearance.InteriorLights = !tmpAppearance.InteriorLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.SensorLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Sensor", false ) )
            {
                tmpAppearance.SensorLights = !tmpAppearance.SensorLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = defaultCol;
            EditorGUI.indentLevel--;

            tmpAppearance.FlamingEffect = EditorGUI.Toggle( position, "Flaming Effect", tmpAppearance.FlamingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Antenna = EditorGUI.Toggle( position, "Antenna Raised", tmpAppearance.Antenna );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Camouflage = ( Camouflage )EditorGUI.EnumPopup( position, "Camouflage", tmpAppearance.Camouflage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.PowerPlant = EditorGUI.Toggle( position, new GUIContent( "Power Plant", "Engine on?" ), tmpAppearance.PowerPlant );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Tent = EditorGUI.Toggle( position, "Tent Raised", tmpAppearance.Tent );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Draw appearance options for space entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawSpace( Rect position )
        {
            tmpAppearance.PaintScheme = ( PaintScheme )EditorGUI.EnumPopup( position, "Paint Scheme", tmpAppearance.PaintScheme );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MobilityKill = EditorGUI.Toggle( position, "Mobility Kill", tmpAppearance.MobilityKill );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Damage = ( Damage )EditorGUI.EnumPopup( position, "Damage", tmpAppearance.Damage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Smoke = ( Smoke )EditorGUI.EnumPopup( position, "Smoke", tmpAppearance.Smoke );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.FlamingEffect = EditorGUI.Toggle( position, "Flaming Effect", tmpAppearance.FlamingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.PowerPlant = EditorGUI.Toggle( position, new GUIContent( "Power Plant", "Engine on?" ), tmpAppearance.PowerPlant );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Draw appearance options for sub surface entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawSubSurface( Rect position )
        {
            tmpAppearance.PaintScheme = ( PaintScheme )EditorGUI.EnumPopup( position, "Paint Scheme", tmpAppearance.PaintScheme );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MobilityKill = EditorGUI.Toggle( position, "Mobility Kill", tmpAppearance.MobilityKill );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Damage = ( Damage )EditorGUI.EnumPopup( position, "Damage", tmpAppearance.Damage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Smoke = ( Smoke )EditorGUI.EnumPopup( position, "Smoke", tmpAppearance.Smoke );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.HatchState = ( HatchState )EditorGUI.EnumPopup( position, "Hatch", tmpAppearance.HatchState );
            position.y += EditorGUIUtility.singleLineHeight;

            Color defaultCol = GUI.backgroundColor;
            GUI.backgroundColor = tmpAppearance.RunningLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Running Lights", false ) )
            {
                tmpAppearance.RunningLights = !tmpAppearance.RunningLights;
            }
            GUI.backgroundColor = defaultCol;
            position.y += EditorGUIUtility.singleLineHeight;

            tmpAppearance.FlamingEffect = EditorGUI.Toggle( position, "Flaming Effect", tmpAppearance.FlamingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.PowerPlant = EditorGUI.Toggle( position, new GUIContent( "Power Plant", "Engine on?" ), tmpAppearance.PowerPlant );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Draw appearance options for surface entities.
        /// </summary>
        /// <param name="position"></param>
        private void DrawSurface( Rect position )
        {
            tmpAppearance.PaintScheme = ( PaintScheme )EditorGUI.EnumPopup( position, "Paint Scheme", tmpAppearance.PaintScheme );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.MobilityKill = EditorGUI.Toggle( position, "Mobility Kill", tmpAppearance.MobilityKill );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Damage = ( Damage )EditorGUI.EnumPopup( position, "Damage", tmpAppearance.Damage );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Smoke = ( Smoke )EditorGUI.EnumPopup( position, "Smoke", tmpAppearance.Smoke );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.TrailingEffect = ( TrailingEffect )EditorGUI.EnumPopup( position, "Trailing Effect", tmpAppearance.TrailingEffect );
            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField( position, "Lights" );
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.indentLevel++;
            Color defaultCol = GUI.backgroundColor;

            GUI.backgroundColor = tmpAppearance.InteriorLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Interior", false ) )
            {
                tmpAppearance.InteriorLights = !tmpAppearance.InteriorLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.RunningLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Running Lights", false ) )
            {
                tmpAppearance.RunningLights = !tmpAppearance.RunningLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = tmpAppearance.SpotLights ? Color.yellow : Color.black;
            if( EditorGUI.Toggle( position, "Spot", false ) )
            {
                tmpAppearance.SpotLights = !tmpAppearance.SpotLights;
            }
            position.y += EditorGUIUtility.singleLineHeight;

            GUI.backgroundColor = defaultCol;
            EditorGUI.indentLevel--;

            tmpAppearance.FlamingEffect = EditorGUI.Toggle( position, "Flaming Effect", tmpAppearance.FlamingEffect );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.Frozen = EditorGUI.Toggle( position, "Frozen", tmpAppearance.Frozen );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.PowerPlant = EditorGUI.Toggle( position, new GUIContent( "Power Plant", "Engine on?" ), tmpAppearance.PowerPlant );
            position.y += EditorGUIUtility.singleLineHeight;
            tmpAppearance.State = EditorGUI.Toggle( position, new GUIContent( "State", "Is the entity Active?" ), tmpAppearance.State );
            position.y += EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Setup SerializedProperty's and any labels used, tools tips etc.
        /// </summary>
        /// <param name="property"></param>
        private void Init( SerializedProperty property )
        {
            // Attempt to find an entity type            
            string root = property.propertyPath.Substring( 0, property.propertyPath.LastIndexOf( '.' ) );
            kind = property.serializedObject.FindProperty( root + ".entityType.kind" );
            domain = property.serializedObject.FindProperty( root + ".entityType.domain" );
            category = property.serializedObject.FindProperty( root + ".entityType.category" );
            DetermineAppearanceType();

            appearance = property.FindPropertyRelative( "appearance" );

            tmpAppearance = new EntityAppearance( appearance.intValue );
        }

        /// <summary>
        /// If an entity type was found we can determine what type of appearance we are.
        /// </summary>
        private void DetermineAppearanceType()
        {
            // Determine the appearance type
            if( kind != null )
            {
                // Determine what type of appearance to use.
                switch( ( EntityKind )Kind )
                {
                    case EntityKind.CulturalFeature:
                    selectedAppearanceType = AppearanceTypes.Cultural;
                    break;

                    case EntityKind.Environmental:
                    selectedAppearanceType = AppearanceTypes.Environmental;
                    break;

                    case EntityKind.Lifeform:
                    selectedAppearanceType = AppearanceTypes.Lifeform;
                    break;

                    case EntityKind.Munition:
                    if( Category == 1 ) // Guided
                    {
                        selectedAppearanceType = AppearanceTypes.GuidedMunition;
                    }
                    break;

                    case EntityKind.Platform:
                    switch( ( EntityDomain )Domain )
                    {
                        case EntityDomain.Air:
                        selectedAppearanceType = AppearanceTypes.Air;
                        break;

                        case EntityDomain.Land:
                        selectedAppearanceType = AppearanceTypes.Land;
                        break;

                        case EntityDomain.Subsurface:
                        selectedAppearanceType = AppearanceTypes.Subsurface;
                        break;

                        case EntityDomain.Surface:
                        selectedAppearanceType = AppearanceTypes.Surface;
                        break;

                        case EntityDomain.Space:
                        selectedAppearanceType = AppearanceTypes.Space;
                        break;
                    }
                    break;

                    case EntityKind.SensorEmitter:
                    selectedAppearanceType = AppearanceTypes.Sensor;
                    break;
                }
            }
        }

        /// <summary>
        /// Current height of this property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            float h = EditorGUIUtility.singleLineHeight; // Label

            DetermineAppearanceType();
            if( property.isExpanded )
            {
                if( EditorSettings.AdvancedMode )
                {
                    h += EditorGUIUtility.singleLineHeight;
                }

                if( EditorSettings.AdvancedMode && selectedAppearanceType != AppearanceTypes.Unknown )
                {
                    h += EditorGUIUtility.singleLineHeight;
                }

                switch( selectedAppearanceType )
                {
                    case AppearanceTypes.Unknown:
                    h += EditorGUIUtility.singleLineHeight; // Select appearance type field
                    break;

                    case AppearanceTypes.Air:
                    h += 18 * EditorGUIUtility.singleLineHeight;
                    break;

                    case AppearanceTypes.Cultural:
                    h += 10 * EditorGUIUtility.singleLineHeight;
                    break;

                    case AppearanceTypes.Environmental:
                    h += 4 * EditorGUIUtility.singleLineHeight;
                    break;

                    case AppearanceTypes.GuidedMunition:
                    h += 8 * EditorGUIUtility.singleLineHeight;
                    break;

                    case AppearanceTypes.Land:
                    h += 26 * EditorGUIUtility.singleLineHeight;
                    break;

                    case AppearanceTypes.Lifeform:
                    h += 12 * EditorGUIUtility.singleLineHeight;
                    break;

                    case AppearanceTypes.Sensor:
                    h += 17 * EditorGUIUtility.singleLineHeight;
                    break;

                    case AppearanceTypes.Space:
                    h += 8 * EditorGUIUtility.singleLineHeight;
                    break;

                    case AppearanceTypes.Subsurface:
                    h += 10 * EditorGUIUtility.singleLineHeight;
                    break;

                    case AppearanceTypes.Surface:
                    h += 13 * EditorGUIUtility.singleLineHeight;
                    break;
                }
            }
            return h;
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
                EditorGUI.indentLevel++;

                EditorGUI.BeginChangeCheck();

                if( EditorSettings.AdvancedMode )
                {
                    tmpAppearance.Appearance = EditorGUI.IntField( position, "Appearance Field", tmpAppearance.Appearance );
                    position.y += EditorGUIUtility.singleLineHeight;
                }

                if( property.isExpanded )
                {
                    if( selectedAppearanceType == AppearanceTypes.Unknown || EditorSettings.AdvancedMode )
                    {
                        selectedAppearanceType = ( AppearanceTypes )EditorGUI.EnumPopup( position, "Appearance Type", selectedAppearanceType );
                        position.y += EditorGUIUtility.singleLineHeight;
                    }

                    switch( selectedAppearanceType )
                    {
                        case AppearanceTypes.Air:
                        DrawAir( position );
                        break;

                        case AppearanceTypes.Cultural:
                        DrawCultural( position );
                        break;

                        case AppearanceTypes.Environmental:
                        DrawEnvironmentals( position );
                        break;

                        case AppearanceTypes.GuidedMunition:
                        DrawGuidedMunitions( position );
                        break;

                        case AppearanceTypes.Land:
                        DrawLand( position );
                        break;

                        case AppearanceTypes.Lifeform:
                        DrawLifeform( position );
                        break;

                        case AppearanceTypes.Sensor:
                        DrawSensorEmitter( position );
                        break;

                        case AppearanceTypes.Space:
                        DrawSpace( position );
                        break;

                        case AppearanceTypes.Subsurface:
                        DrawSubSurface( position );
                        break;

                        case AppearanceTypes.Surface:
                        DrawSurface( position );
                        break;
                    }
                }

                if( EditorGUI.EndChangeCheck() )
                {
                    appearance.intValue = tmpAppearance.Appearance;
                }
            }
            EditorGUI.EndProperty();
        }
    }
}
