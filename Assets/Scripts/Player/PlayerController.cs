using System;
using UnityEngine;




namespace LalaFight
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _playerSpeed = 30f;
        private Vector3 _velocity;
        private Rigidbody _rigidBody;

        public Action OnPlayerFall;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (transform.position.y <= -.5f)
                OnPlayerFall?.Invoke();
        }

        private void FixedUpdate()
        {
            _rigidBody.MovePosition(_rigidBody.position + _velocity * Time.fixedDeltaTime);
        }

        public void Move(Vector3 velocity)
        {
            _velocity = velocity * _playerSpeed;
        }

        public void LookAt(Vector3 point)
        {
            //_rigidBody.MoveRotation(Quaternion.LookRotation(transform.position.DirectionTo(point.WithY(1))));
            transform.LookAt(point.WithY(1));
        }
    }
}
