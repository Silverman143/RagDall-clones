using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private List<WeaponMono> _weapons;
    [SerializeField] private WeaponMono _currentWeapon;
    [SerializeField] private WeaponMono _axe;
    [SerializeField] private WeaponMono _picker;
    [SerializeField] private float _meleeDistance = 2;
    [SerializeField] private float _rangeDistance = 5;

    private bool _isActionWeapon = false;

    public WeaponType GetCurrentWeaponType() => _currentWeapon.Type;
    public WeaponMono GetCurretnWeapon() => _currentWeapon;



    public void ChangeWeapon(WeaponMono weaponTo)
    {
        _currentWeapon.gameObject.SetActive(false);
        _currentWeapon = weaponTo;
        _currentWeapon.gameObject.SetActive(true);
    }

    private void ActivateAxe(bool value)
    {
        _currentWeapon.gameObject.SetActive(!value);
        _axe.gameObject.SetActive(value);
    }

    private void ActivatePicker(bool value)
    {
        _currentWeapon.gameObject.SetActive(!value);
        _picker.gameObject.SetActive(value);
    }

    public void ActivateActionWeapon(ActionType type)
    {
        if(type == ActionType.Tree)
        {
            ActivateAxe(true);
        }
        else if (type == ActionType.Ore)
        {
            ActivatePicker(true);
        }

        _isActionWeapon = true;
    }

    public void DeactivateActionWeapons()
    {
        ActivateAxe(false);
        ActivatePicker(false);
        _isActionWeapon = false;
    }

    public string GetIdleAnimationName()
    {
        return _currentWeapon.IdleAnimation.ToString();

    }

    public int GetWeaponDamageForce()
    {
        return _currentWeapon.DamageForce;
    }

    public float GetWeaponAttackDistance()
    {
        if (_currentWeapon.Type == WeaponType.Melee) return _meleeDistance;
        else return _rangeDistance;
    }

    //public string GetAttackAnimationName()
    //{
    //    if (_currentWeapon != null) return _currentWeapon.IdleAnimation.ToString();
    //    else return "";
    //}
}
