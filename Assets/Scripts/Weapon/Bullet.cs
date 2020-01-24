using System;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask _bulletIntractionLayerMask = new LayerMask();

    private float _bulletSpeed;
    private int _bulletDamage;

    public event Action<Collider, Vector3, Vector3> Collided;

    private void Start()
    {
        CheckCollision();
    }

    private void Update()
    {
        float moveDistance = _bulletSpeed * Time.deltaTime;
        CheckCollision(moveDistance);
        transform.Translate(moveDistance * Vector3.forward);
    }

    protected void CheckCollision()
    {
        var colliders = Physics.OverlapSphere(transform.position, 0.15f, _bulletIntractionLayerMask, QueryTriggerInteraction.Collide);
        if (colliders.Length > 0)
            HandleColliderObject(colliders[0], transform.position);
    }

    protected void CheckCollision(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, moveDistance, _bulletIntractionLayerMask, QueryTriggerInteraction.Collide))
            HandleColliderObject(hit.collider, hit.point);
    }

    private void HandleColliderObject(Collider collider, Vector3 hitPoint)
    {
        var enemy = collider.GetComponent<IDamageable>();
        if (enemy != null)
        {
            enemy.TakeDamage(_bulletDamage, transform.position, transform.forward);
        }
        Collided?.Invoke(collider, hitPoint, transform.forward);
        Destroy(gameObject);
    }

    public void Initialize(Weapon weapon, float rotationAmount)
    {
        _bulletSpeed = weapon.bulletSpeed;
        _bulletDamage = weapon.damage;

        float accuracyFlaw = 0f;
        if (UnityEngine.Random.value > weapon.accuracy)
             accuracyFlaw = UnityEngine.Random.Range(-8f, 8f);

        transform.Rotate(Vector3.up, rotationAmount + accuracyFlaw);

        Destroy(gameObject, 5f);
    }
}