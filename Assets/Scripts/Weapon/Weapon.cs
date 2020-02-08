using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string _modelName = "ex: m4a1";
    [SerializeField] private WeaponType _type = WeaponType.Pistol;
    [SerializeField] private BulletType _bulletType = BulletType.Medium;

    [Header("Bullet")]
    [SerializeField] private Transform _muzzle = null;
    [SerializeField] private Bullet _bulletPrefab = null;

    [Header("Weapon Stat[Single | Multiple]")]
    [SerializeField] protected WeaponStats _stats = null;

    private float _nextShootingTime = 0f;
    private bool _isAnimating = false;
    private IMagazineController _magazine = null;
    private IMagazineController _nullMagazine = new NullMagazine();
    private Transform _playerOwner = null;

    //TODO: add Ammunation Inventory

    //TODO: add equipment manager to modifying the stats' values
    //GETTERS AND SETTERS
    public string modelName => _modelName;
    public WeaponType type => _type;
    public BulletType bulletType => _bulletType;
    public Transform playerOwner => _playerOwner;
    public IMagazineController magazine => _magazine ?? _nullMagazine;

    public int currentMagazine { get; set; }
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
    public event Action<Vector3, bool> OnCursorPositionReceived;

    // UNITY CALLBACKS
    protected virtual void Awake()
    {
        var subscribers = GetComponents<IOnObjectNotifier<Weapon>>();
        foreach(var sub in subscribers)
        {
            sub.OnAwakeCalled(this);
        }
    }

    //TODO: delete this test
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var text = GameObject.FindGameObjectWithTag("TextMeshPro").GetComponent<UnityEngine.UI.Text>();
            text.text = _stats.ToString();
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

    public void SetShootCursorPosition(Vector3 hitPoint, bool aimOnEnemy)
    {
        OnCursorPositionReceived?.Invoke(hitPoint, aimOnEnemy);
    }

    public void AttachMagazineController(MagazineController magazineController)
    {
        _magazine = magazineController;
    }

    public void ClearMagazineController()
    {
        _magazine = null;
    }

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

    public virtual void OnWeaponLoad()
    {
        if (_isAnimating == true)
            return;

        _isAnimating = true;
        gameObject.SetActive(true);

        OnWeaponLoaded?.Invoke();

        StartCoroutine(LoadAnimation(60, 0, true));
    }

    public virtual void OnWeaponUnload()
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
        _playerOwner = owner;
        OnOwnerChanged?.Invoke(owner != null);
    }

    protected void CreateBullet(float rotation = 0)
    {
        var bullet = Instantiate(_bulletPrefab, _muzzle.position, _muzzle.rotation);
        bullet.Initialize(this, rotation);
    }
}