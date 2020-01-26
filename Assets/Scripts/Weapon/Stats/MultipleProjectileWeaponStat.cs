using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/StatContainer/MultipleShot")]
public class MultipleProjectileWeaponStat : WeaponStats
{
    public WeaponBulletPerShot bulletPerShot;

    public MultipleProjectileWeaponStat() : base()
    {
        _properties.Add("bulletPerShot", bulletPerShot);
    }

    public override string ToString()
    {
        var sb = new StringBuilder().Append(base.ToString());
        sb.Append("bulletPerShot: ").Append(bulletPerShot.value);
        return sb.ToString();
    }
}
