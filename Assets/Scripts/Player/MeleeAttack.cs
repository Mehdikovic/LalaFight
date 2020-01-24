using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Transform _meleeAttackPoint = null;
    [SerializeField] private Transform _meleeAttackWeapon = null;

    [Header("Enemy LayerMask")]
    [SerializeField] private LayerMask _enemyLayer = new LayerMask();

    [Header("Melee Attack Properties")]
    [Range(0.35f, 1.2f)]
    [SerializeField] private float _attackRate = 0.8f;
    [Range(0.15f, 1f)]
    [SerializeField] private float _freezTime = 4;
    [Range(1, 5)]
    [SerializeField] private int _affectedEnemy = 3;

    private WeaponManager _weaponManager;
    private float _meleeAttackTime;
    private Vector3 _boxSize; //Vector3.one * 2 + Vector3.forward * 0.5f + Vector3.right;

    public event Action Attacked;

    private void Awake()
    {
        _boxSize = new Vector3(3, 2.5f, 1.5f);
        _weaponManager = GetComponent<WeaponManager>();
        _meleeAttackWeapon.gameObject.SetActive(false);
    }

    public void Attack()
    {
        if (Time.time > _meleeAttackTime)
        {
            _meleeAttackTime = Time.time + _attackRate;
            _weaponManager.HideWeapon();
            StartCoroutine(MeleeAttackAnimation());
        }
    }

    private IEnumerator MeleeAttackAnimation()
    {
        float time = 0;
        _meleeAttackWeapon.gameObject.SetActive(true);
        _meleeAttackWeapon.localRotation = Quaternion.Euler(Vector3.up * -70);
        Quaternion targetRotation = Quaternion.Euler(Vector3.up * 70);

        AttackToEnemies();

        while(time < _attackRate)
        {
            time += Time.deltaTime * 2;
            _meleeAttackWeapon.localRotation = Quaternion.Slerp(_meleeAttackWeapon.localRotation, targetRotation, time / _attackRate);
            yield return null;
        }

        _meleeAttackWeapon.gameObject.SetActive(false);
        _weaponManager.ShowWeapon();
        Attacked?.Invoke();
    }

    private void AttackToEnemies()
    {
        var enemies = Physics.OverlapBox(_meleeAttackPoint.position, _boxSize, Quaternion.identity, _enemyLayer, QueryTriggerInteraction.Collide);
        var enemyAttackableCount = enemies.Length >= _affectedEnemy ? _affectedEnemy : enemies.Length;
        for (int i = 0; i < enemyAttackableCount; ++i)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            IDamageable enemyDamageable = enemies[i].GetComponent<IDamageable>();
            enemyDamageable.TakeDamage(1, enemy.transform.position, -enemy.transform.forward);
            enemy.FreezeEnemy(_freezTime);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(_meleeAttackPoint.position, _boxSize);
    }
}
