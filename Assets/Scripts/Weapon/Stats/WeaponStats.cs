﻿using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class WeaponStats : ScriptableObject
{
    public WeaponDamage damage;
    public WeaponAccuracy accuracy;
    public WeaponBulletSpeed bulletSpeed;
    public WeaponFireRate fireRate;
    public WeaponMagazineSize magazineSize;
    public WeaponReloadSize reloadSpeed;

    protected Dictionary<string, IWeaponProperty> _properties = null;

    public WeaponStats()
    {
        _properties = new Dictionary<string, IWeaponProperty>();
        _properties.Add("damage", damage);
        _properties.Add("accuracy", accuracy);
        _properties.Add("bulletSpeed", bulletSpeed);
        _properties.Add("fireRate", fireRate);
    }

    public bool UpdateStat(string propertyName)
    {
        if (_properties.ContainsKey(propertyName) == false)
            return false;
        return _properties[propertyName].Update();
    }

    public int GetStatLevel(string propertyName) 
    {
        if (_properties.ContainsKey(propertyName) == false)
            return 0;
        return _properties[propertyName].GetLevel();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("damage: ").Append(damage.value).AppendLine();
        sb.Append("accuracy: ").Append(accuracy.value).AppendLine();
        sb.Append("bulletSpeed: ").Append(bulletSpeed.value).AppendLine();
        sb.Append("fireRate: ").Append(fireRate.value).AppendLine();
        return sb.ToString();
    }
}