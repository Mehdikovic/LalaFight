using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName ="Weapon/StatContainer/SingleShot")]
public class SingleProjectileWeaponStat : WeaponStats
{
    public WeaponBulletPerBurstShot bulletPerBurstShot;

    public SingleProjectileWeaponStat() : base()
    {
        _properties.Add("bulletPerBurstShot", bulletPerBurstShot);
    }

    public override string ToString()
    {
        var sb = new StringBuilder().Append(base.ToString());
        sb.Append("bulletPerBurstShot: ").Append(bulletPerBurstShot.value);
        return sb.ToString();
    }
}
