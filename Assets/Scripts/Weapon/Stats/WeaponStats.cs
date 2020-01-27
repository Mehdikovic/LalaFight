using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class WeaponStats : ScriptableObject
{
    public WeaponDamage damage;
    public WeaponAccuracy accuracy;
    public WeaponBulletSpeed bulletSpeed;
    public WeaponFireRate fireRate;
    public WeaponMagazineSize magazineSize;
    public WeaponReloadTime reloadTime;
    public WeaponScopeRange scopeRange;

    protected Dictionary<string, IWeaponProperty> _properties = null;

    protected virtual void OnEnable()
    {
        _properties = new Dictionary<string, IWeaponProperty>();
        _properties.Add("damage", damage);
        _properties.Add("accuracy", accuracy);
        _properties.Add("bulletSpeed", bulletSpeed);
        _properties.Add("fireRate", fireRate);
        _properties.Add("magazineSize", magazineSize);
        _properties.Add("reloadTime", reloadTime);
        _properties.Add("scopeRange", scopeRange);
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
        sb.Append("magazine Size: ").Append(magazineSize.value).AppendLine();
        sb.Append("reloadTime: ").Append(reloadTime.value).AppendLine();
        sb.Append("scopeRange: ").Append(scopeRange.value).AppendLine();
        return sb.ToString();
    }
}