namespace MCHexBOT.Protocol
{
    public enum ConnectionState
    {
        Handshaking,
        Status,
        Login,
        Play
    }

    public enum HandshakeType
    {
        Status = 1,
        Login = 2
    }

    public enum EntityInteractType
    {
        Interact = 0,
        Attack = 1,
        InteractAt = 2,
    }

    public enum EntityInteractHandType
    {
        Main = 0,
        Second = 1
    }

    public enum ChatMode
    {
        Enabled = 0,
        Commands = 1,
        Hidden = 2
    }

    public enum PlayerAction
    {
        StartSneaking = 0,
        StopSneaking = 1,
        LeaveBed = 2,
        StartSprinting = 3,
        StopSprinting = 4,
        StartHorseJump = 5,
        StopHorseJump = 6,
        OpenHorseInventory = 7,
        StartElytraFlying = 8
    }

    public enum MainHandType
    {
        Left = 0,
        Right = 1
    }

    public enum ParticleType : byte
    {
        AmbientEntityEffect = 0,
        AngryVillager = 1,
        Block = 2,
        BlockMarker = 3,
        Bubble = 4,
        Cloud = 5,
        Crit = 6,
        DamageIndicator = 7,
        DragonBreath = 8,
        DrippingLava = 9,
        FallingLava = 10,
        LandingLava = 11,
        DrippingWater = 12,
        FallingWater = 13,
        Dust = 14,
        DustColorTransition = 15,
        Effect = 16,
        ElderGuardian = 17,
        EnchantedHit = 18,
        Enchant = 19,
        EndRod = 20,
        EntityEffect = 21,
        ExplosionEmitter = 22,
        Explosion = 23,
        FallingDust = 24,
        Firework = 25,
        Fishing = 26,
        Flame = 27,
        SoulFireFlame = 28,
        Soul = 29,
        Flash = 30,
        HappyVillager = 31,
        Composter = 32,
        Heart = 33,
        InstantEffect = 34,
        Item = 35,
        Vibration = 36,
        ItemSlime = 37,
        ItemSnowball = 38,
        LargeSmoke = 39,
        Lava = 40,
        Mycelium = 41,
        Note = 42,
        Poof = 43,
        Portal = 44,
        Rain = 45,
        Smoke = 46,
        Sneeze = 47,
        Spit = 48,
        SquidInk = 49,
        SweepAttack = 50,
        TotemOfundying = 51,
        Underwater = 52,
        Splash = 53,
        Witch = 54,
        BubblePop = 55,
        CurrentDown = 56,
        BubbleColumnUp = 57,
        Nautilus = 58,
        Dolphin = 59,
        CampfireCosySmoke = 60,
        CampfireSignalSmoke = 61,
        DrippingHoney = 62,
        FallingHoney = 63,
        LandingHoney = 64,
        FallingNectar = 65,
        FallingSporeBlossom = 66,
        Ash = 67,
        CrimsonSpore = 68,
        WarpedSpore = 69,
        SporeBlossomAir = 70,
        DrippingObsidianTear = 71,
        FallingObsidianTear = 72,
        LandingObsidianTear = 73,
        ReversePortal = 74,
        WhiteAsh = 75,
        SmallFlame = 76,
        Snowflake = 77,
        DrippingDripstoneLava = 78,
        FallingDripstoneLava = 79,
        DrippingDripstoneWater = 80,
        FallingDripstoneWater = 81,
        GlowSquidInk = 82,
        Glow = 83,
        WaxOn = 84,
        WaxOff = 85,
        ElectricSpark = 86,
        Scrape = 87
    }
}
