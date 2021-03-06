using System;
using UnityEngine;


namespace LalaFight
{
    public class SingleProjectileWeapon : Weapon
    {
        [Header("FireModes")]
        [SerializeField] private FireModeType[] _fireModes = new FireModeType[] { FireModeType.Auto };

        private bool _triggerReleasedSinceLastShot;
        private int _burstRemainingShoot;

        private int _currentFireModeIndex = 0;
        private SingleProjectileWeaponStat _singleShotStat;

        //GETTERS AND SETTERS
        private FireModeType CurrentFireMode => _fireModes[_currentFireModeIndex];

        public override FireModeType GetCurrentFireMode() => CurrentFireMode;

        private void SwapFireMode()
        {
            if (_fireModes.Length < 2)
                return;

            ++_currentFireModeIndex;
            _currentFireModeIndex %= _fireModes.Length;
            
            RaiseFireModeSwapped();
        }

        protected override void Awake()
        {
            base.Awake();
            _singleShotStat = _stats as SingleProjectileWeaponStat;

            _burstRemainingShoot = _singleShotStat.bulletPerBurstShot.value;
            _triggerReleasedSinceLastShot = true;
        }

        protected override void HandleShootInputs()
        {
            if (Input.GetButtonDown("SwapFireMode"))
                SwapFireMode();

            if (Input.GetButton("Fire1"))
                OnTriggerHold();
            else if (Input.GetButtonUp("Fire1"))
                OnTriggerRelease();
        }

        private void Shoot()
        {

            if (CanShoot())
            {
                RaiseOnFiredBeginEvent();

                if (CurrentFireMode == FireModeType.Burst)
                {
                    if (_burstRemainingShoot == 0)
                        return;
                    --_burstRemainingShoot;
                }
                else if (CurrentFireMode == FireModeType.Semi)
                {
                    if (_triggerReleasedSinceLastShot == false)
                        return;
                }

                CreateBullet();
                NextShootingTime();
                magazine.DecreaseAmmo();

                RaiseOnFiredEndEvent();
            }
        }

        private void OnTriggerHold()
        {
            Shoot();
            _triggerReleasedSinceLastShot = false;
        }

        private void OnTriggerRelease()
        {
            _triggerReleasedSinceLastShot = true;
            _burstRemainingShoot = _singleShotStat.bulletPerBurstShot.value;
        }
    }
}