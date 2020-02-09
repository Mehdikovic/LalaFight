using UnityEngine;


namespace LalaFight
{
    [CreateAssetMenu(menuName = "Weapon/WeaponStat/Accuracy")]
    public class WeaponAccuracy : WeaponStatProperty<float>
    {
        private float _min = 0f;
        private float _max = 1f;
        public override float value => Mathf.Clamp(base.value, _min, _max);
    }
}