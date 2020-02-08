public interface IMagazineController
{
    event System.Action NoAmmunationAtInventory;
    event System.Action Reloading;
    event System.Action Reloaded;
    event System.Action ReloadingCanceled;
    event System.Action MagazineFull;

    int magazineSize { get; }
    float reloadTime { get; }
    int currentMagazine { get; }
    bool isReloading { get; }

    bool ShootingAllowed();
    void CancelReloading();
    void DecreaseAmmo();
    void Reload();
}
