using UnityEngine;

public class MultipleProjectileWeapon : Weapon
{
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

    protected override void HandleShootInputs()
    {
        if (Input.GetButtonDown("Fire1"))
            Shoot();
    }

    private void Shoot()
    {
        RaiseOnFiredBeginEvent();
        if (CanShoot())
        {
            NextShootingTime();
            for (int i = 0; i < shots.Length; ++i)
                CreateBullet(shots[i] * 10);
            RaiseOnFiredEndEvent();
        }
    }
}
