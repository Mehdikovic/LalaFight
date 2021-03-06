using UnityEngine;
using UnityEngine.UI;


namespace LalaFight
{
    public class WeaponReloadTimerUI : MonoBehaviour
    {
        [SerializeField] private GameObject _UIparent = null;
        [SerializeField] private Image _timerImage = null;
        [SerializeField] private Text _timerText = null;

        private IMagazineController _magazine;
        private bool _isReloading = false;
        private float _passedTime = 0f;

        private void Awake()
        {
            SetUIVisibility(false);
        }

        public void Bind(Weapon weapon)
        {
            if (weapon == null)
                return;
            
            _magazine = weapon.magazine;

            SetUIVisibility(false);
            RegisterEvents();
        }

        public void UnBind()
        {
            if (_magazine == null)
                return;
            
            SetUIVisibility(false);
            ClearEvents();
            
            _magazine = null;
        }

        private void RegisterEvents()
        {
            _magazine.Reloading += OnMagazineReloading;
            _magazine.Reloaded += OnMagazineReloaded;
            _magazine.ReloadingCanceled += OnMagazineReloadingCanceled;
        }

        private void ClearEvents()
        {
            _magazine.Reloading -= OnMagazineReloading;
            _magazine.Reloaded -= OnMagazineReloaded;
            _magazine.ReloadingCanceled -= OnMagazineReloadingCanceled;
        }


        private void Update()
        {
            if (!_isReloading)
                return;

            _passedTime += Time.deltaTime;
            float remainingTime = _magazine.reloadTime - _passedTime;
            _timerText.text = (remainingTime > 1f) ? ((int)(remainingTime)).ToString() : remainingTime.ToString("F1");
            _timerImage.fillAmount = _passedTime / _magazine.reloadTime;

            if (remainingTime <= 0)
                OnMagazineReloaded();
        }


        private void OnMagazineReloadingCanceled()
        {
            _isReloading = false;
            _passedTime = 0f;
            SetUIVisibility(false);
        }

        private void OnMagazineReloaded()
        {
            _isReloading = false;
            _passedTime = 0f;
            SetUIVisibility(false);
        }

        private void OnMagazineReloading()
        {
            _isReloading = true;
            SetUIVisibility(true);
        }

        private void SetUIVisibility(bool visible)
        {
            _UIparent.SetActive(visible);
        }
    }
}