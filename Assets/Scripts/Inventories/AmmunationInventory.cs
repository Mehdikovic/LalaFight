using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmunationInventory : MonoBehaviour
{
    [SerializeField] private int _startingPistolAmmo = 40;
    [SerializeField] private int _startingSMGAmmo = 120;
    [SerializeField] private int _startingShotgunAmmo = 32;
    [SerializeField] private int _startingRifleAmmo = 90;

    private Dictionary<WeaponType, IntObject> _bullets;
    
    public void Init()
    {
        _bullets = new Dictionary<WeaponType, IntObject>();
        _bullets.Add(WeaponType.Pistol, new IntObject() { Value = _startingPistolAmmo });
        _bullets.Add(WeaponType.SMG, new IntObject() { Value = _startingSMGAmmo });
        _bullets.Add(WeaponType.Shotgun, new IntObject() { Value = _startingShotgunAmmo });
        _bullets.Add(WeaponType.Rifle, new IntObject() { Value = _startingRifleAmmo });
    }

    public Dictionary<WeaponType, IntObject> Ammunation => _bullets;
}