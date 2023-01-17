using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private List<WeaponMono> _weapons;
    [SerializeField] private WeaponMono _currentWeapon;

    public WeaponType CurrentWeaponType() => _currentWeapon.Type;
}
