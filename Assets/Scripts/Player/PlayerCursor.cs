using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField] private GameObject _playerCursor = null;
    void Awake()
    {
        GetComponent<WeaponManager>().WeaponHideStateChanged += OnWeaponHideStateChanged;
        GetComponent<PlayerInput>().OnHitPointSet += OnHitPointSet;
        _playerCursor.SetActive(false);
    }

    private void OnHitPointSet(Vector3 hitPoint, Vector3 cameraForward) 
    {
        _playerCursor.transform.position = hitPoint;
        _playerCursor.transform.forward = -cameraForward;
    }

    private void OnWeaponHideStateChanged(bool isHided)
    {
        _playerCursor.SetActive(isHided);
    }
}
