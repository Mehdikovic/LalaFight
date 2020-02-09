using UnityEngine;


namespace LalaFight
{
    public class ParticleDestroyer : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            Destroy(gameObject, _particleSystem.main.startLifetimeMultiplier);
        }
    }
}