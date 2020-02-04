public class NullMagazine : IMagazineController
{
    #pragma warning disable 0067
    public event System.Action NoAmmunationAtInventory;
    public event System.Action Reloading;
    public event System.Action Reloaded;
    public event System.Action ReloadingCanceled;
    public event System.Action MagazineFull;
    #pragma warning restore 0067

    public int magazineSize => -1;

    public float reloadTime => -1f;

    public int currentMagazine => -1;

    public bool isReloading => false;

    public void SetCurrentMagazine(int amount)
    {
        
    }

    public bool ShootingAllowed()
    {
        return true;
    }

    public void CancelReloading()
    {
        
    }

    public void DecreaseAmmo()
    {
        
    }

    public void Reload()
    {
        
    }
}
