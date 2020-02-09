using System.Text;
using UnityEngine;


namespace LalaFight
{
    [CreateAssetMenu(menuName = "Weapon/StatContainer/MultipleShot")]
    public class MultipleProjectileWeaponStat : WeaponStats
    {
        public WeaponBulletPerShot bulletPerShot;

        protected override void OnEnable()
        {
            base.OnEnable();
            _properties.Add("bulletPerShot", bulletPerShot);
        }

        public override string ToString()
        {
            var sb = new StringBuilder().Append(base.ToString());
            sb.Append("bulletPerShot: ").Append(bulletPerShot.value);
            return sb.ToString();
        }
    }
}