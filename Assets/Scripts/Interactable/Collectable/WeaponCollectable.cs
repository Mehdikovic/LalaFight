using UnityEngine;

public class WeaponCollectable : Interactable
{
    [SerializeField] private InventoryItemWeapon _weapon = null;
    [SerializeField] private int _rounds = 10;

    //GETTERS AND SETTERS
    public int rounds { get => _rounds; set => _rounds = value; }

    public override void Interact(Transform player)
    {
        var weaponManager = player.GetComponent<WeaponManager>();

        if (weaponManager == null)
        {
            Debug.Log("Player doesn't have WeaponManager");
            return;
        }

        var mountInfo = new WeaponMountInfo() { inventoryWeapon = _weapon, rounds = _rounds };

        var returndMountInfo = weaponManager.AddInventoryItem(mountInfo);

        if (returndMountInfo.inventoryWeapon != _weapon)
        {
            if (returndMountInfo)
                SpawnCollectable(returndMountInfo, player.position.WithY(1.5f), player.forward);
            Destroy(gameObject);
            return;
        }
    }

    private void SpawnCollectable(WeaponMountInfo mountInfo, Vector3 position, Vector3 dir)
    {
        WeaponCollectable collectable = Instantiate(mountInfo.inventoryWeapon.collectable, position, Quaternion.identity);
        collectable.rounds = mountInfo.rounds;
        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.AddForce(dir * 2000);
            rigidbody.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 300);
        }
    }
}
