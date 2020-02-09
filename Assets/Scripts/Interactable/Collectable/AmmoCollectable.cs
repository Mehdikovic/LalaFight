using UnityEngine;


namespace LalaFight
{
    public class AmmoCollectable : Interactable
    {
        [SerializeField] private BulletType _bulletType = BulletType.Medium;
        [SerializeField] private int _rounds = 10;

        public int Rounds { get => _rounds; set => _rounds = value; }

        public override void Interact(Transform player)
        {
            var ammoManager = player.GetComponent<AmmoManager>();
            
            if (ammoManager != null)
                ammoManager.AddAmmoToInventory(_bulletType, _rounds);
            
            Destroy(gameObject);
        }
    }
}