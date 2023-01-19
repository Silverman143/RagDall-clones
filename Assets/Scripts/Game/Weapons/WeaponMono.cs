using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponType
{
    Melee, Ranged, Axe, Piker, Non
}

[System.Serializable]
public enum AttackAnimation
{
    HitTree, Attack
}

[System.Serializable]
public enum IdleAnimation
{
    Non, PunchIdle, 
}

public class WeaponMono : MonoBehaviour
{
    public WeaponType Type;
    public IdleAnimation IdleAnimation;
    public AttackAnimation AttackAnimation;
    public int DamageForce = 10;

}
