using System.Collections.Generic;

public enum FireModeType
{
    Semi,
    Burst,
    Auto
}

static public class FireMode
{
    static private Dictionary<FireModeType, string> _names;

    static FireMode()
    {
        _names = new Dictionary<FireModeType, string>()
        {
            {FireModeType.Auto, "Auto" },
            {FireModeType.Burst, "Burst" },
            {FireModeType.Semi, "Semi" }
        };
    }

    static public Dictionary<FireModeType, string> Names => _names;
}
