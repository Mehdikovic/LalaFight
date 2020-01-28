using System.Collections.Generic;
using UnityEngine;

public class IntractableController : MonoBehaviour
{
    [SerializeField] private float _timeBetweenIntract = 2f;

    private float _intractTimer = 0;
    
    private PlayerController _playerController;

    //private Collider[] _colliders = new Collider[4];

    private Stack<Collider> _colliders = new Stack<Collider>();
    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_colliders.Count > 3)
            return;
        if (_colliders.Contains(other))
            return;
        
        _colliders.Push(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_colliders.Count > 0)
            _colliders.Pop();
    }

    public void Tick()
    {
        if (Input.GetButtonDown("Intract"))
            IntreactWith();
    }

    private void IntreactWith()
    {
        if (_intractTimer < Time.time)
        {
            _intractTimer = Time.time + _timeBetweenIntract;
            while (_colliders.Count > 0)
            {
                Intractable intractable = _colliders.Pop().GetComponent<Intractable>();
                intractable.Use(_playerController);
            }
        }
    }
}
