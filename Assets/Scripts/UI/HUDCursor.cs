using UnityEngine;


namespace LalaFight
{
    public class HUDCursor : MonoBehaviour
    {
        [SerializeField] private Sprite _defaultCursor = null;
        [SerializeField] private GameObject _cursorHolder = null;
        [SerializeField] private GameObject _aimPointHolder = null;


        private SpriteRenderer _spriteRenderer = null;
        private WeaponManager _weaponManager = null;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;

            GetComponent<PlayerInput>().OnHitPointSet += OnHitPointSet;
            
            _weaponManager = GetComponent<WeaponManager>();

            _weaponManager.WeaponHideStateChanged += OnWeaponHideStateChanged;
            _weaponManager.WeaponAdded += WeaponAdded;
            _weaponManager.WeaponSwapped += WeaponSwapped;

            _aimPointHolder.SetActive(false);
            _cursorHolder.SetActive(true);
            
            _spriteRenderer = _cursorHolder.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _defaultCursor;

        }

        private void WeaponSwapped(Weapon oldWeapon, Weapon currentWeapon)
        {
            SetWeaponCursorAsCursor(currentWeapon);
        }

        private void WeaponAdded(Weapon newWeapon)
        {
            SetWeaponCursorAsCursor(newWeapon);
        }

        private void OnHitPointSet(Vector3 hitPoint, Vector3 cameraForward, bool aimOnEnemy)
        {
            _cursorHolder.transform.position = hitPoint;
            _cursorHolder.transform.forward = -cameraForward;

            if (_weaponManager.isWeaponHided == false && aimOnEnemy)
                _aimPointHolder.SetActive(true);
            else
                _aimPointHolder.SetActive(false);
        }

        private void OnWeaponHideStateChanged(bool isHided)
        {
            if (isHided)
                _spriteRenderer.sprite = _defaultCursor;
            else
                SetWeaponCursorAsCursor(_weaponManager.currentWeapon);
        }

        private void SetWeaponCursorAsCursor(Weapon weapon)
        {
            if (weapon == null)
                _spriteRenderer.sprite = _defaultCursor;

            _spriteRenderer.sprite = weapon.cursor ?? _defaultCursor;
        }
    }
}