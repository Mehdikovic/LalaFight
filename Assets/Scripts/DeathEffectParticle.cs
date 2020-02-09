using UnityEngine;


namespace LalaFight
{
    [RequireComponent(typeof(HealthController))]
    public class DeathEffectParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _deathEffect = null;

        private HealthController _healthController;

        private void Awake()
        {
            _healthController = GetComponent<HealthController>();
            _healthController.OnFinalDamage += ShowParticleEffect;
        }


        private void ShowParticleEffect(Vector3 position, Vector3 direction)
        {
            var particle = Instantiate(_deathEffect, position, Quaternion.FromToRotation(Vector3.forward, direction));
            particle.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        }
    }
}