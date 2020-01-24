using UnityEngine;

public class ShootCursorController : MonoBehaviour
{
    [SerializeField] private Sprite _cursorImage = null;
    private GameObject _centerPoint = null;

    private Transform _cameraTransform = null;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        Weapon weapon = GetComponentInParent<Weapon>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        weapon.OnWeaponLoaded += OnWeaponLoaded;
        weapon.OnWeaponUnloaded += OnWeaponUnloaded;
        weapon.OnCursorPositionReceived += SetCursorPosition;

        _cameraTransform = CameraDatabase.MainCamera.transform;
        _centerPoint = transform.GetChild(0).gameObject;
        _centerPoint.SetActive(false);

        _spriteRenderer.sprite = _cursorImage;
    }

    private void OnWeaponUnloaded()
    {
        _spriteRenderer.sprite = null;
    }

    private void OnWeaponLoaded()
    {
        _spriteRenderer.sprite = _cursorImage;
    }

    private void SetCursorPosition(Vector3 rayHitPoint, bool aimOnEnemy)
    {
        transform.position = rayHitPoint;
        transform.forward = -_cameraTransform.forward;

        if (aimOnEnemy)
            _centerPoint.SetActive(true);
        else
            _centerPoint.SetActive(false);
    }
}
