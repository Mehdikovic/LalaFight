using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HealthController))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask _lineOfSightLayer = new LayerMask();
    [SerializeField] private float _timeBetweenPathFinding = 0.25f;
    [SerializeField] private float _timeBetweenAttack = 3f;
    [SerializeField] private float _attackSpeed = 2f;
    
    private int _damage = 2;
    private Transform _target;
    private NavMeshAgent _navAgent = null;
    private HealthController _healthController;
    private Coroutine _pursueTargetCoroutine;
    private Coroutine _attackCoroutine;
    private MeshRenderer _meshRenderer;
    private float _nextAttackTime;
    private IDamageable _targetDamageable;

    public event System.Action OnFreezeBegin;
    public event System.Action OnFreezeEnd;

    #region UNITY_METHODS
    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _healthController = GetComponent<HealthController>();
        _healthController.Dead += OnEnemyDeath;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetProperties(Wave wave)
    {
        _navAgent.speed = wave.enemySpeed;
        _healthController.MaxHealth = wave.enemyHealth;
        _damage = wave.enemyDamage;
        _timeBetweenAttack = wave.enemyAttackRate;
        _attackSpeed = wave.enemyAttackSpeed;
        var newColor = Color.Lerp(wave.enemyColor1, wave.enemyColor2, UnityEngine.Random.value);
        _meshRenderer.material.color = newColor;
    }

    private void Start()
    {
        _pursueTargetCoroutine = StartCoroutine(PursueTarget());
    }

    private void Update()
    {
        if (_navAgent.enabled == false)
            return;

        if (_target && _targetDamageable.IsDead == false && TargetInFOV() && TargetInAttackRange())
        {
            if (Time.time > _nextAttackTime && TargetAtLOS())
            {
                _nextAttackTime = Time.time + _timeBetweenAttack;
                _attackCoroutine = StartCoroutine(AttackToTarget());
            }
        }
    }
    #endregion

    #region CUSTOM_METHODS
    public void SetTarget(Transform target)
    {
        _target = target;
        if (_target)
            _targetDamageable = _target.GetComponent<IDamageable>();
    }

    private IEnumerator AttackToTarget()
    {
        Color color = _meshRenderer.material.color;

        StopCoroutine(_pursueTargetCoroutine);

        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = _target.position;

        float percent = 0f;
        while (percent <= 1f && _navAgent.enabled)
        {
            percent += Time.deltaTime * _attackSpeed;
            float interpolation = Utility.Interpolate(percent);
            transform.position = Vector3.Lerp(originalPosition, targetPosition, interpolation);
            _meshRenderer.material.color = UnityEngine.Random.ColorHSV();
            yield return null;
        }

        if (_target)
            _targetDamageable.TakeDamage(_damage, _target.position, -_target.forward);
        _meshRenderer.material.color = color;
        _pursueTargetCoroutine = StartCoroutine(PursueTarget());
    }

    private IEnumerator PursueTarget()
    {
        while (_target != null)
        {
            Vector3 targetPos = _target.position.Flat();
            if (_navAgent.enabled)
                _navAgent.SetDestination(targetPos);
            yield return new WaitForSeconds(_timeBetweenPathFinding);
        }
    }

    public void FreezeEnemy(float freezeTime)
    {
        OnFreezeBegin?.Invoke();
        _navAgent.enabled = false;
        StartCoroutine(MoveBack(freezeTime));
    }

    

    private IEnumerator MoveBack(float freezeTime)
    {
        /*
        float time = 0f;
        Vector3 originalPosition = transform.position;
        Vector3 destination = transform.position + _target.forward * pushAmount;
        while (time <= 1f)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(originalPosition, destination, time);
            yield return null;
        }
        */
        yield return new WaitForSeconds(freezeTime);
        OnFreezeEnd?.Invoke();
        _navAgent.enabled = true;
    }

    private void OnEnemyDeath()
    {
        if (_pursueTargetCoroutine != null)
            StopCoroutine(_pursueTargetCoroutine);
        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);
    }

    private bool TargetInAttackRange()
    {
        return _target.sqrLengthTo(transform) <= Mathf.Pow(_navAgent.stoppingDistance, 2);
    }

    private bool TargetInFOV()
    {
        return true;
    }

    private bool TargetAtLOS()
    {
        Vector3 playerPosition = _target.position;
        Vector3 enemyPosition = transform.position;
        Ray ray = new Ray(enemyPosition, enemyPosition.DirectionTo(playerPosition));
        if (Physics.Raycast(ray, out RaycastHit hit, _navAgent.stoppingDistance, _lineOfSightLayer, QueryTriggerInteraction.Collide))
            return hit.collider.tag == _target.tag;
        return false;
    }
    #endregion
}
