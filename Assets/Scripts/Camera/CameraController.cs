using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerInput _targetInput = null;
    private Transform _targetTransform = null;

    [Header("Speed")]
    [SerializeField] private float _rotationSpeed = 360f;
    [SerializeField] private float _panSpeed = 10f;

    [Header("Posision")]
    [SerializeField] private float _distance = 4f;
    [SerializeField] private float _height = 16f;
    [SerializeField] private float _pitch = 8f;
    
    private float _angle = 0f;
    private float _distanceToTarget;

    public void SetTarget(PlayerInput target)
    {
        _targetInput = target;
        _targetTransform = _targetInput.transform;
    }

    public void PositionCamera(float angle)
    {
        _angle += (angle * _rotationSpeed);
        Vector3 wantedPosition = Vector3.forward * _distance + Vector3.up * _height;
        Vector3 rotatedWantedPosition = Quaternion.AngleAxis(_angle, Vector3.up) * wantedPosition;

        if (_targetInput == null)
            return;
        transform.position = _targetTransform.position + rotatedWantedPosition;

        _distanceToTarget = _targetTransform.position.sqrLengthTo(transform.position);

        Vector3 cameraForward = transform.forward.Flat().normalized;
        transform.LookAt(_targetTransform.position + cameraForward * _pitch);
    }

    public bool SetCameraOriginalPosition()
    {
        if (_targetInput == null)
            return false;
        
        if ((_targetTransform.position.sqrLengthTo(transform.position)) <= _distanceToTarget + 15)
            return true;

        Vector3 wantedPosition = Vector3.forward * _distance + Vector3.up * _height;
        Vector3 rotatedWantedPosition = Quaternion.AngleAxis(_angle, Vector3.up) * wantedPosition;

        transform.position = Vector3.SlerpUnclamped(transform.position, _targetTransform.position + rotatedWantedPosition, .1f);
        return false;
    }

    public void PanCamera(Vector3 pan)
    {
        pan *= _panSpeed;
        Vector3 camRight = transform.right.Flat().normalized;
        Vector3 camForward = transform.forward.Flat().normalized;
        var d = camRight * pan.x + camForward * pan.z;
        if (_targetInput == null || (_targetTransform.position.sqrLengthTo(transform.position + d)) > Mathf.Pow(_targetInput.GetPlayerActiveRange(), 2))
            return;
        //transform.position += d;
        transform.position = Vector3.Lerp(transform.position, transform.position + d, 0.8f);
    }
}
