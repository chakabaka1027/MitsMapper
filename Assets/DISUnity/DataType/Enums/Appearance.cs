
namespace DISUnity.DataType.Enums.Apperance
{
    /// <summary>
    /// Camouflage type worn.
    /// </summary>
    public enum Camouflage : byte
    {
        DesertCamouflage = 0,
        WinterCamouflage = 1,
        ForestCamouflage = 2
    }

    /// <summary>
    /// Describes the canopy state of an entity
    /// </summary>
    public enum CanopyState : byte
    {
        NotApplicable = 0,
        PrimaryHatchIsClosed = 1,
        PrimaryHatchIsOpen = 4,
    }

    /// <summary>
    /// Describes compliance of a life form.
    /// </summary>
    public enum Compliance : byte
    {
        Detained = 1,
        Surrender = 2,
        UsingFists = 3,
        VerbalAbuseLevel1 = 4,
        VerbalAbuseLevel2 = 5,
        VerbalAbuseLevel3 = 6,
        PassiveResistanceLevel1 = 7,
        PassiveResistanceLevel2 = 8,
        PassiveResistanceLevel3 = 9,
        UsingNonLethalWeapon1 = 10,
        UsingNonLethalWeapon2 = 11,
        UsingNonLethalWeapon3 = 12,
        UsingNonLethalWeapon4 = 13,
        UsingNonLethalWeapon5 = 14,
        UsingNonLethalWeapon6 = 15
    }

    /// <summary>
    /// Describes the damaged appearance of an entity
    /// </summary>
    public enum Damage : byte
    {
        NoDamage = 0,
        SlightDamage = 1,
        ModerateDamage = 2,
        Destroyed = 3
    }

    /// <summary>
    /// Describes the density of the environmentals
    /// </summary>
    public enum Density : byte
    {
        Clear = 0,
        Hazy = 1,
        Dense = 2,
        VeryDense = 3,
        Opaque = 4
    }

    /// <summary>
    /// Describes the hatch state of an entity
    /// </summary>
    public enum HatchState : byte
    {
        NotApplicable = 0,
        PrimaryHatchIsClosed = 1,
        PrimaryHatchIsPopped = 2,
        PrimaryHatchIsPoppedPersonVisibleUnderHatch = 3,
        PrimaryHatchIsOpen = 4,
        PrimaryHatchIsOpenPersonVisible = 5
    }

    /// <summary>
    /// Describes the state of the life form.
    /// </summary>
    public enum LifeformState : byte
    {
        UprightStandingStill = 1,
        UprightWalking = 2,
        UprightRunning = 3,
        Kneeling = 4,
        Prone = 5,
        Crawling = 6,
        Swimming = 7,
        Parachuting = 8,
        Jumping = 9,
        Sitting = 10,
        Squatting = 11,
        Crouching = 12,
        Wading = 13,
        SurrenderAppearance = 14,
        DetainedAppearance = 15
    }

    /// <summary>
    /// Describes the position of the life forms weapon
    /// </summary>
    public enum LifeformWeapon : byte
    {
        NoWeaponPresent = 0,
        WeaponIsStowed = 1,
        WeaponIsDeployed = 2,
        WeaponIsInTheFiringPosition = 3
    }

    /// <summary>
    /// Describes the paint scheme of an entity
    /// </summary>
    public enum PaintScheme : byte
    {
        UniformColor = 0,
        Camouflage = 1
    }

    /// <summary>
    /// Describes status or location of smoke emanating from an entity
    /// </summary>
    public enum Smoke : byte
    {
        NotSmoking = 0,
        SmokePlume = 1,
        EngineSmoke = 2,
        EngineSmokeAndSmokePlume = 3
    }

    /// <summary>
    /// Describes the size of the dust cloud trailing effect for the entity
    /// </summary>
    public enum TrailingEffect : byte
    {
        NoTrail = 0,
        Small = 1,
        Medium = 2,
        Large = 3
    }
}
