using UnityEngine;


namespace LalaFight
{
    public class InteractableController : MonoBehaviour
    {
        [Header("Interact")]
        [SerializeField] private LayerMask _interactableMask = new LayerMask();
        [SerializeField] private float _timeBetweenInteract = 0.2f;

        private float _interactTimer = 0;
        private Vector3 _position = Vector3.zero;
        private float _radius = 2f;

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

                _position = transform.position + Vector3.forward;

                var colliders = Physics.OverlapSphere(_position, _radius, _interactableMask, QueryTriggerInteraction.Collide);
                if (colliders.Length > 0)
                    colliders[0].GetComponent<Interactable>().Interact(transform);
            }
        }
    }
}