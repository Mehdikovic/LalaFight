using System;
using System.Collections;
using UnityEngine;


namespace LalaFight
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private string _modelName = "ex: m4a1";
        [Tooltip("If null default value will be set")]
        [SerializeField] private Sprite _cursor = null;
        [SerializeField] private Sprite _sprite = null;
        [SerializeField] private WeaponType _type = WeaponType.Pistol;
        [SerializeField] private BulletType _bulletType = BulletType.Medium;

        [Header("Bullet")]
        [SerializeField] private Transform _muzzle = null;
        [SerializeField] private Bullet _bulletPrefab = null;

        [Header("Weapon Stat[Single | Multiple]")]
        [SerializeField] protected WeaponStats _stats = null;

        private float _nextShootingTime = 0f;
        private bool _isAnimating = false;
        private IMagazineController _nullMagazine = new NullMagazine();
        private Transform _playerOwner = null;
        private int _currentMagazine = 0;
        private AmmoManager _ammoManager;

        //TODO: add equipment manager to modifying the stats' values
        //GETTERS AND SETTERS
        public string modelName => _modelName;
        public Sprite cursor => _cursor;
        public Sprite sprite => _sprite;
        public WeaponType type => _type;
        public BulletType bulletType => _bulletType;
        public Transform playerOwner => _playerOwner;
        public IMagazineController magazine => GetComponent<IMagazineController>() ?? _nullMagazine;

        public int ammo
        {
            get 
            {
                return _ammoManager == null ? 0 : _ammoManager.GetAmmo(_bulletType);
            }
            set
            {
                if (_ammoManager != null)
                    _ammoManager.SetAmmo(_bulletType, value);
            }
        }


        public int currentMagazine
        {
            get => _currentMagazine;
            set => _currentMagazine = Mathf.Clamp(value, 0, magazieSize);
        }
        public float accuracy => _stats.accuracy.value;
        public int damage => _stats.damage.value;
        public float bulletSpeed => _stats.bulletSpeed.value;
        public float fireRate => _stats.fireRate.value;
        public int magazieSize => _stats.magazineSize.value;
        public float reloadTime => _stats.reloadTime.value;
        public float scopeRange => _stats.scopeRange.value;

        public bool isAnimating => _isAnimating;

        //EVENTS
        public event Action<bool> OnOwnerChanged;
        public event Action OnFireBegin;
        public event Action OnFireEnd;
        public event Action OnWeaponLoaded;
        public event Action OnWeaponUnloaded;
        public event Action<FireModeType> FireModeChanged;

        // UNITY CALLBACKS
        protected virtual void Awake()
        {
            var subscribers = GetComponents<IOnObjectNotifier<Weapon>>();
            foreach (var sub in subscribers)
            {
                sub.OnAwakeCalled(this);
            }
        }

        //CUSTOM METHODS
        protected abstract void HandleShootInputs();

        public virtual void HandleUpdateInputs()
        {
            if (Input.GetButtonDown("Reload"))
                magazine.Reload();

            HandleShootInputs();
        }

        protected void RaiseOnFiredBeginEvent()
        {
            OnFireBegin?.Invoke();
        }

        protected void RaiseOnFiredEndEvent() => OnFireEnd?.Invoke();

        protected bool CanShoot() => magazine.ShootingAllowed() && _isAnimating == false && Time.time > _nextShootingTime;

        protected void NextShootingTime() => _nextShootingTime = Time.time + fireRate;

        public virtual void FastLoad(bool _isWeaponHided)
        {
            gameObject.SetActive(true);
            OnWeaponLoaded?.Invoke();
            gameObject.SetActive(!_isWeaponHided);
        }

        public virtual void FastUnload()
        {
            magazine.CancelReloading();
            OnWeaponLoaded?.Invoke();

            gameObject.SetActive(false);
        }

        public virtual void WeaponLoad()
        {
            if (_isAnimating == true)
                return;

            _isAnimating = true;
            gameObject.SetActive(true);

            OnWeaponLoaded?.Invoke();

            StartCoroutine(LoadAnimation(60, 0, true));
        }

        public virtual void WeaponUnload()
        {
            if (_isAnimating == true)
                return;

            _isAnimating = true;

            magazine.CancelReloading();
            OnWeaponUnloaded?.Invoke();

            StartCoroutine(LoadAnimation(0, 60, false));
        }

        private IEnumerator LoadAnimation(float angleFrom, float angleTo, bool loaded)
        {
            //TODO: grab this 5 value from player stat
            float speed = 2;

            float percent = 0f;
            while (percent < 1)
            {
                percent += Time.deltaTime * speed;
                transform.localEulerAngles = Vector3.right * Mathf.Lerp(angleFrom, angleTo, percent);
                yield return null;
            }

            _isAnimating = false;
            gameObject.SetActive(loaded);
        }

        public void SetOwner(Transform owner)
        {
            if (_playerOwner != owner)
                OnOwnerChanged?.Invoke(owner != null);
            _playerOwner = owner;
            _ammoManager = _playerOwner?.GetComponent<AmmoManager>();
        }

        protected void CreateBullet(float rotation = 0)
        {
            var bullet = Instantiate(_bulletPrefab, _muzzle.position, _muzzle.rotation);
            bullet.Initialize(this, rotation);
        }

        protected void RaiseFireModeSwapped()
        {
            FireModeChanged?.Invoke(GetCurrentFireMode());
        }

        public abstract FireModeType GetCurrentFireMode();
    }
}