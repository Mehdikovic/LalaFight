using UnityEngine;


namespace LalaFight
{
    public class MultipleProjectileWeapon : Weapon
    {
        [Header("FireMode")]
        [Tooltip("Burst Fire mode is not recomended here")]
        [SerializeField] private FireModeType _fireMode = FireModeType.Semi;
        
        float[] shots;
        private MultipleProjectileWeaponStat _multipleShotStat;

        protected override void Awake()
        {
            base.Awake();

            _multipleShotStat = _stats as MultipleProjectileWeaponStat;
            shots = new float[_multipleShotStat.bulletPerShot.value];
            int shotNumbers = _multipleShotStat.bulletPerShot.value;

            for (int i = 0; i < shotNumbers; ++i)
                shots[i] = Utility.Map(i, 0, shotNumbers - 1, -2, 2);
        }

        public override FireModeType GetCurrentFireMode() => _fireMode;

        protected override void HandleShootInputs()
        {
            if (_fireMode == FireModeType.Auto)
            {
                if (Input.GetButton("Fire1"))
                    Shoot();
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                    Shoot();
            }
        }

        private void Shoot()
        {

            if (CanShoot())
            {
                RaiseOnFiredBeginEvent();

                for (int i = 0; i < shots.Length; ++i)
                    CreateBullet(shots[i] * 10);

                NextShootingTime();
                magazine.DecreaseAmmo();

                RaiseOnFiredEndEvent();
            }
        }
    }
}