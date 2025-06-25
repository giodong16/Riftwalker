using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Handgun,
    Rifle
}
public enum NameAttack
{
    Firing,
    //  Firing_In_The_Air,
    Kicking,
    // Run_Firing,
    //  Run_Slashing,
    // Run_Throwing,
    Slashing,
    //  Slashing_In_The_Air,
    Throwing,
    // Throwing_In_The_Air,
    Dash,
}
public enum AnimationType
{
    Dying, Falling_Down, Firing,Firing_In_The_Air, Hurt, Idle, Idle_Blinking, Jump_Start_Handgun, Kicking, Laughing,
    Run_Firing, Run_Slashing, Run_Throwing, Running, Slashing, Slashing_In_The_Air, Sliding, Throwing, Throwing_In_The_Air,

    Dying_Rifle, Falling_Down_Rifle, Firing_Rifle, Firing_In_The_Air_Rifle, Hurt_Rifle, Idle_Rifle, Idle_Blinking_Rifle, Jump_Loop_Rifle,
    Jump_Start_Rifle, Kicking_Rifle, Laughing_Rifle, Run_Firing_Rifle, Run_Slashing_Rifle, Run_Throwing_Rifle, Running_Rifle, 
    Slashing_Rifle, Slashing_In_The_Air_Rifle, Sliding_Rifle, Throwing_Rifle, Throwing_In_The_Air_Rifle
}

public enum CanvasName
{
    Canvas_Home,
    Canvas_GamePlay,
    Canvas_LoseDialog,
    Canvas_PauseDialog,
    Canvas_WinDialog,
    Canvas_Inventory,
    Canvas_Settings,
    Canvas_Shop,
    Canvas_SelectMap,
    Canvas_Transition,
    Canvas_MessageDialog,

}


public enum GameConsts
{
    VOLUMESFX,
    VOLUMEMUSIC,
    FPSLimit,
    Coins,
    Hearts,
}

public enum ItemType
{
    Ammo,
    SupportItem,
    Weapons,
}

public enum ParticleName
{
    Healing,
    Debuff,
    Buff,
    ElectroSlash,
    Shield,
}
public enum GemType
{
    Clear, Yellow, Green, Blue, Pink, Red,
}

public enum MapName
{
    Mossy,
    Cave,
}

public enum ProgessLevel
{
    UNLOCKEDLEVEL,
    STARS,
    CURRENTLEVEL,
}

public enum PlatformType { None, Linear, RotatingBucket }
public enum StatType
{
    Damage,
    CriticalRate,
    CriticalDamage,
    Defense,
    CooldownRate,
    BonusHP,
    BonusSpeed
}


