using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [SerializeField] private float _timeBetweenInteract = 2f;

    private float _interactTimer = 0;
    

    //private Collider[] _colliders = new Collider[4];
    //TODO: add SphereColision for pooling objects
    private Stack<Collider> _colliders = new Stack<Collider>();

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
        if (_interactTimer < Time.time)
        {
            _interactTimer = Time.time + _timeBetweenInteract;
            while (_colliders.Count > 0)
            {
                Interactable intractable = _colliders.Pop().GetComponent<Interactable>();
                intractable.Interact(transform);
            }
        }
    }
}
