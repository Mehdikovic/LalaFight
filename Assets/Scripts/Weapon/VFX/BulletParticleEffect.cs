using UnityEngine;


namespace LalaFight
{
    public class BulletParticleEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _bulletCollisionEffect = null;

        private Bullet _bullet;

        private void Awake()
        {
            _bullet = GetComponent<Bullet>();
            _bullet.Collided += OnBulletCollided;
        }

        private void OnBulletCollided(Collider collider, Vector3 position, Vector3 bulletForward)
        {
            var particle = Instantiate(_bulletCollisionEffect, position, Quaternion.FromToRotation(Vector3.forward, bulletForward));
            particle.GetComponent<Renderer>().material.color = collider.GetComponent<Renderer>().material.color;
        }
    }
}