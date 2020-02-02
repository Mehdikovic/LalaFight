using System.Collections;
using UnityEngine;

public class WeaponRecoilReloadAnimation : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private Weapon _weapon = null;
    
    [Header("Kick Amount")]
    [Range(0.1f, 0.6f)]
    [SerializeField] float _kickback = 0.1f;
    [Range(0f, 10f)]
    [SerializeField] float _kickup = 10f;

    private IMagazineController _magazine;

    private Vector3 _recoilPositionSmoothVelocity;
    private float _recoilRotationSmoothVelocity;
    private float _rotationAngleOverX;
    private bool _isReloaded = false;
    private float _reloadTime = 0;

    private void Awake()
    {
        _magazine = GetComponent<IMagazineController>() ?? new NullMagazine();
        _reloadTime = _magazine.reloadTime;
        
        _weapon.OnFireEnd += OnWeaponFired;
        _magazine.Reloading += OnMagazineReloading;
        _magazine.ReloadingCanceled += OnMagazineReloadingCanceled;
        _magazine.Reloaded += OnMagazineReloaded;
    }

    private void OnMagazineReloaded()
    {
        _isReloaded = false;
    }

    private void OnMagazineReloadingCanceled()
    {
        _isReloaded = false;
    }

    private void OnMagazineReloading()
    {
        _isReloaded = true;
        StartCoroutine(ReloadAnimation());
    }

    private void LateUpdate()
    {
        if (_weapon.isAnimating) 
            return;
        
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref _recoilPositionSmoothVelocity, 0.1f);
        _rotationAngleOverX = Mathf.SmoothDamp(_rotationAngleOverX, 0, ref _recoilRotationSmoothVelocity, 0.1f);
        
        if (_isReloaded == false)
            transform.localEulerAngles = _rotationAngleOverX * Vector3.left;
        
    }

    private void OnWeaponFired()
    {
        transform.localPosition += -Vector3.forward * _kickback;
        _rotationAngleOverX += _kickup;
        _rotationAngleOverX = Mathf.Clamp(_rotationAngleOverX, 0f, 10f);
    }

    private IEnumerator ReloadAnimation()
    {
        float speed = 1 / _reloadTime;

        Vector3 initialEulerAngle = transform.localEulerAngles;
        float reloadAngle = 20f;

        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;
            float interpolation = Utility.Interpolate(percent);
            float lerp = Mathf.Lerp(0, reloadAngle, interpolation);
            transform.localEulerAngles = initialEulerAngle + new Vector3(lerp, -2 * lerp, -lerp / 2);
            yield return null;
        }
    }
}
