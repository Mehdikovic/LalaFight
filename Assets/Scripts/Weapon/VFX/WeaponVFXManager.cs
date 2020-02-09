using UnityEngine;


namespace LalaFight
{
    public class WeaponVFXManager : MonoBehaviour, IOnObjectNotifier<Weapon>
    {
        [Header("Shell")]
        [SerializeField] private Transform _shellSpawnerPoint = null;
        [SerializeField] private BulletShellVFX _bulletShellPrefab = null;

        [Header("Clip")]
        [SerializeField] private Transform _clipSpawnerPoint = null;
        [SerializeField] private GameObject _clipPrefab = null;

        private Weapon _weapon = null;
        private IMagazineController _magazine = null;

        public void OnAwakeCalled(Weapon weapon)
        {
            _weapon = weapon;

            _magazine = GetComponent<IMagazineController>() ?? new NullMagazine();

            _weapon.OnFireEnd += OnWeaponFire;
            _magazine.Reloading += OnMagazineReloading;
        }

        private void OnMagazineReloading()
        {
            Instantiate(_clipPrefab, _clipSpawnerPoint.position, _clipSpawnerPoint.rotation);
        }

        private void OnWeaponFire()
        {
            var rotation = Quaternion.AngleAxis(Random.Range(-60f, 60f), Vector3.up) * _shellSpawnerPoint.rotation;
            Instantiate(_bulletShellPrefab, _shellSpawnerPoint.position, rotation);
        }
    }
}