using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipVFX : MonoBehaviour
{
    [SerializeField] private float _lifetime = 10f;

    private Rigidbody _rigidbody = null;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(Vector3.down * 10f, ForceMode.Force);
        _rigidbody.AddTorque(Random.insideUnitSphere * 1000f, ForceMode.Force);
        Destroy(gameObject, _lifetime);
    }
}
