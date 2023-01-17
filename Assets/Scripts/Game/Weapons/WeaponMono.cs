using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponType
{
    Melee, Ranged
}

public class WeaponMono : MonoBehaviour
{
    public WeaponType Type;
    
}
