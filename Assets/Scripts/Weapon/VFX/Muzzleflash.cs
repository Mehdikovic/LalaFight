using UnityEngine;


namespace LalaFight
{
    public class Muzzleflash : MonoBehaviour
    {
        [SerializeField] private GameObject _GFXEffect = null;
        [SerializeField] private Sprite[] _sprites = null;
        [SerializeField] private SpriteRenderer[] _spriterenderers = null;
        [SerializeField] private float _lifetime = 0.1f;

        private Weapon _weapon = null;

        private void Awake()
        {
            _GFXEffect.SetActive(false);
            _weapon = GetComponentInParent<Weapon>();
            _weapon.OnFireEnd += OnWeaponFire;
        }

        private void OnWeaponFire()
        {
            Activate();
        }

        private void Activate()
        {
            int randomSpriteIndex = UnityEngine.Random.Range(0, _sprites.Length);
            for (int i = 0; i < _spriterenderers.Length; i++)
                _spriterenderers[i].sprite = _sprites[randomSpriteIndex];
            _GFXEffect.SetActive(true);
            Invoke("Deactivate", _lifetime);
        }

        private void Deactivate()
        {
            _GFXEffect.SetActive(false);
        }
    }
}