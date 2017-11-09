using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using DISUnity.DataType;
using System.Text;
using DISUnity.DataType.Enums.Apperance;

namespace DISUnity.DataType
{
    /// <summary>
    /// Represents an entity appearance using bit fields.
    /// Please read the remarks for each property to ensure you use the correct ones for your type of entity.
    /// </summary>
    /// <size>4 bytes</size>
    [Serializable]    
    public class EntityAppearance : DataTypeBaseSimple
    {
        #region Properties

        #region Private
        
        [SerializeField]        
        private int appearance;

        #endregion Private

        /// <summary>
        /// Size of this data type in bytes
        /// </summary>
        /// <returns></returns>
        public override int Length
        {
            get
            {
                return 4;
            }
        }

        /// <summary>
        /// The appearance field. This is interpreted differently depending on the type of entity.
        /// </summary>
        public int Appearance
        {
            get
            {
                return appearance;
            }
            set
            {
                isDirty = true;
                appearance = value;
            }
        }


        #region Shared

        /// <summary>
        /// Describes the paint scheme of an entity
        /// This value is valid only on entity types:
        ///     Air
        ///     Land
        ///     Lifeform
        ///     Sensor Emitter
        ///     Space
        ///     SubSurface
        ///     Surface
        /// </summary>
        /// <remarks>
        /// Bit 0
        /// </remarks>
        public PaintScheme PaintScheme
        {
            get
            {
                return ( PaintScheme )( appearance & 0x01 );
            }
            set
            {
                isDirty = true;
                appearance = ( int )( ( int )value == 1 ? ( appearance | 0x01 ) : ( appearance & ~0x01 ) );
            }
        }

        /// <summary>
        /// Mobility kills?
        /// This value is valid only on entity types:
        ///     Air 
        ///     Land
        ///     Sensor Emitter
        ///     Space
        ///     SubSurface
        ///     Surface        
        /// </summary>
        /// <remarks>
        /// Bit 1
        /// </remarks>
        public bool MobilityKill
        {
            get
            {
                return ( appearance & 0x02 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x02 ) : ( appearance & ~0x02 ) );
            }
        }

        /// <summary>
        /// Describes the damaged appearance of an entity
        /// This value is valid only on entity types:
        ///     Air 
        ///     Cultural
        ///     Guided Munitions
        ///     Land
        ///     Lifeform
        ///     Sensor Emitter    
        ///     Space      
        ///     SubSurface
        ///     Surface        
        /// </summary>
        /// <remarks>
        /// Bits 3-4
        /// </remarks>
        public Damage Damage
        {
            get
            {
                return ( Damage )( ( appearance & 0x18 ) >> 3 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 3 ) | ( appearance & ~0x18 );
            }
        }

        /// <summary>
        /// Describes status or location of smoke emanating from an entity        
        /// This value is valid only on entity types:
        ///     Air 
        ///     Cultural
        ///     Guided Munitions
        ///     Land        
        ///     Sensor Emitter    
        ///     Space      
        ///     SubSurface
        ///     Surface        
        /// </summary>
        /// <remarks>
        /// Bits 5-6
        /// </remarks>
        public Smoke Smoke
        {
            get
            {
                return ( Smoke )( ( appearance & 0x60 ) >> 5 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 5 ) | ( appearance & ~0x60 );
            }
        }

        /// <summary>
        /// Describes the size of the dust cloud trailing effect for the entity        
        /// This value is valid only on entity types:
        ///     Air 
        ///     Guided Munitions
        ///     Land        
        ///     Sensor Emitter        
        ///     SubSurface 
        /// </summary>
        /// <remarks>
        /// Bits 7-8
        /// </remarks>
        public TrailingEffect TrailingEffect
        {
            get
            {
                return ( TrailingEffect )( ( appearance & 0x180 ) >> 7 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 7 ) | ( appearance & ~0x180 );
            }
        }

        /// <summary>
        /// Entity primary hatch state. Open, closed etc.
        /// This value is valid only on entity types:
        ///     Land             
        ///     SubSurface 
        /// </summary>
        /// <remarks>
        /// Bits 9-11
        /// </remarks>
        public HatchState HatchState
        {
            get
            {
                return ( HatchState )( ( appearance & 0xE00 ) >> 9 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 9 ) | ( appearance & ~0xE00 );
            }
        }

        /// <summary>
        /// Are flames rising from the entity?
        /// This value is valid only on entity types:
        ///     Air 
        ///     Cultural
        ///     Guided Munitions
        ///     Land        
        ///     Sensor Emitter    
        ///     Space      
        ///     SubSurface
        ///     Surface       
        /// </summary>
        /// <remarks>
        /// Bit 15
        /// </remarks>
        public bool FlamingEffect
        {
            get
            {
                return ( appearance & 0x8000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x8000 ) : ( appearance & ~0x8000 ) );
            }
        }
        
        /// <summary>
        /// Camouflage type worn.
        /// This value is valid only on entity types:
        ///     Land             
        ///     Sensor Emitter    
        /// For Lifeforms use <see cref="CamouflageLifeform"/>.
        /// </summary>
        /// <remarks>
        /// Bits 17-18
        /// </remarks>
        public Camouflage Camouflage 
        {
            get
            {
                return ( Camouflage )( ( appearance & 0x60000 ) >> 17 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 17 ) | ( appearance & ~0x60000 );
            }
        }

        /// <summary>
        /// Is the entity concealed? 
        //  False = Not concealed. True = Entity in a prepared concealed position (with netting, etc)        
        /// This value is valid only on entity types:
        ///     Land        
        ///     Sensor Emitter        
        /// </summary>
        /// <remarks>
        /// Bit 19
        /// </remarks>
        public bool Concealed 
        {
            get
            {
                return ( appearance & 0x80000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x80000 ) : ( appearance & ~0x80000 ) );
            }
        }

        /// <summary>
        /// Is the entity frozen?
        /// Note: Frozen entities should not be dead-reckoned, they should remain frozen in place. 
        /// You would likely freeze entites when your application is in a paused state.        
        /// This value is valid only on entity types:
        ///     Air 
        ///     Cultural
        ///     Environmentals
        ///     Guided Munitions
        ///     Land    
        ///     LifeForm
        ///     Sensor Emitter    
        ///     Space      
        ///     SubSurface
        ///     Surface           
        /// </summary>
        /// <remarks>
        /// Bit 21
        /// </remarks>
        public bool Frozen
        {
            get
            {
                return ( appearance & 0x200000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x200000 ) : ( appearance & ~0x200000 ) );
            }
        }

        /// <summary>
        /// Power Plant On/Off. True = On, False = Off.        
        /// This value is valid only on entity types:
        ///     Air 
        ///     Guided Munitions
        ///     Land    
        ///     Sensor Emitter    
        ///     Space      
        ///     SubSurface
        ///     Surface           
        /// </summary>
        /// <remarks>
        /// Bit 22
        /// </remarks>
        public bool PowerPlant
        {
            get
            {
                return ( appearance & 0x400000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x400000 ) : ( appearance & ~0x400000 ) );
            }
        }

        /// <summary>
        /// State of entity.
        /// True = Active, False = Deactivated.        
        /// This value is valid only on entity types:
        ///     Air 
        ///     Cultural
        ///     Environmentals
        ///     Guided Munitions
        ///     Land    
        ///     LifeForm
        ///     Sensor Emitter    
        ///     Space      
        ///     SubSurface
        ///     Surface               
        /// </summary>
        /// <remarks>
        /// Bit 23
        /// </remarks>
        public bool State
        {
            get
            {
                return ( appearance & 0x800000 ) != 0 ? false : true;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x800000 ) : ( appearance & ~0x800000 ) );
            }
        }

        /// <summary>
        /// Tent status. 
        /// True = Not extended, False = Extended.        
        /// This value is valid only on entity types:
        ///     Land    
        ///     Sensor Emitter               
        /// </summary>
        /// <remarks>
        /// Bit 24
        /// </remarks>
        public bool Tent
        {
            get
            {
                return ( appearance & 0x1000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x1000000 ) : ( appearance & ~0x1000000 ) );
            }
        }

        /// <summary>
        /// Are the vehicles blackout lights turned on?
        /// True = On, False = Off.        
        /// This value is valid only on entity types:
        ///     Land    
        ///     Sensor Emitter               
        /// </summary>
        /// <remarks>
        /// Bit 26
        /// </remarks>
        public bool BlackoutLights
        {
            get
            {
                return ( appearance & 0x4000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x4000000 ) : ( appearance & ~0x4000000 ) );
            }
        }

        /// <summary>
        /// Are the vehicles spot lights turned on?
        /// True = On, False = Off.        
        /// This value is valid only on entity types:
        ///     Air
        ///     Land    
        ///     Surface        
        /// </summary>
        /// <remarks>
        /// Bit 28
        /// </remarks>
        public bool SpotLights
        {
            get
            {
                return ( appearance & 0x10000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x10000000 ) : ( appearance & ~0x10000000 ) );
            }
        }

        /// <summary>
        /// Are the vehicles interior lights turned on?
        /// True = On, False = Off.        
        /// This value is valid only on entity types:
        ///     Air
        ///     Cultural
        ///     Land    
        ///     Sensor Emitter
        ///     Surface        
        /// </summary>
        /// <remarks>
        /// Bit 29
        /// </remarks>
        public bool InteriorLights 
        {
            get
            {
                return ( appearance & 0x20000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x20000000 ) : ( appearance & ~0x20000000 ) );
            }
        }

        /// <summary>
        /// Is the entity masked/cloaked or not. 
        /// True = Masked/Cloaked, False = Not Masked/Cloaked.        
        /// This value is valid only on entity types:        
        ///     Cultural
        ///     Environmental
        ///     Guided
        ///     Land      
        /// </summary>
        /// <remarks>
        /// Bit 31
        /// </remarks>
        public bool MaskedCloaked
        {
            get
            {
                return ( appearance & -2147483648 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | -2147483648 ) : ( appearance & ~-2147483648 ) );
            }
        }

        /// <summary>
        /// Are the vehicles running lights turned on?
        /// True = On, False = Off.        
        /// This value is valid only on entity types:
        ///     SubSurface
        ///     Surface        
        /// </summary>
        /// <remarks>
        /// Bit 12
        /// </remarks>
        public bool RunningLights
        {
            get
            {
                return ( appearance & 0x1000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x1000 ) : ( appearance & ~0x1000 ) );
            }
        }
                
        #endregion Shared

        #region Air

        /// <summary>
        /// Is the canopy open or closed?        
        /// This value is valid only on Air entity types.               
        /// </summary>
        /// <remarks>
        /// Bits 9-11
        /// </remarks>
        public CanopyState CanopyState 
        {
            get
            {
                return ( CanopyState )( ( appearance & 0xE00 ) >> 9 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 9 ) | ( appearance & ~0xE00 );
            }
        }

        /// <summary>
        /// Are the landing lights turned on?
        /// True = On, False = Off.
        /// This value is valid only on Air entity types.       
        /// </summary>
        /// <remarks>
        /// Bit 12
        /// </remarks>
        public bool LandingLights
        {
            get
            {
                return ( appearance & 0x1000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x1000 ) : ( appearance & ~0x1000 ) );
            }
        }

        /// <summary>
        /// Are the navigation lights turned on?
        /// True = On, False = Off.        
        /// This value is valid only on Air entity types.       
        /// </summary>
        /// <remarks>
        /// Bit 13
        /// </remarks>
        public bool NavigationLights
        {
            get
            {
                return ( appearance & 0x2000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x2000 ) : ( appearance & ~0x2000 ) );
            }
        }

        /// <summary>
        /// Are the anti collision lights turned on?
        /// True = On, False = Off.        
        /// This value is valid only on Air entity types.       
        /// </summary>
        /// <remarks>
        /// Bit 14
        /// </remarks>
        public bool AntiCollisionLights
        {
            get
            {
                return ( appearance & 0x4000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x4000 ) : ( appearance & ~0x4000 ) );
            }
        }

        /// <summary>
        /// Is the entity afterburner on?
        /// True = On, False = Off.        
        /// This value is valid only on Air entity types.       
        /// </summary>
        /// <remarks>
        /// Bit 16
        /// </remarks>
        public bool Afterburner
        {
            get
            {
                return ( appearance & 0x10000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x10000 ) : ( appearance & ~0x10000 ) );
            }
        }

        /// <summary>
        /// Are the formation lights on?
        /// True = On, False = Off.        
        /// This value is valid only on Air entity types.       
        /// </summary>
        /// <remarks>
        /// Bit 24
        /// </remarks>
        public bool FormationLights 
        {
            get
            {
                return ( appearance & 0x1000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x1000000 ) : ( appearance & ~0x1000000 ) );
            }
        }

        #endregion

        #region Cultural

        /// <summary>
        /// Is the feature emitting internal heat?        
        /// This value is valid only on Cultural entity types.
        /// </summary>
        /// <remarks>
        /// Bit 22
        /// </remarks>
        public bool InternalHeat
        {
            get
            {
                return ( appearance & 0x400000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x400000 ) : ( appearance & ~0x400000 ) );
            }
        }

        /// <summary>
        /// Are the exterior lights turned on? 
        /// True = On, False = Off.        
        /// This value is valid only on Cultural entity types.       
        /// </summary>
        /// <remarks>
        /// Bit 28
        /// </remarks>
        public bool ExteriorLights
        {
            get
            {
                return ( appearance & 0x10000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x10000000 ) : ( appearance & ~0x10000000 ) );
            }
        }        

        #endregion Cultural

        #region Environmentals

        /// <summary>
        /// Describes the density of the environmentals.        
        /// This value is valid only on Environmentals.
        /// </summary>
        /// <remarks>
        /// Bits 16-19
        /// </remarks>
        public Density Density
        {
            get
            {
                return ( Density )( ( appearance & 0xF0000 ) >> 16 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 16 ) | ( appearance & ~0xF0000 );
            }
        }

        #endregion Environmentals

        #region Guided Munitions

        /// <summary>
        /// Presence of a launch flash? 
        /// True = Flash Present, False = No Flash.        
        /// This value is valid only on Land entity types.        
        /// </summary>
        /// <remarks>
        /// Bit 16
        /// </remarks>
        public bool LaunchFlash
        {
            get
            {
                return ( appearance & 0x10000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x10000 ) : ( appearance & ~0x10000 ) );
            }
        }

        #endregion Guided Munitions

        #region Land

        /// <summary>
        /// Does Fire Power Kill?        
        /// This value is valid only on Land entity types.        
        /// </summary>
        /// <remarks>
        /// Bit 2
        /// </remarks>
        public bool FirePower
        {
            get
            {
                return ( appearance & 0x04 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x04 ) : ( appearance & ~0x04 ) );
            }
        }

        /// <summary>
        /// Are the vehicles head lights turned on? True = On, False = Off.
        /// This value is valid only on Land entity types.        
        /// </summary>
        /// <remarks>
        /// Bit 12
        /// </remarks>
        public bool HeadLights
        {
            get
            {
                return ( appearance & 0x1000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x1000 ) : ( appearance & ~0x1000 ) );
            }
        }

        /// <summary>
        /// Are the vehicles tail lights turned on? True = On, False = Off.
        /// This value is valid only on Land entity types.        
        /// </summary>
        /// <remarks>
        /// Bit 13
        /// </remarks>
        public bool TailLights
        {
            get
            {
                return ( appearance & 0x2000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x2000 ) : ( appearance & ~0x2000 ) );
            }
        }

        /// <summary>
        /// Are the vehicles brake lights turned on? True = On, False = Off.
        /// This value is valid only on Land entity types.        
        /// </summary>
        /// <remarks>
        /// Bit 14
        /// </remarks>
        public bool BrakeLights
        {
            get
            {
                return ( appearance & 0x4000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x4000 ) : ( appearance & ~0x4000 ) );
            }
        }

        /// <summary>
        /// Is the launcher raised?        
        /// This value is valid only on Land entity types.        
        /// </summary>
        /// <remarks>
        /// Bit 16
        /// </remarks>
        public bool LauncherRaised
        {
            get
            {
                return ( appearance & 0x10000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x10000 ) : ( appearance & ~0x10000 ) );
            }
        }

        /// <summary>
        /// Ramp Status. 
        /// True = Down, False = Up.
        /// This value is valid only on Land entity types.                      
        /// </summary>
        /// <remarks>
        /// Bit 25
        /// </remarks>
        public bool Ramp
        {
            get
            {
                return ( appearance & 0x2000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x2000000 ) : ( appearance & ~0x2000000 ) );
            }
        }

        /// <summary>
        /// Are the vehicles blackout brake lights turned on?
        /// True = On, False = Off.
        /// This value is valid only on Land entity types.               
        /// </summary>
        /// <remarks>
        /// Bit 27
        /// </remarks>
        public bool BlackoutBrakeLights
        {
            get
            {
                return ( appearance & 0x8000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x8000000 ) : ( appearance & ~0x8000000 ) );
            }
        }

        /// <summary>
        /// Has the vehicle occupant surrendered? 
        /// True = Surrendered, False = Not Surrendered.        
        /// This value is valid only on Land entity types.               
        /// </summary>
        /// <remarks>
        /// Bit 30
        /// </remarks>
        public bool Surrendered
        {
            get
            {
                return ( appearance & 0x40000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x40000000 ) : ( appearance & ~0x40000000 ) );
            }
        }

        #endregion Land

        #region Lifeform

        /// <summary>
        /// Camouflage type worn on Lifeforms.        
        /// This value is valid only on Lifeforms.
        /// To set the Camouflage on Land or Sensors use <see cref="Camouflage"/>
        /// </summary>
        /// <remarks>
        /// Bits 28-29
        /// </remarks>
        public Camouflage CamouflageLifeform 
        {
            get
            {
                return ( Camouflage )( ( appearance & 0x30000000 ) >> 28 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 28 ) | ( appearance & ~0x30000000 );
            }
        }

        /// <summary>
        /// Describes compliance of a life form.        
        /// This value is valid only on Lifeforms.
        /// </summary>
        /// <remarks>
        /// Bits 5-8
        /// </remarks>
        public Compliance Compliance
        {
            get
            {
                return ( Compliance )( ( appearance & 0x1E0 ) >> 5 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 5 ) | ( appearance & ~0x1E0 );
            }
        }

        /// <summary>
        /// Describes whether Flash Lights are on or off. 
        /// True = On. False = Off.        
        /// This value is valid only on Land entity types.        
        /// </summary>
        /// <remarks>
        /// Bit 12
        /// </remarks>
        public bool Flashlight
        {
            get
            {
                return ( appearance & 0x1000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x1000 ) : ( appearance & ~0x1000 ) );
            }
        }

        /// <summary>
        /// Describes the state of the life form.        
        /// This value is valid only on Lifeforms.
        /// </summary>
        /// <remarks>
        /// Bits 16-19
        /// </remarks>
        public LifeformState LifeformState
        {
            get
            {
                return ( LifeformState )( ( appearance & 0xF0000 ) >> 16 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 16 ) | ( appearance & ~0xF0000 );
            }
        }
        
        /// <summary>
        /// Describes the position of the life forms primary weapon.        
        /// This value is valid only on Lifeforms.
        /// </summary>
        /// <remarks>
        /// Bits 24-25
        /// </remarks>
        public LifeformWeapon LifeformWeaponPrimary
        {
            get
            {
                return ( LifeformWeapon )( ( appearance & 0x3000000 ) >> 24 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 24 ) | ( appearance & ~0x3000000 );
            }
        }

        /// <summary>
        /// Describes the position of the life forms secondary weapon.        
        /// This value is valid only on Lifeforms.
        /// </summary>
        /// <remarks>
        /// Bits 26-27
        /// </remarks>
        public LifeformWeapon LifeformWeaponSecondary
        {
            get
            {
                return ( LifeformWeapon )( ( appearance & 0xC000000 ) >> 26 );
            }
            set
            {
                isDirty = true;
                appearance = ( ( int )value << 26 ) | ( appearance & ~0xC000000 );
            }
        }

        /// <summary>
        /// Describes the type of stationary concealment
        /// True = Entity in a prepared concealed position. False = Not concealed.        
        /// This value is valid only on Lifeforms.        
        /// </summary>
        /// <remarks>
        /// Bit 30
        /// </remarks>
        public bool ConcealedStationary
        {
            get
            {
                return ( appearance & 0x40000000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x40000000 ) : ( appearance & ~0x40000000 ) );
            }
        }

        /// <summary>
        /// Describes the type of movement concealment. 
        /// True = Rushes between covered positions. False = Open movement.         
        /// This value is valid only on Lifeforms.    
        /// </summary>
        /// <remarks>
        /// Bit 31
        /// </remarks>
        public bool ConcealedMovement
        {
            get
            {
                return ( appearance & -2147483648 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | -2147483648 ) : ( appearance & ~-2147483648 ) );
            }
        }

        #endregion Lifeform

        #region Sensor Emitter

        /// <summary>
        /// Describes characteristics of mission kill (e.g. damaged antenna).
        /// False = No mission kill, True = Mission kill        
        /// This value is valid only on Sensor Emitter entity types.   
        /// </summary>
        /// <remarks>
        /// Bit 2
        /// </remarks>
        public bool MissionKill
        {
            get
            {
                return ( appearance & 0x04 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x04 ) : ( appearance & ~0x04 ) );
            }
        }

        /// <summary>
        /// Are the sensors lights turned on? 
        /// True = On, False = Off.        
        /// This value is valid only on Sensor Emitter entity types.   
        /// </summary>
        /// <remarks>
        /// Bit 12
        /// </remarks>
        public bool SensorLights
        {
            get
            {
                return ( appearance & 0x1000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x1000 ) : ( appearance & ~0x1000 ) );
            }
        }

        /// <summary>
        /// Sensor antenna raised status. 
        /// False = Not Raised, True = Raised.        
        /// This value is valid only on Sensor Emitter entity types.   
        /// </summary>
        /// <remarks>
        /// Bit 16
        /// </remarks>
        public bool Antenna
        {
            get
            {
                return ( appearance & 0x10000 ) != 0 ? true : false;
            }
            set
            {
                isDirty = true;
                appearance = ( int )( value ? ( appearance | 0x10000 ) : ( appearance & ~0x10000 ) );
            }
        }

        #endregion Sensor Emitter
        
        #region Space

        // Nothing unique

        #endregion Space

        #region SubSurface

        // Nothing unique

        #endregion SubSurface

        #region Surface

        // Nothing unique

        #endregion Surface
        
        #endregion Properties
        
        public EntityAppearance()
        {
        }

        public EntityAppearance( int appearanceBits )
        {
            appearance = appearanceBits;
        }

        /// <summary>
        /// Create a new instance from binary data.
        /// </summary>
        /// <param name="br"></param>
        public EntityAppearance( BinaryReader br )
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
            appearance = br.ReadInt32();
        }

        /// <summary>
        /// Encode data for network transmission.
        /// </summary>
        /// <param name="bw"></param>
        public override void Encode( BinaryWriter bw )
        {            
            bw.Write( appearance );
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {                        
            return string.Format( "{0, -20} : {1}\n", "Entity Appearance Field", appearance );
        }

        #endregion DataTypeBase

        #region Operators

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals( EntityAppearance b )
        {
            if( appearance != b.appearance ) return false;
            return true;
        }

        /// <summary>
        /// Compares internal data for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals( EntityAppearance a, EntityAppearance b )
        {
            return a.Equals( b );
        }

        #endregion Operators
    }
}