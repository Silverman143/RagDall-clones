using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Create new PlayerData")]
public class PlayerData : ScriptableObject
{
    public int MaxHealth;
    public int CurrentHealth;
    public int DamageForce;
    public int Speed;
}

