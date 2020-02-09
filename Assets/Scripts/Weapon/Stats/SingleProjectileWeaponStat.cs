using System.Text;
using UnityEngine;


namespace LalaFight
{
    [CreateAssetMenu(menuName = "Weapon/StatContainer/SingleShot")]
    public class SingleProjectileWeaponStat : WeaponStats
    {
        public WeaponBulletPerBurstShot bulletPerBurstShot;

        protected override void OnEnable()
        {
            base.OnEnable();
            _properties.Add("bulletPerBurstShot", bulletPerBurstShot);
        }

        public override string ToString()
        {
            var sb = new StringBuilder().Append(base.ToString());
            sb.Append("bulletPerBurstShot: ").Append(bulletPerBurstShot.value);
            return sb.ToString();
        }
    }
}