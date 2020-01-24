using UnityEngine;

public class MultipleProjectileWeapon : Weapon
{
    [Header("Bullet per shot")]
    [SerializeField] private int _bulletsPerShot = 5;

    float[] shots;

    protected override void Awake()
    {
        base.Awake();

        shots = new float[_bulletsPerShot];

        for (int i = 0; i < _bulletsPerShot; ++i)
            shots[i] = Utility.Map(i, 0, _bulletsPerShot - 1, -2, 2);
    }

    protected override void HandleShootInputs()
    {
        if (Input.GetButtonDown("Fire1"))
            Shoot();
    }

    private void Shoot()
    {
        CallOnFiredBeginEvent();
        if (CanShoot())
        {
            NextShootingTime();
            for (int i = 0; i < shots.Length; ++i)
                CreateBullet(shots[i] * 10);
            CallOnFiredEndEvent();
        }
    }
}
