using System.Collections.Generic;

public enum BulletType
{
    Small,
    Medium,
    Heavy
}

static public class BulletTypes
{
    static private Dictionary<BulletType, string> _names;

    static BulletTypes()
    {
        _names = new Dictionary<BulletType, string>()
        {
            {BulletType.Small, "Small" },
            {BulletType.Medium, "Medium" },
            {BulletType.Heavy, "Heavy" }
        };
    }

    static public Dictionary<BulletType, string> names => _names;
}