using System.Collections;
using UnityEngine;


namespace LalaFight
{
    public class BulletShellVFX : MonoBehaviour
    {

        [SerializeField] private float _minForce = 90f;
        [SerializeField] private float _maxForce = 130f;
        [SerializeField] private float _lifetime = 6f;


        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            float force = Random.Range(_minForce, _maxForce);
            _rigidbody.AddForce(transform.right * force);
            _rigidbody.AddTorque(Random.insideUnitSphere * force);
            StartCoroutine(FadeOutAndDestroy());
        }

        private IEnumerator FadeOutAndDestroy()
        {
            float speed = 1 / _lifetime;
            float percent = 0;
            Material material = GetComponent<Renderer>().material;
            Color originalColor = material.color;

            while (percent < 1f)
            {
                material.color = Color.Lerp(originalColor, Color.clear, percent);
                percent += Time.deltaTime * speed;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}